using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DTOs.orderDTO

{
    public class AddOrderDetailsDTO
    {

 

        public int Book_Id { get; set; }
        public int Quantity { get; set; }

        //public decimal UnitPrice { get; set; }




    }
}
