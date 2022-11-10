using crimson_closet.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace crimson_closet.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("ItemId")]
        public Guid ItemId { get; set; }
        public Item Item { get; set; }

    }
}
