using crimson_closet.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace crimson_closet.Models
{
    public class Cart
    {
        public Guid Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }

    }
}
