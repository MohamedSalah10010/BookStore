using BookStore.Models;
using BookStore.UnitOfWork;
using BookStore.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.DTOs.bookDTO;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize (Roles = "admin")]
        public IActionResult delete(int id)
        {
            _unit.Generic_BookRepo.remove(id);
            _unit.Save();
            return Ok();

        }
    }

}
    

