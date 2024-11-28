using BookStore.DTOs.adminDTO;
using BookStore.DTOs.customerDTO;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="admin")]
    public class AdminController : ControllerBase
    {
        UserManager<IdentityUser> userManager;
        RoleManager<IdentityRole> roleManager;
        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;

        }

        [HttpPost]
        public IActionResult createAdmin(addAdminDTO adminDTO)
        {

            Admin admin = new Admin()
            {

                Email = adminDTO.Email,
                UserName = adminDTO.Username,
                PhoneNumber = adminDTO.Phonenumber
            };
            var createAdminPass = userManager.CreateAsync(admin, adminDTO.Password).Result;
            if (createAdminPass.Succeeded)
            {

                var addingToAdminResult = userManager.AddToRoleAsync(admin, "admin").Result;
                if (addingToAdminResult.Succeeded)
                {

                    return Ok(addingToAdminResult);

                }
                else
                {
                    return BadRequest(addingToAdminResult.Errors);
                }
            }
            else
            {
                return BadRequest(createAdminPass.Errors);
            }

        }
        //[HttpGet]
        //public IActionResult getAdmin()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}
        [HttpPut]
        public IActionResult editAdminProfile(EditAdminDTO adminDTO)
        {
            if (ModelState.IsValid)
            {
                //var admin = (Admin)userManager.FindByIdAsync(adminDTO.Id).Result;
                var admin = userManager.FindByNameAsync(User.Identity.Name).Result;
                if (admin == null)
                {
                    return NotFound();
                }

                admin.Email = adminDTO.Email;
                admin.PhoneNumber = adminDTO.PhoneNumber;
                admin.UserName = adminDTO.UserName;
               

                var updateAdmin = userManager.UpdateAsync(admin).Result;
                if (updateAdmin.Succeeded)
                {
                    return NoContent();
                }
                else return BadRequest(updateAdmin.Errors);




            }
            else { return BadRequest(ModelState); }

        }
       
        [HttpGet("{id}")]
        public IActionResult getAdminById(string id)
        {
            var user = (Admin)userManager.GetUsersInRoleAsync("admin").Result.Where(c => c.Id == id).FirstOrDefault();

            if (user == null) { return NotFound(); }
            SelectAdminDTO adminDTO = new SelectAdminDTO()
            {
                UserName = user.UserName,
                Email = user.Email,
                ID = user.Id,
                PhoneNumber = user.PhoneNumber
            };
            return Ok(adminDTO);
        }

        [HttpGet]
        public IActionResult getAdmins()
        {
            var admins = userManager.GetUsersInRoleAsync("admin").Result.OfType<Admin>().ToList();
            if (!admins.Any()) { return NotFound(); }
            List<SelectAdminDTO> adminsDTO = new List<SelectAdminDTO>();
            foreach (var admin in admins)
            {

                SelectAdminDTO adminDTO = new SelectAdminDTO()
                {

                    Email = admin.Email,
                    ID = admin.Id,
                    PhoneNumber = admin.PhoneNumber,
                    UserName = admin.UserName

                };
                adminsDTO.Add(adminDTO);


            }
            return Ok(adminsDTO);

        }

    }
}
