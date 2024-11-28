using BookStore.DTOs.orderDTO;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.customerDTO
{
    public class AddCustomerDTO
    {

        public string fullname { get; set; }
        public string address { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$")]
        public string password { get; set; }
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
        public string email { get; set; }
        [Required]
        public string phonenumber { get; set; }


    }
}
