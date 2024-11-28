
using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs
{
    public class CatalogDTO
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50) ]
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<BookDTO> Books { get; set; }=new List<BookDTO>();
    }
}
