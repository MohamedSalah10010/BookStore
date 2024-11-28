using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.adminDTO
{
    public class EditAdminDTO
    {

        [Required]
        public string Id { get; set; }
      
        [Required]
        public string UserName { get; set; }

        [Required]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]

        public string Email { get; set; }
     
        [Required]
        [RegularExpression("^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s\\./0-9]*$")]
        public string PhoneNumber { get; set; }
    }
}
