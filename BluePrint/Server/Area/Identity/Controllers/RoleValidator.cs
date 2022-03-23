using BluePrint.EF;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BluePrint.Server.Area.Identity.Controllers
{
    public sealed class RoleValidator : IAuthorizationFilter
    {
        private readonly IEnumerable<string> _roles;
        private BluePrintOracleContext _BluePrintOracleContext;

        public RoleValidator(BluePrintOracleContext context, params string[] roles)
        {
            _BluePrintOracleContext = context;
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.HttpContext.User.Claims == null || filterContext.HttpContext.User.Claims?.Count() <= 0)
            {
                filterContext.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;
            }

            var eventId = (filterContext.HttpContext.Request.Path).ToString().Replace("/api/events/", string.Empty);
            if (!String.IsNullOrWhiteSpace(eventId))
            {
                int charLocation = eventId.IndexOf("/", StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    eventId = eventId.Substring(0, charLocation);
                }
            }

            if (CheckUserRoles(filterContext.HttpContext.User.Claims, eventId))
                return;

            filterContext.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
        }

        private bool CheckUserRoles(IEnumerable<Claim> claims, string eventId)
        {
            var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);

            string currentRoleFromUser = _BluePrintOracleContext.AspNetRoles.FirstOrDefault(r =>
                r.Id == _BluePrintOracleContext.AspNetUserRoles.FirstOrDefault(u =>
                    u.UserId.ToString() == userIdClaim.Value)
                    .RoleId)?.Name;

            bool validUser = false;
            if (_roles.Any(r => r == currentRoleFromUser))
            {
                validUser = true;
            }

            return validUser;
        }
    }
}
