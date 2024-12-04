using BookStore.DTOs.customerDTO;
using BookStore.Models;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        UserManager<IdentityUser> userManager;
        RoleManager<IdentityRole> roleManager;
      


        public CustomerController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get all customers", Description = "This endpoint retrieves all customers from the system. Accessible only by users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of customers retrieved successfully.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No customers found.")]

        public IActionResult getCustomers()
        {
            var customers = userManager.GetUsersInRoleAsync("customer").Result.OfType<Customer>().ToList();
            if (!customers.Any()) { return NotFound(); }
            List<SelectCustomerDTO> custmoersDTO = new List<SelectCustomerDTO>();
            foreach (var customer in customers) {

                SelectCustomerDTO customerDTO = new SelectCustomerDTO()
                {

                    Address = customer.Address,
                    FullName = customer.FullName,
                    Email = customer.Email,
                    ID = customer.Id,
                    PhoneNumber = customer.PhoneNumber,
                    UserName = customer.UserName

                };
                custmoersDTO.Add(customerDTO);


            }
            return Ok(custmoersDTO);

        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Get customer by ID", Description = "This endpoint retrieves customer details by their ID. Accessible only by users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Customer details retrieved successfully.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Customer not found.")]
        public IActionResult getCustomerById(string id) {


            var user = (Customer)userManager.GetUsersInRoleAsync("customer").Result.Where(c => c.Id == id).FirstOrDefault();

            if (user == null) { return NotFound(); }
            SelectCustomerDTO customerDTO = new SelectCustomerDTO()
            {

                UserName = user.UserName,
                FullName = user.FullName,
                Address = user.Address,
                Email = user.Email,
                ID = user.Id,
                PhoneNumber = user.PhoneNumber
            };
            return Ok(customerDTO);
        }
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Create a new customer", Description = "This endpoint creates a new customer. Accessible to all authenticated users.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Customer created successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request - Invalid data.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]

        public IActionResult createCustomer(AddCustomerDTO customerDTO) {

            Customer customer = new Customer()
            {

                FullName = customerDTO.fullname,
                Address = customerDTO.address,
                Email = customerDTO.email,
                UserName = customerDTO.username,
                PhoneNumber = customerDTO.phonenumber
            };
            var createCustomerPass = userManager.CreateAsync(customer, customerDTO.password).Result;
            if (createCustomerPass.Succeeded)
            {

                var addingToCustomerResult = userManager.AddToRoleAsync(customer, "customer").Result;
                if (addingToCustomerResult.Succeeded)
                {

                    return Ok(addingToCustomerResult);

                }
                else
                {
                    return BadRequest(addingToCustomerResult.Errors);
                }
            }
            else
            {
                return BadRequest(createCustomerPass.Errors);
            }

        }

        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Edit a customer profile", Description = "This endpoint allows a customer to update their profile information.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Profile updated successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request - Invalid data.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Customer not found.")]
        public IActionResult editCutomerProfile(EditCustomerDTO customerDTO) 
        {
            if (ModelState.IsValid) 
            {
                var user = User.Identity.Name;
                var customer = (Customer) userManager.FindByNameAsync(user).Result;
                if (customer == null)
                {
                    return NotFound();
                }


                customer.FullName = customerDTO.FullName;
                customer.Email = customerDTO.Email;
                customer.PhoneNumber = customerDTO.PhoneNumber;
                customer.UserName = customerDTO.UserName;
                customer.Address = customerDTO.Address;

                var updateCustomer = userManager.UpdateAsync(customer).Result;
                if (updateCustomer.Succeeded)
                {
                    return NoContent();
                }
                else return BadRequest(updateCustomer.Errors);




            }
            else { return BadRequest(ModelState); }
            
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Delete a customer by ID", Description = "This endpoint allows an admin to delete a customer from the system by their ID. Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Customer deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Customer not found.")]

        public IActionResult deleteCustomer(string id)
        {
            var customer = userManager.FindByIdAsync(id).Result;
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                // Remove roles associated with the user
                var roles = userManager.GetRolesAsync(customer).Result;
                foreach (var role in roles)
                {
                    userManager.RemoveFromRoleAsync(customer, role).Wait();
                }
                var res = userManager.DeleteAsync(customer).Result;
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
