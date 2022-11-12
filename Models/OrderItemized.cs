using System.ComponentModel.DataAnnotations.Schema;

namespace crimson_closet.Models
{
    public class OrderItemized
    {
        public Guid Id { get; set; }
        [ForeignKey("ItemId")]
        public Guid ItemId { get; set; }
        public Item? Item { get; set; }
        [ForeignKey("CustOrderId")]
        public Guid? CustOrderId { get; set; }   
        public CustOrder CustOrder { get; set; }
    }
}
