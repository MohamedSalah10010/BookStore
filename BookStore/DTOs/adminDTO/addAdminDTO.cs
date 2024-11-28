using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.adminDTO
{
    public class addAdminDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]

        public string Phonenumber
        { get; set; }
    }
}
