namespace OrderApp.DTO
{
    public class NewOrder
    {
        public long UserId { get; set; }
        public string Comment { get; set; }
        public string DeliveryAddress { get; set; }
        public List<long> ProductIds { get; set; }
    }
}
