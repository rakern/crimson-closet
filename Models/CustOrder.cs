using crimson_closet.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace crimson_closet.Models
{
    public class CustOrder
    {
        public Guid Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime CheckOutDate { get; set; }

        public DateTime ReturnByDate { get; set; }

        public string Status { get; set; }

        public CustOrder() {
            Status = "Pending";
        }
    }
}
