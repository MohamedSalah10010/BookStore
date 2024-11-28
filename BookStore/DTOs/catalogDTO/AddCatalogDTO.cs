
using System.ComponentModel.DataAnnotations;
using BookStore.DTOs.bookDTO;

namespace BookStore.DTOs.catalogDTO
{
    public class AddCatalogDTO
    {

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

    }
}
