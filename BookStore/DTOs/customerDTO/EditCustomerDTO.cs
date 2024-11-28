using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.customerDTO
{
    public class EditCustomerDTO
    {
        //[Required]
        //public string Id { get; set; } // cause i'm using authorize
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]

        public string Email { get; set; }
        public string Address { get; set; }
        [Required]
        [RegularExpression("^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s\\./0-9]*$")]

        public string PhoneNumber { get; set; }
    }
}
