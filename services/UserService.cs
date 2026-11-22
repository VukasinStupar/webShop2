
using Microsoft.AspNetCore.Identity;
using webShop2.model;
using webShop2.model.enums;
using webShop2.repository.core;
using webShop2.services.core;

namespace webShop2.services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public UserService(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _unitOfWork.Users.GetAllAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _unitOfWork.Users.GetByEmailAsync(email);
        }

        public async Task<User?> UpdateAsync(int id, User userUpdate)
        {
            User? user = await _userManager.FindByIdAsync(
                id.ToString());

            if (user == null)
            {
                return null;
            }

            bool isAdmin = await _userManager.IsInRoleAsync(
                user,
                UserRoles.Admin);

            if (isAdmin && !userUpdate.IsActive)
            {
                int activeAdmins = await GetActiveAdminCountAsync();

                if (activeAdmins <= 1)
                {
                    throw new InvalidOperationException(
                        "The last active administrator cannot be deactivated.");
                }
            }

            user.FirstName = userUpdate.FirstName;
            user.LastName = userUpdate.LastName;
            user.IsActive = userUpdate.IsActive;

            if (!string.Equals(
                user.Email,
                userUpdate.Email,
                StringComparison.OrdinalIgnoreCase))
            {
                IdentityResult emailResult =
                    await _userManager.SetEmailAsync(
                        user,
                        userUpdate.Email);

                if (!emailResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join(
                            ", ",
                            emailResult.Errors.Select(
                                error => error.Description)));
                }

                IdentityResult usernameResult =
                    await _userManager.SetUserNameAsync(
                        user,
                        userUpdate.Email);

                if (!usernameResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join(
                            ", ",
                            usernameResult.Errors.Select(
                                error => error.Description)));
                }
            }

            user.UpdatedAt = DateTime.UtcNow;

            IdentityResult result =
                await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                string errors = string.Join(
                    ", ",
                    result.Errors.Select(
                        error => error.Description));

                throw new InvalidOperationException(errors);
            }

            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            User? user = await _userManager.FindByIdAsync(
                id.ToString());

            if (user == null)
            {
                return false;
            }

            bool isAdmin = await _userManager.IsInRoleAsync(
                user,
                UserRoles.Admin);

            if (isAdmin)
            {
                int activeAdmins = await GetActiveAdminCountAsync();

                if (activeAdmins <= 1 && user.IsActive)
                {
                    throw new InvalidOperationException(
                        "The last active administrator cannot be deleted.");
                }
            }

            IdentityResult result =
                await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                string errors = string.Join(
                    ", ",
                    result.Errors.Select(
                        error => error.Description));

                throw new InvalidOperationException(errors);
            }

            return true;
        }

        private async Task<int> GetActiveAdminCountAsync()
        {
            IList<User> admins =
                await _userManager.GetUsersInRoleAsync(
                    UserRoles.Admin);

            return admins.Count(user => user.IsActive);
        }
    }
}
