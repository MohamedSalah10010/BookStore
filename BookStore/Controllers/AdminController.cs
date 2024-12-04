using BookStore.DTOs.adminDTO;
using BookStore.DTOs.customerDTO;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
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
        [SwaggerOperation(Summary = "Create a new admin", Description = "This endpoint allows an admin user to create a new admin. Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Admin created successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid admin data or user creation failed.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [Produces("application/json")]
        [Consumes("application/json")]
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
        [SwaggerOperation(Summary = "Edit admin profile", Description = "This endpoint allows an admin to update their profile information (email, username, phone number). Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Admin profile updated successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid profile data.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [Produces("application/json")]
        [Consumes("application/json")]
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
        [SwaggerOperation(Summary = "Get admin by ID", Description = "This endpoint retrieves an admin's profile by their ID. Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Admin data retrieved successfully.", typeof(SelectAdminDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Admin not found.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [Produces("application/json")]
        
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
        [SwaggerOperation(Summary = "Get all admins", Description = "This endpoint retrieves a list of all admins. Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of admins retrieved successfully.", typeof(List<SelectAdminDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No admins found.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [Produces("application/json")]
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

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an admin by ID", Description = "This endpoint allows an admin user to delete another admin's account. Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Admin deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Admin not found.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [Produces("application/json")]
        public IActionResult deleteAdmin(string id)
        {
            var admin = userManager.FindByIdAsync(id).Result;
            if (admin == null)
            {
                return NotFound();
            }
            else
            {
                // Remove roles associated with the user
                var roles = userManager.GetRolesAsync(admin).Result;
                foreach (var role in roles)
                {
                    userManager.RemoveFromRoleAsync(admin, role).Wait();
                }
                var res = userManager.DeleteAsync(admin).Result;
                if (res.Succeeded)
                {
                    return Ok();
                }
                else
                { return BadRequest(res.Errors); }
            }
        }
    }
}
