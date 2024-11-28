using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookStore.DTOs.bookDTO
{
    public class DisplayBookDTO
    {

        public int Book_Id { get; set; }

        public string BookTitle { get; set; }
        public string BookDescription { get; set; }
        public int QuantityInStock { get; set; }
        [JsonIgnore]
        public decimal Price { get; set; }
        public DateOnly PublishDate { get; set; }
        public string? BookAuthor { get; set; }
        public string? BookCatalog { get; set; }


    }
}
