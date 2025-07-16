using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarketAPI.Application.DTOs
{
    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }  //ürünün idsi
        public int Quantity { get; set; } //adet 

    }
}
