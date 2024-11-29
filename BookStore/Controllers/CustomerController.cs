using BookStore.DTOs.customerDTO;
using BookStore.Models;
using BookStore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult deleteCustomer(string id)
        {
            var customer = userManager.FindByNameAsync(id).Result;
            if(customer == null) { return NotFound(); }
            else
            {

                userManager.DeleteAsync(customer);
                return Ok();
            }
        
        }
    }
}
