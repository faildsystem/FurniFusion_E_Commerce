namespace FurniFusion.Dtos.Order
{
    public class UpdateOrdersStatusDto
    {

        public List<int> UpdatedOrders { get; set; }
        public List<int> FailedOrders { get; set; }
    }
}
