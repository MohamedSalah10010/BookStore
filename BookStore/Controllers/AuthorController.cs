using BookStore.DTOs.authorDTO;
using BookStore.DTOs.customerDTO;
using BookStore.Models;
using BookStore.Repos;
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
    public class AuthorController : ControllerBase
    {
        UserManager<IdentityUser> userManager;
        RoleManager<IdentityRole> roleManager;
        UnitWork unit;

        public AuthorController(UnitWork unit,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.unit = unit;

        }


        [HttpGet]

        public IActionResult getAuthors()
        {
            List<Author> Authors = unit.Generic_AuthorRepo.selectAll();
                if (!Authors.Any()) { return NotFound(); }
            List<SelectAuthorDTO> authorsDTO = new List<SelectAuthorDTO>();
            foreach (var author in Authors)
            {

                SelectAuthorDTO authorDTO = new SelectAuthorDTO()
                {
                    AuthorFullName=author.FullName,
                    Authors_NumberOfBooks=author.NumberOfBooks,
                    AuthorBIO=author.Bio,
                    Author_Age=author.age,
                    Id=author.Id

                };
                authorsDTO.Add(authorDTO);


            }
            return Ok(authorsDTO);

        }

        [HttpGet("{id}")]

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

        [HttpDelete]
        [Authorize(Roles ="admin")]
        public IActionResult deleteAuthor(int id) { 
        
            unit.Generic_AuthorRepo.remove(id);
            return Ok();
        
        }
    
    }
}
