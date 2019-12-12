using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WikidataGame.Backend.Controllers;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Validators
{
    public class UserValidator : UserValidator<User>
    {
        public async override Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            var result = await base.ValidateAsync(manager, user);
            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            if (user.UserName.Replace($"{AuthController.AnonPrefix}-",string.Empty).Length < 3)
            {
                errors.Add(new IdentityError
                {
                    Description = "Username must have at least 3 characters"
                });
            }
            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
