﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DTOs
{
    public class OrderDTO
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName ="date")]
        
        public DateOnly orderDate { get; set; }
        [Column(TypeName ="money")]
        public decimal totalPrice { get; set; }  

        public string status { get; set; }

        [ForeignKey("customer")]
        public string customer_id { get; set; }
        public virtual CustomerDTO customer { get; set; }

        public virtual List<OrderDetailsDTO> orderDetails { get; set; }=new List<OrderDetailsDTO>();
    }
}