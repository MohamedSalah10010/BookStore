
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Catalog
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50) ]
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public virtual List<Book> Books { get; set; }=new List<Book>();
    }
}
