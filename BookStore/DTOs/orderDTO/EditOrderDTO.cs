using BookStore.Models;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.orderDTO
{
    public class EditOrderDTO
    {
        [Required]
        public int OrderId { get; set; }
        //public DateOnly orderModificationDate { get; set; }

        public virtual bool flagAddOrOverwrite { get; set; } = false; // false : add , true; overwrite
        public List<EditOrderDetailsDTO> OrderDetails { get; set; }

        public string CustomerId { get; set; }
        public string OrderStatus { get; set; }
    }
}
