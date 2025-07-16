
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarketAPI.Application.DTOs
{
    public class CreateOrderDto
    {
        public string UserId { get; set; } = null!;
        public List<CreateOrderItemDto> Items { get; set; } = new(); 
    }
}
