using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int stock { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        [Column(TypeName="date")]
        public DateOnly publishDate { get; set; }
        [ForeignKey("catalog")]
        public int? cat_id { get; set; }
        public virtual Catalog? catalog { get; set; }

        [ForeignKey("author")]
        public int? auth_id { get; set; }
        public virtual Author? author { get; set; }

        public virtual List<OrderDetails> orderDetails { get; set; } = new List<OrderDetails>();

    }
}
