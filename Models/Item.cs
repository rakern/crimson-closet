using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace crimson_closet.Models
{
    public enum ItemStatus
    {
        InCloset,
        Borrowed,
        AtCleaners
    }
    public enum ItemGender
    {
        [Display(Name = "Men's")] Mens,
        [Display(Name = "Women's")] Womens,
    }
    public class Item
    {
        public Guid ItemId { get; set; }
        public string ItemCode { get; set; }
        public ItemStatus? ItemStatus { get; set; }
        public string? ItemBrand { get; set; }
        public string? ItemSize { get; set; }
        public string? ItemColor { get; set; }
        public ItemGender? ItemGender { get; set; }
        public byte[]? ItemPhoto { get; set; }

        [ForeignKey("ItemType")]
        // foreign key property
        public Guid? ItemTypeID { get; set; }

        // navigational property for foreign key
        [ForeignKey("ItemTypeID")]
        public virtual ItemType? ItemType { get; set; }
    }

    
}


