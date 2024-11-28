using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        UserManager<IdentityUser> _userManger;
        RoleManager<IdentityRole> _roleManager;
        public AdminController(UserManager<IdentityUser> _userManger, RoleManager<IdentityRole> _roleManager)
        {
            this._userManger = _userManger;
            this._roleManager = _roleManager;

        }

        

    }
}
