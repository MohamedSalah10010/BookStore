namespace BookStore.DTOs
{
    public class AddBookDTO
    {
     
        public string? BookTitle { get; set; }
        public string? BookDescription { get; set; }
        public int BookAuthorId { get; set; }
        public int QuantityInStock { get; set; }
        public decimal Price { get; set; }
        public int BookCatalogId { get; set; }
        public DateOnly PublishDate { get; set; }
    }
}
