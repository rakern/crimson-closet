using System.ComponentModel.DataAnnotations.Schema;

namespace crimson_closet.Models
{
    public class CartItems
    {
        public Guid Id { get; set; }

        [ForeignKey("Cart")]
        public Guid CartId { get; set; }
        public Cart Cart { get; set; }

        [ForeignKey("Item")]
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
    }
}
