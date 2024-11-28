using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DTOs.bookDTO
{
    public class AddBookDTO
    {
        [Required]
        public string BookTitle { get; set; }
        public string? BookDescription { get; set; }
        [Range(1, 500, ErrorMessage = "Invalid quantity, stock has only 500 places")]

        public int QuantityInStock { get; set; }
        [Range(20,1000, ErrorMessage="Invalid price must be in range of (20,1000)")]
        public decimal Price { get; set; }
        [Column(TypeName="date")]
        public DateOnly PublishDate { get; set; }
        public int? BookAuthorId { get; set; }
        public int? BookCatalogId { get; set; }


    }
}
