using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Models.Validate
{
    public class ProductValidate : AbstractValidator<Product>
    {
        public ProductValidate()
        {
            RuleFor(x => x.Title)
               .NotEmpty().WithMessage("Campo Title deve ser informado.")
               .MinimumLength(3).WithMessage("Campo Title deve ter no minimo 3 caracteres.")
               .MaximumLength(120).WithMessage("Campo Title deve ter no maximo 120 caracteres.");

            RuleFor(x => x.Price)
                .NotNull().WithMessage("Campo Price deve ser informado.")
                .GreaterThan(0).WithMessage("Price deve ser maior que 0");

            RuleFor(x => x.Quantity)
                .NotNull().WithMessage("Campo Quantity deve ser informado.")
                .GreaterThan(0).WithMessage("Quantity deve ser maior que 0");

            RuleFor(x => x.Description)
                .MaximumLength(1024).WithMessage("Campo Description deve ter no maximo 1024 caracteres.");

            RuleFor(x => x.CreateDate)
               .NotEmpty().WithMessage("Campo CreateDate deve ser informado.");

            RuleFor(x => x.LastUpdateDate)
               .NotEmpty().WithMessage("Campo LastUpdateDate deve ser informado.");

            RuleFor(x => x.CategoryId)
               .NotEmpty().WithMessage("Campo CategoryId deve ser informado.");
        }
    }
}
