using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using BluePrint.Server;
using BluePrint.EF;
using Microsoft.EntityFrameworkCore;
using Okta.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using BluePrint.Server.Areas.Identity.CustomProvider;
using Microsoft.AspNetCore.Components.Authorization;

namespace BluePrint.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddDbContext<BluePrintOracleContext>(options => options.UseOracle(Configuration.GetConnectionString("BluePrintOracleConnection")));

            services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));

            services.AddMvc()
                .AddJsonOptions(
                    options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                );

            //  Old way of doing auth
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
            //    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
            //    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
            //})
            //    .AddOktaWebApi(new OktaWebApiOptions()
            //    {
            //        OktaDomain = Configuration["Okta:OktaDomain"]
            //    });

            //      https://www.syncfusion.com/faq/blazo/general/how-do-i-implement-blazor-authentication-with-openid-connect
            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

                //opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddCookie().AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.Authority   = Configuration.GetValue<string>("Okta:OktaDomain");
                options.RequireHttpsMetadata = true;
                options.ClientId = Configuration.GetValue<string>("Okta:ClientId");
                options.ClientSecret = Configuration.GetValue<string>("Okta:ClientSecret");
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.UseTokenLifetime = false;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");

                //options.TokenValidationParameters = new TokenValidationParameters { NameClaimType = "name" };

                options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
                options.NonceCookie.SameSite = SameSiteMode.Unspecified;


                options.Events = new OpenIdConnectEvents
                {
                    // OnRedirectToIdentityProvider handler is needed so that we properly route the user
                    // through our "ExternalLogin" logic, which will sign them in and redirect them to the
                    // requested page.
                    OnRedirectToIdentityProvider = context =>
                    {
                        string uri = context.Properties.RedirectUri;
                        if (!uri.Contains("ExternalLogin"))
                        {
                            context.Properties.RedirectUri = $"/Identity/Account/ExternalLogin?returnUrl={context.Properties.RedirectUri}&handler=Callback";
                            context.Properties.Items.Add("LoginProvider", "OpenIdConnect");
                            context.Properties.IsPersistent = true;
                        }
                        return Task.CompletedTask;
                    },

                    // OnTokenValidated is not needed for anything, but is good informational stuff
                    OnTokenValidated = context =>
                    {
                        var uri = context.Properties.RedirectUri;

                        var http = context.HttpContext;
                        var idToken = context.SecurityToken;
                        string userIdentifier = idToken.Subject;
                        string userEmail =
                            idToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value
                            ?? idToken.Claims.SingleOrDefault(c => c.Type == "preferred_username")?.Value;

                        string firstName = idToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.GivenName)?.Value;
                        string lastName = idToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.FamilyName)?.Value;
                        string name = idToken.Claims.SingleOrDefault(c => c.Type == "name")?.Value;

                            // manage roles, modify token and claims etc.

                            return Task.CompletedTask;
                    },

                    // TODO: update this handler
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.Redirect("/Home/Error");
                        context.HandleResponse(); // Suppress the exception
                            return Task.CompletedTask;
                    },
                };





            });

            //Vanilla
            services.AddDefaultIdentity<IdentityUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<BluePrintOracleContext>();

            //// Add identity types
            //services.AddIdentity<ApplicationUser, ApplicationRole>()
            //    // .AddSignInManager<ApplicationSignInManager<ApplicationUser>>()
            //    .AddRoleManager<ApplicationRoleManager<ApplicationRole>>()
            //    .AddUserManager<ApplicationUserManager<ApplicationUser>>()
            //    .AddDefaultTokenProviders();

            // Identity Services
            services.AddTransient<IUserStore<ApplicationUser>, AspNetUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, AspNetRoleStore>();

            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

            //services.AddSingleton<AspNetUserStore>();
            //services.AddSingleton<AspNetRoleStore>();


            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //})

            //.AddOpenIdConnect(options =>
            //{
            //    string env = Configuration.GetSection("GlobalSettings:Environment").Value;
            //    string prodSuffix = env == "Production" ? "_prod" : "";

            //    options.CallbackPath = "/authorization-code/callback";
            //    options.SignInScheme = IdentityConstants.ExternalScheme;// CookieAuthenticationDefaults.AuthenticationScheme;

            //    options.Authority = Configuration[$"okta_domain{prodSuffix}"];
            //    options.RequireHttpsMetadata = true;
            //    options.ClientId = Configuration[$"okta_client_id{prodSuffix}"];
            //    options.ClientSecret = Configuration[$"okta_client_secret{prodSuffix}"];

            //    options.ResponseType = OpenIdConnectResponseType.Code;
            //    options.GetClaimsFromUserInfoEndpoint = true;
            //    options.Scope.Add("openid");
            //    options.Scope.Add("profile");
            //    options.Scope.Add("email");

            //    options.SaveTokens = true;

            //    // The following 2 lines are needed to deal with "Correlation" error when redirecting back from Okta
            //    // This error occurs on older Chrome browsers (< version 80)
            //    options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
            //    options.NonceCookie.SameSite = SameSiteMode.Unspecified;


            //    options.Events = new OpenIdConnectEvents
            //    {
            //        // OnRedirectToIdentityProvider handler is needed so that we properly route the user
            //        // through our "ExternalLogin" logic, which will sign them in and redirect them to the
            //        // requested page.
            //        OnRedirectToIdentityProvider = context =>
            //        {
            //            string uri = context.Properties.RedirectUri;
            //            if (!uri.Contains("ExternalLogin"))
            //            {
            //                context.Properties.RedirectUri = $"/Identity/Account/ExternalLogin?returnUrl={context.Properties.RedirectUri}&handler=Callback";
            //                context.Properties.Items.Add("LoginProvider", "OpenIdConnect");
            //                context.Properties.IsPersistent = true;
            //            }
            //            return Task.CompletedTask;
            //        },

            //        // OnTokenValidated is not needed for anything, but is good informational stuff
            //        OnTokenValidated = context =>
            //        {
            //            var uri = context.Properties.RedirectUri;

            //            var http = context.HttpContext;
            //            var idToken = context.SecurityToken;
            //            string userIdentifier = idToken.Subject;
            //            string userEmail =
            //                idToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value
            //                ?? idToken.Claims.SingleOrDefault(c => c.Type == "preferred_username")?.Value;

            //            string firstName = idToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.GivenName)?.Value;
            //            string lastName = idToken.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.FamilyName)?.Value;
            //            string name = idToken.Claims.SingleOrDefault(c => c.Type == "name")?.Value;

            //            // manage roles, modify token and claims etc.

            //            return Task.CompletedTask;
            //        },

            //        // TODO: update this handler
            //        OnAuthenticationFailed = context =>
            //        {
            //            context.Response.Redirect("/Home/Error");
            //            context.HandleResponse(); // Suppress the exception
            //            return Task.CompletedTask;
            //        },
            //    };

            //});

            //services.AddDefaultIdentity<IdentityUser>()
            //        .AddRoles<IdentityRole>()
            //        .AddEntityFrameworkStores<BluePrintOracleContext>();


            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.User.RequireUniqueEmail = true;
            //});



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

        }
    }
}
