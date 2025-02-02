﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AngularSkilledHubProject.Models
{
        public class AuthRepository : IDisposable
        {
            private AuthContext _ctx;

            private UserManager<IdentityUser> _userManager;

            public AuthRepository()
            {
                _ctx = new AuthContext();
                _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            }

            // Register User
            public async Task<IdentityResult> RegisterUser(UserModel userModel)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = userModel.Username
                };

                var result = await _userManager.CreateAsync(user, userModel.Password);
                await this._userManager.AddToRoleAsync(user.Id, userModel.UserRole);

                return result;
            }

            // CHECKING IS USERNAME ALREADY EXISTS FOR REGISTRATION
            public async Task<bool> IsUsernameExists(string userName)
            {
                IdentityUser user = await _userManager.FindByNameAsync(userName);
                if(user == null )
                {
                    return false;
                }
                return true;
            }

            public async Task<IdentityUser> FindUser(string userName, string password)
            {
                IdentityUser user = await _userManager.FindAsync(userName, password);

                return user;
            }

            public void Dispose()
            {
                _ctx.Dispose();
                _userManager.Dispose();

            }
    }
}