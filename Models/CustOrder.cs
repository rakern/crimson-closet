namespace crimson_closet.Models
{
    public class CustOrder
    {
        public int Id { get; set; }
        public int QuantOfItems { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime ReturnByDate { get; set; }
    }
}
