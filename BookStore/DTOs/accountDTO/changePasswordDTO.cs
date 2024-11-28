using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.accountDTO
{
    public class changePasswordDTO
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "password not match")]
        public string ConfirmPassword { get; set; }

    }
}
