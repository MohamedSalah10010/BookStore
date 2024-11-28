using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.DTOs.customerDTO;

namespace BookStore.DTOs.orderDTO
{
    public class AddOrderDTO
    {
        public string customer_id { get; set; }
        public List<AddOrderDetailsDTO> orderDetails { get; set; } = new List<AddOrderDetailsDTO>();
    }
}
