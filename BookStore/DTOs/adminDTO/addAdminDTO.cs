using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.adminDTO
{
    public class addAdminDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$")]

        public string Password { get; set; }
        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]

        public string Email { get; set; }
        [Required]

        public string Phonenumber  { get; set; }
    }
}
