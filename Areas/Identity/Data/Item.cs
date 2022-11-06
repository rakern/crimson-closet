using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace crimson_closet.Areas.Identity.Data
{
    public enum ItemStatus
    {
        InCloset,
        Borrowed,
        AtCleaners
    }
    public class Item
    {
        public Guid ItemId { get; set; }
        public string ItemCode { get; set; }
        public ItemStatus? ItemStatus { get; set; }
        public string? ItemBrand { get; set; }
        public string? ItemSize { get; set; }
        public string? ItemColor { get; set; }
        public byte[]? ItemPhoto { get; set; }
        
        // foreign key property
        public Guid? ItemTypeID { get; set; }

        // navigational property for foreign key
        [ForeignKey("ItemTypeID")]
        public virtual ItemType? ItemType { get; set; }
    }
}
