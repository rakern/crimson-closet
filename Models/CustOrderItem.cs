using System.ComponentModel.DataAnnotations.Schema;

namespace crimson_closet.Models
{
    public class CustOrderItem
    {
        public Guid Id { get; set; }

        [ForeignKey("CustOrder")]
        public Guid CustOrderId { get; set; }
        public CustOrder CustOrder { get; set; }

        [ForeignKey("Item")]
        public Guid ItemId { get; set; }
        public Item Item { get; set; }

    }
}
