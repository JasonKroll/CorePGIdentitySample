using System;
using CorePGIdentityTest.Entities;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Identity;

namespace CorePGIdentityTest.Services
{
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> _userManager { get; }

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetOrCreateUser(string username)
        {
            var existingUser = await _userManager.FindByNameAsync(username);
            if (existingUser != null)
                return existingUser;

            var token = new CancellationToken();

            UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(username);
            var newUser = new ApplicationUser
            {
                Id = username,
                UserName = username,
                Email = userRecord.Email
            };

            var createUserResult = await _userManager.CreateAsync(newUser);
            //if (createUserResult.Succeeded)
            //{
            //}
            return newUser;

        }
    }
}

