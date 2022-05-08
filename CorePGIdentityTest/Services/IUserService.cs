using System;
using CorePGIdentityTest.Entities;

namespace CorePGIdentityTest.Services
{
    public interface IUserService
    {
        public Task<ApplicationUser> GetOrCreateUser(string userId);
    }
}

