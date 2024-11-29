using BookStore.DTOs.authorDTO;
using BookStore.DTOs.customerDTO;
using BookStore.Models;
using BookStore.Repos;
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
    public class AuthorController : ControllerBase
    {
        UserManager<IdentityUser> userManager;
        RoleManager<IdentityRole> roleManager;
        UnitWork unit;

        public AuthorController(UnitWork unit, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.unit = unit;

        }


        [HttpGet]
     
        [SwaggerOperation(Summary = "Get all authors", Description = "This endpoint retrieves a list of all authors. Requires authentication for access.")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of authors retrieved successfully.", typeof(List<SelectAuthorDTO>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No authors found.")]
        [Produces("application/json")]
        [Consumes("application/json")]

        public IActionResult getAuthors()
        {
            List<Author> Authors = unit.Generic_AuthorRepo.selectAll();
            if (!Authors.Any()) { return NotFound(); }
            List<SelectAuthorDTO> authorsDTO = new List<SelectAuthorDTO>();
            foreach (var author in Authors)
            {

                SelectAuthorDTO authorDTO = new SelectAuthorDTO()
                {
                    AuthorFullName = author.FullName,
                    Authors_NumberOfBooks = author.NumberOfBooks,
                    AuthorBIO = author.Bio,
                    Author_Age = author.age,
                    Id = author.Id

                };
                authorsDTO.Add(authorDTO);


            }
            return Ok(authorsDTO);

        }
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get author by ID", Description = "This endpoint retrieves a specific author by their ID. Requires authentication for access.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Author data retrieved successfully.", typeof(SelectAuthorDTO))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Author not found.")]
        [Produces("application/json")]
        public IActionResult getAuthorById(int id)
        {
            var author = unit.Generic_AuthorRepo.selectById(id);
            if (author == null) { return NotFound(); }
            SelectAuthorDTO authorDTO = new SelectAuthorDTO()
            {
                AuthorFullName = author.FullName,
                Authors_NumberOfBooks = author.NumberOfBooks,
                AuthorBIO = author.Bio,
                Author_Age = author.age,
                Id = author.Id
            };
            return Ok(authorDTO);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "Create a new author", Description = "This endpoint allows an admin to create a new author. Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Author created successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid author data.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [Produces("application/json")]
        [Consumes("application/json")]

        public IActionResult createAuthor(AddAuthorDTO authorDTO)
        {

            if (ModelState.IsValid)
            {
                Author auhtor = new Author()
                {
                    FullName = authorDTO.AuthorFullName,
                    age = authorDTO.Author_Age,
                    Bio = authorDTO.AuthorBIO,
                    NumberOfBooks = authorDTO.Authors_NumberOfBooks
                };
                unit.Generic_AuthorRepo.add(auhtor);
                unit.Generic_AuthorRepo.save();
                return Ok();
            }
            else { return BadRequest(ModelState); }

        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "Edit author profile", Description = "This endpoint allows an admin to update an author's profile information (name, bio, age, number of books). Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Author profile updated successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid profile data.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public IActionResult editAuthorProfile(EditAuthorDTO authorDTO)
        {
            if (ModelState.IsValid)
            {


                var author = unit.Generic_AuthorRepo.selectById(authorDTO.Id);

                if (author == null)
                {
                    return NotFound();
                }


                //author.FullName       = authorDTO.AuthorFullName;
                //author.Bio          = authorDTO.AuthorBIO;
                //author.NumberOfBooks    = authorDTO.Authors_NumberOfBooks;
                //author.age       = authorDTO.Author_Age;

                unit.Generic_AuthorRepo.update(author);
                unit.Generic_AuthorRepo.save();


                return Ok();

            }
            else { return BadRequest(ModelState); }

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "Delete an author by ID", Description = "This endpoint allows an admin to delete an author's profile by their ID. Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Author deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Author not found.")]
        [Produces("application/json")]
     
        public IActionResult deleteAuthor(int id) {

            var author = unit.Generic_AuthorRepo.selectById(id);
            if (author == null) { return NotFound(); }
            unit.Generic_AuthorRepo.remove(id);
            return Ok();
        
        }
    
    }
}
