using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.DTOs;
using FluentValidation;

namespace EMarketAPI.Application.Validators.Order
{
    public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Ürün ID geçerli olmalıdır.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Adet en az 1 olmalıdır.");
        }
    }
}
