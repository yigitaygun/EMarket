using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.DTOs.User;
using FluentValidation;

namespace EMarketAPI.Application.Validators.User
{
    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {

        public LoginDtoValidator()
        {
            RuleFor(x=>x.Email)
                .NotEmpty().WithMessage("Email boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");


            RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Şifre boş olamaz.")
           .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
        }
    }
}
