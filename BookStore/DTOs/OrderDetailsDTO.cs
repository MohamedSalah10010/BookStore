using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DTOs

{
    public class OrderDetailsDTO
    {
        
        public int Order_Id { get; set; }
        
        public int Book_Id { get; set; }
        public string Book_Title { get; set; }
        

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        



    }
}
