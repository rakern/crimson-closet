using crimson_closet.Areas.Identity.Data;

namespace crimson_closet.Models
{
    public class CustOrder
    {
        public Guid Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int QuantOfItems { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime ReturnByDate { get; set; }
    }
}
