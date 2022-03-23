using AutoMapper;
using BluePrint.EF;
//using BluePrint.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BluePrint.Server.Areas.Identity.CustomProvider
{
    public class AspNetUserStore : IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>
    {
        private readonly BluePrintOracleContext _context;
        private readonly IMapper _mapper;

        public AspNetUserStore(BluePrintOracleContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Dispose()
        {
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            _context.Add(user);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not create user {user.UserName}." });
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            var userFromDb = await _context.AspNetUsers.FindAsync(user.Id);
            _context.Remove(userFromDb);

            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not delete user {user.UserName}." });
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var AspUser = _context.AspNetUsers.SingleOrDefaultAsync(u => u.Id.Equals(Guid.Parse(userId)), cancellationToken);
            var _ApplicationUser = _mapper.Map<ApplicationUser>(AspUser);

            _ApplicationUser.AuthenticationType = "OKTA";
            _ApplicationUser.IsAuthenticated = true;
            _ApplicationUser.Name = AspUser.Result.UserName;

            return _ApplicationUser;

        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var AspUser = _context.AspNetUsers.SingleOrDefaultAsync(u => u.UserName.Equals(normalizedUserName.ToLower()),
                cancellationToken);
            var _ApplicationUser = _mapper.Map<ApplicationUser>(AspUser);

            _ApplicationUser.AuthenticationType = "OKTA";
            _ApplicationUser.IsAuthenticated = true;
            _ApplicationUser.Name = AspUser.Result.UserName;

            return _ApplicationUser;

        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UserName);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.UserName = userName;
            return Task.FromResult<object>(null);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            _context.Update(user);
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            return affectedRows > 0
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError() { Description = $"Could not update user {user.UserName}." });
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();


            var AspUser = _context.AspNetUsers.SingleOrDefaultAsync(u => u.UserName.Equals(normalizedEmail),
                cancellationToken);
            var _ApplicationUser = _mapper.Map<ApplicationUser>(AspUser);

            _ApplicationUser.AuthenticationType = "OKTA";
            _ApplicationUser.IsAuthenticated = true;
            _ApplicationUser.Name = AspUser.Result.UserName;

            return _ApplicationUser;
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UserName);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UserName);
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.UserName = email;
            return Task.FromResult<object>(null);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<object>(null);
        }
    }
}
