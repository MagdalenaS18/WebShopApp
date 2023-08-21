using System.ComponentModel.DataAnnotations;

namespace OrderApp.DTOs
{
    public class OrderDto
    {
        public string Comment { get; set; }
        public string Address { get; set; }
        public double TotalPrice { get; set; }
        public bool IsPayed { get; set; }
    }
}
