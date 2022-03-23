using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BluePrint.EF;
using System.Threading;
using System.Security.Claims;

namespace BluePrint.Server.Areas.Identity.CustomProvider
{
    public class AspNetRoleStore : IRoleStore<ApplicationRole>
    {
        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }






    //public class AspNetRoleStore //:  IRoleStore<AspNetRole>,  IRoleClaimStore<AspNetRole>
    // {
    /*
    private readonly BluePrintOracleContext _context;

    public AspNetRoleStore(BluePrintOracleContext context)
    {
        _context = context;
    }

    public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        try
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var roleEntity = getRoleEntity(role);

            _context.Add(roleEntity);
            _context.SaveChangesAsync();

            return Task.FromResult(IdentityResult.Success);
        }
        catch (Exception ex)
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
        }
    }

    public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        try
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            _context.Remove(role.Id);
            _context.SaveChangesAsync();

            return Task.FromResult(IdentityResult.Success);
        }
        catch (Exception ex)
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
        }
    }

    public Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        if (cancellationToken != null)
            cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(roleId))
            throw new ArgumentNullException(nameof(roleId));

        if (!Guid.TryParse(roleId, out Guid id))
            throw new ArgumentOutOfRangeException(nameof(roleId), $"{nameof(roleId)} is not a valid GUID");

        var roleEntity = _context.AspNetRoles.Find(id.ToString());
        return Task.FromResult(getIdentityRole(roleEntity));
    }

    public Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        if (cancellationToken != null)
            cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(normalizedRoleName))
            throw new ArgumentNullException(nameof(normalizedRoleName));

        var roleEntity = _context.RoleRepository.FindByName(normalizedRoleName);
        return Task.FromResult(getIdentityRole(roleEntity));
    }

    public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        if (cancellationToken != null)
            cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        return Task.FromResult(role.NormalizedName);
    }

    public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        if (cancellationToken != null)
            cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        return Task.FromResult(role.Id);
    }

    public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        if (cancellationToken != null)
            cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        return Task.FromResult(role.Name);
    }

    public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
    {
        if (cancellationToken != null)
            cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        role.NormalizedName = normalizedName;

        return Task.CompletedTask;
    }

    public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
    {
        if (cancellationToken != null)
            cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        role.Name = roleName;

        return Task.CompletedTask;
    }

    public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
    {
        try
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var roleEntity = getRoleEntity(role);

            _context.RoleRepository.Update(roleEntity);
            _context.Commit();

            return Task.FromResult(IdentityResult.Success);
        }
        catch (Exception ex)
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = ex.Message, Description = ex.Message }));
        }
    }

    public void Dispose()
    {
        // Lifetimes of dependencies are managed by the IoC container, so disposal here is unnecessary.
    }
#endregion

    #region IRoleClaimStore<IdentityRole> Members
    public Task<IList<Claim>> GetClaimsAsync(IdentityRole role, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (cancellationToken != null)
            cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        IList<AspNetRoleClaim> result = _context.RoleClaimRepository.FindByRoleId(role.Id).Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList();

        return Task.FromResult(result);
    }

    public Task AddClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (cancellationToken != null)
            cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        if (claim == null)
            throw new ArgumentNullException(nameof(claim));

        var roleClaimEntity = new RoleClaim
        {
            ClaimType = claim.Type,
            ClaimValue = claim.Value,
            RoleId = role.Id
        };

        _context.RoleClaimRepository.Add(roleClaimEntity);
        _context.Commit();

        return Task.CompletedTask;
    }

    public Task RemoveClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (cancellationToken != null)
            cancellationToken.ThrowIfCancellationRequested();

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        if (claim == null)
            throw new ArgumentNullException(nameof(claim));

        var roleClaimEntity = _context.RoleClaimRepository.FindByRoleId(role.Id)
            .SingleOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);

        if (roleClaimEntity != null)
        {
            _context.RoleClaimRepository.Remove(roleClaimEntity.Id);
            _context.Commit();
        }

        return Task.CompletedTask;
    }
    #endregion

    #region Private Methods
    private AspNetRole getRoleEntity(IdentityRole value)
    {
        return value == null
            ? default(AspNetRole)
            : new AspNetRole
            {
                ConcurrencyStamp = value.ConcurrencyStamp,
                Id = value.Id,
                Name = value.Name,
                NormalizedName = value.NormalizedName
            };
    }

    private IdentityRole getIdentityRole(AspNetRole value)
    {
        return value == null
            ? default(IdentityRole)
            : new IdentityRole
            {
                ConcurrencyStamp = value.ConcurrencyStamp,
                Id = value.Id,
                Name = value.Name,
                NormalizedName = value.NormalizedName
            };
    }

    public Task<IdentityResult> CreateAsync(AspNetRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(AspNetRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(AspNetRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetRoleIdAsync(AspNetRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetRoleNameAsync(AspNetRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetRoleNameAsync(AspNetRole role, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetNormalizedRoleNameAsync(AspNetRole role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedRoleNameAsync(AspNetRole role, string normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<AspNetRole> IRoleStore<AspNetRole>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<AspNetRole> IRoleStore<AspNetRole>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<Claim>> GetClaimsAsync(AspNetRole role, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task AddClaimAsync(AspNetRole role, Claim claim, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveClaimAsync(AspNetRole role, Claim claim, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    #endregion

}
    */
    //  }
}
