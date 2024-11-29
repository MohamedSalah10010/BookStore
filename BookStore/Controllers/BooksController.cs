using BookStore.Models;
using BookStore.UnitOfWork;
using BookStore.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.DTOs.bookDTO;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        UnitWork _unit;
        public BooksController(UnitWork _unit)
        {
            this._unit = _unit;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all books", Description = "This endpoint retrieves a list of all books. Requires authentication for access.")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of books retrieved successfully.", typeof(List<DisplayBookDTO>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [Produces("application/json")]
        public IActionResult getAllBooks() {
            List<Book> books = _unit.Generic_BookRepo.selectAll();
            List<DisplayBookDTO> booksDTO = new List<DisplayBookDTO>();
            foreach (Book b in books)
            {
                DisplayBookDTO bookDTO = new DisplayBookDTO()
                {
                    Book_Id = b.Id,
                    BookTitle = b.Title,
                    Price = b.Price,
                    PublishDate = b.publishDate,
                    QuantityInStock = b.stock,
                    BookCatalog = b.catalog.Name,
                    BookAuthor = b.author.FullName
                };
                booksDTO.Add(bookDTO);

            }

            return Ok(booksDTO);
        }
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get book by ID", Description = "This endpoint retrieves a specific book by its ID. Requires authentication for access.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Book data retrieved successfully.", typeof(DisplayBookDTO))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Book not found.")]
        [Produces("application/json")]
       
        public IActionResult getBookById(int id)
        {
            Book b = _unit.Generic_BookRepo.selectById(id);
            if (b != null)
            {
                DisplayBookDTO bookDTO = new DisplayBookDTO()
                {
                    Book_Id = b.Id,
                    BookTitle = b.Title,
                    Price = b.Price,
                    PublishDate = b.publishDate,
                    QuantityInStock = b.stock,
                    BookCatalog = b.catalog.Name,
                    BookAuthor = b.author.FullName
                };
                return Ok(bookDTO);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "Add a new book", Description = "This endpoint allows an admin to add a new book to the store. Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Book added successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid book data.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public IActionResult addBook(AddBookDTO bookdto)
        {
            if (ModelState.IsValid)
            {
                Book book = new Book()
                {
                    Title = bookdto.BookTitle,
                    stock = bookdto.QuantityInStock,

                    publishDate = bookdto.PublishDate,
                    Price = bookdto.Price,
                    auth_id = bookdto.BookAuthorId,
                    cat_id = bookdto.BookCatalogId

                };

                _unit.Generic_BookRepo.add(book);
                _unit.Save();
                return CreatedAtAction("getBookById", new { id = book.Id }, null);

            }
            return BadRequest(ModelState);

        }
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "Edit a book's details", Description = "This endpoint allows an admin to update a book's details. Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Book updated successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid book data.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public IActionResult editBook(int id, AddBookDTO bookdto)
        {

            if (ModelState.IsValid)
            {
                Book b = new Book()
                {
                    Title = bookdto.BookTitle,
                    stock = bookdto.QuantityInStock,

                    publishDate = bookdto.PublishDate,
                    Price = bookdto.Price,
                    auth_id = bookdto.BookAuthorId,
                    cat_id = bookdto.BookCatalogId

                };



                _unit.Generic_BookRepo.update(b);
                _unit.Save();
                return NoContent();



            }
            return BadRequest(ModelState);

        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [SwaggerOperation(Summary = "Delete a book by ID", Description = "This endpoint allows an admin to delete a book from the store by its ID. Only accessible by authenticated users with the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Book deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized - Requires authentication.")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden - User does not have the 'admin' role.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Book not found.")]
        [Produces("application/json")]
        public IActionResult delete(int id)
        {
            _unit.Generic_BookRepo.remove(id);
            _unit.Save();
            return Ok();

        }
    }

}
    

