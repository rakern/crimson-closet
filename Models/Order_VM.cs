namespace crimson_closet.Models
{
	public class Order_VM
	{
		public CustOrder Order { get; set; }
		public List<CustOrderItem> CustOrderItemList { get; set; }
	}
}
