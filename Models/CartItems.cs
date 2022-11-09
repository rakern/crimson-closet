using System.ComponentModel.DataAnnotations.Schema;

namespace crimson_closet.Models
{
    public class CartItems
    {
        public Guid Id { get; set; }
        public Cart Cart { get; set; }
        public Item Item { get; set; }
    }
}
