using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DTOs
{
    public class BookDTO
    {
        
        public int Book_Id { get; set; }
        
        public string BookTitle { get; set; }
        public string BookDescription { get; set; }
        public string? BookAuthor { get; set; }
        public int QuantityInStock { get; set; }
        public decimal Price { get; set; }
        public string? BookCatalog { get; set; }
        public DateOnly? PublishDate { get; set; }
        
        
    }
}
