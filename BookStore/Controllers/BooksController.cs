using BookStore.DTOs;
using BookStore.Models;
using BookStore.UnitOfWork;
using BookStore.Repos; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        UnitWork _unit;
        public BooksController(UnitWork _unit)
        { 
            this._unit = _unit;
        }

        [HttpGet]
        public IActionResult getall() {
            List<Book> books = _unit.Generic_BookRepo.selectAll();
            List<BookDTO> booksDTO = new List<BookDTO>();
            foreach (Book b in books)
            {
                BookDTO bookDTO = new BookDTO()
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
        public IActionResult getbyid(int id)
        {
            Book b = _unit.Generic_BookRepo.selectById(id);
            if (b != null)
            {
                BookDTO bookDTO = new BookDTO()
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
        public IActionResult add(AddBookDTO bookdto)
        {
            if (ModelState.IsValid)
            {
                Book b = new Book()
                {
                    Title=bookdto.BookTitle,
                    stock=bookdto.QuantityInStock,
                    
                    publishDate = bookdto.PublishDate,
                    Price = bookdto.Price,
                    auth_id = bookdto.BookAuthorId,
                    cat_id = bookdto.BookCatalogId

                };

                _unit.Generic_BookRepo.add(b);
                _unit.Save();
                return CreatedAtAction("getbyid", new { id = b.Id }, null);

            }
            return BadRequest(ModelState);

        }
        [HttpPut("{id}")]
        public IActionResult edit(int id, AddBookDTO bookdto)
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
        [HttpDelete]
        public IActionResult delete(int id)
        {


            _unit.Generic_BookRepo.remove(id);
            _unit.Save();
            return Ok();

        }
    }

}
    

