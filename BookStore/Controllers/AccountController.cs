using BookStore.DTOs.accountDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        SignInManager<IdentityUser> signinManger;
        UserManager<IdentityUser> userManager;
        public AccountController(SignInManager<IdentityUser> signinManger, UserManager<IdentityUser> userManager)
        {
            this.signinManger = signinManger;   
            this.userManager = userManager;
        }

        [HttpPost]
        public IActionResult Login(loginDTO loginDTO) 
        {
            if (loginDTO == null) { return BadRequest(); }
          
            var loginResult = signinManger.PasswordSignInAsync(loginDTO.Username, loginDTO.Password, false, false).Result;
            if (loginResult.Succeeded) 
            {
               var user = userManager.FindByNameAsync(loginDTO.Username).Result;
                #region generate Token

                #region claims
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim( ClaimTypes.Name, user.UserName));
                claims.Add(new Claim( ClaimTypes.NameIdentifier, user.Id));
                
                var roles = userManager.GetRolesAsync(user).Result;
                foreach (var role in roles) { 
                
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                #endregion
                #region secretkey and signature

                string key = "Mohamed Salah is making this secret key using HmacSha256";
                var secretKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

                var signature = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                #endregion

                // generate token
                var token = new JwtSecurityToken(

                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: signature

                    );
                
                // token object

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(tokenString);
                
                
                #endregion



            }
            else return Unauthorized("Invalid username or password");
        }
        [HttpPost("changepass")]
        [Authorize]
        public IActionResult changePassword(changePasswordDTO changePasswordDTO) 
        {
            var userName = User.Identity.Name;
            if (ModelState.IsValid)
            {
                var user = userManager.FindByNameAsync(userName).Result;
                var changeResult = userManager.ChangePasswordAsync(user, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword).Result;

                if (changeResult.Succeeded) 
                    { return Ok(); }
                else 
                    { return BadRequest(changeResult.Errors); }
            }
            else return BadRequest(ModelState);
            
        
        }

        [HttpGet("logout")]
        [Authorize]
        public IActionResult logout() 
        {
            
            signinManger.SignOutAsync();
            return Ok();
        
        }
        
    }
}
