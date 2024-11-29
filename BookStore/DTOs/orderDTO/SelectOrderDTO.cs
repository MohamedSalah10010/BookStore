using System.Text.Json.Serialization;

namespace BookStore.DTOs.orderDTO
{
    public class SelectOrderDTO:EditOrderDTO
    {
        public decimal totalPrice { get; set; }
        [JsonIgnore]
        public override bool flagAddOrOverwrite { get; set; } // false : add , true; overwrite


    }
}
