using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AngularTemplate.Models.Database;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace AngularTemplate.Models.Stores
{
    public class InsightUserStore :
        IUserPhoneNumberStore<ApplicationUser, int>,
        IUserPasswordStore<ApplicationUser, int>,
        IUserLoginStore<ApplicationUser, int>,
        IUserRoleStore<ApplicationUser, int>,
        IUserClaimStore<ApplicationUser, int>,
        IUserEmailStore<ApplicationUser, int>,
        IUserLockoutStore<ApplicationUser, int>,
        IUserSecurityStampStore<ApplicationUser, int>,
        IUserTwoFactorStore<ApplicationUser, int>
    {
        private readonly IUserDataAccess _userData;

        public InsightUserStore(IUserDataAccess userData)
        {
            _userData = userData;
        }


        public void Dispose()
        {
        }

        public Task CreateAsync(ApplicationUser user)
        {
            return _userData.InsertUserAsync(user);
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            return _userData.UpdateUserAsync(user);
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            return _userData.DeleteUserAsync(user.Id);
        }

        public async Task<ApplicationUser> FindByIdAsync(int userId)
        {
            var user = await _userData.SelectUserAsync(userId);
            return user;
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = await _userData.FindUserByUserNameAsync(userName);
            return user;
        }

        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            var userLogin = new UserLogin
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };

            return _userData.InsertUserLoginAsync(userLogin);
        }

        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            var userLogin = new UserLogin
            {
                UserId = user.Id,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            };

            return _userData.DeleteUserLoginAsync(userLogin);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            var logins = await _userData.GetLoginsForUserAsync(user.Id);

            return logins
                .Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey))
                .ToList();
        }

        public async Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            var user = await _userData.FindUserByLoginAsync(login.LoginProvider, login.ProviderKey);
            return user;
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            return Task.FromResult(user.PasswordHash = passwordHash);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task AddToRoleAsync(ApplicationUser user, string role)
        {
            return _userData.AddUserToRoleAsync(user.Id, role);
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            return _userData.RemoveUserFromRoleAsync(user.Id, role);
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return _userData.GetRolesForUserAsync(user.Id);
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string role)
        {
            return _userData.IsUserInRoleAsync(user.Id, role);
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            // get claims from database table
            var userClaims = await _userData.GetUserClaimsAsync(user.Id);
            var claims = userClaims.Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList();

            // add other app specific claims
            if (user.Name != null)
                claims.Add(new Claim(ClaimTypes.GivenName, user.Name));

            return claims;
        }

        public async Task AddClaimAsync(ApplicationUser user, Claim claim)
        {
            await _userData.InsertUserClaimAsync(new UserClaim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            });
        }

        public async Task RemoveClaimAsync(ApplicationUser user, Claim claim)
        {
            await _userData.RemoveUserClaimAsync(user.Id, claim.Type);
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            return Task.FromResult(user.Email = email);
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return Task.FromResult(user.EmailConfirmed = confirmed);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userData.FindUserByEmailAsync(email);
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            return Task.FromResult(user.PhoneNumber = phoneNumber);
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return Task.FromResult(user.PhoneNumberConfirmed = confirmed);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return Task.FromResult(user.LockoutEndDate);
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            return Task.FromResult(user.LockoutEndDate = lockoutEnd);
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.FromResult(++user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.FromResult(user.AccessFailedCount = 0);
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(user.LockoutEnabled);

        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.FromResult(user.LockoutEnabled = enabled);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.FromResult(user.TwoFactorEnabled = enabled);
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);

        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            return Task.FromResult(user.SecurityStamp = stamp);
        }

        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }
    }
}