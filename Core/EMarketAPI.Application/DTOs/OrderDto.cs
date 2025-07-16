using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarketAPI.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public decimal TotalPrice { get; set; }
    }
}
