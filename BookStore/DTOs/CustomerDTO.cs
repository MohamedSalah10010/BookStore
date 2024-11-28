using Microsoft.AspNetCore.Identity;

namespace BookStore.DTOs
{
    public class CustomerDTO:IdentityUser
    {

        public string FullName { get; set; }
        public string Address { get; set; }


        public virtual List<OrderDTO> Orders { get; set; }=new List<OrderDTO>();
    
    }
}
