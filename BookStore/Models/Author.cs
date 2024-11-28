using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }    
        public string? Bio { get; set; } 
        public int NumberOfBooks { get; set; }  

        public int age { get; set; }

        public virtual List<Book> Books { get;set; }= new List<Book>();

       
    }
}
