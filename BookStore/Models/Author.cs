namespace BookStore.Models
{
    public class Author
    {

        public int Id { get; set; } 
        public string? FullName { get; set; }    
        public string Bio { get; set; } 
        public int NumberOfBooks { get; set; }  

        public int age { get; set; }

        public virtual List<Book> Books { get;set; }

       
    }
}
