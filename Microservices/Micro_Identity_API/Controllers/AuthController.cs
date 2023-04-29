using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Security.Claims;

namespace Micro_Identity_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("signUpUser")]
        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // create a new IdentityUser
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };

                // create a new user account with IdentityUserManager
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // assign a role to the user
                await AssignRoleToUser("UserRole", user);

                // assign a clamis to the user
                await AssignClaims(user, model, "UserRole");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("signUpAdmin")]
        public async Task<IActionResult> SignUpAdmin([FromBody] SignUpViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // create a new IdentityUser
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };

                // create a new user account with IdentityUserManager
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                // assign a role to the user
                await AssignRoleToUser("AdminRole", user);

                // assign a clamis to the user
                await AssignClaims(user, model, "AdminRole");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [NonAction]
        public async Task AssignClaims(IdentityUser user, SignUpViewModel model, string roleName)
        {
            await _userManager.AddClaimsAsync(user, new Claim[]
            {
               new Claim(JwtClaimTypes.Name, model.FullName),
               new Claim(JwtClaimTypes.GivenName, model.FirstName),
               new Claim(JwtClaimTypes.FamilyName, model.LastName),
               new Claim(JwtClaimTypes.Email, model.Email),
               new Claim(JwtClaimTypes.Role, roleName),
               new Claim(JwtClaimTypes.WebSite, ""),
            });
        }

        [NonAction]
        public async Task AssignRoleToUser(string roleName, IdentityUser user)
        {
            // assign a role to the user
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}
