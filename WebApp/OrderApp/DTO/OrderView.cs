namespace OrderApp.DTO
{
    public class OrderView
    {
        public long Id { get; set; }
        public int NumberOfProducts { get; set; }
        public double TotalPrice { get; set; }
        public string OrderedAt { get; set; }
        public string DeliveringTime { get; set; }
        public bool OrderCanceled { get; set; }
        public string? Comment { get; set; }
    }
}
