using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.customerDTO
{
    public class EditCustomerDTO
    {
        [Required]
        public string Id { get; set; }
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
