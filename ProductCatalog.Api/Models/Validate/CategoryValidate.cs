using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Api.Models.Validate
{
    public class CategoryValidate : AbstractValidator<Category>
    {
        public CategoryValidate()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Campo titulo deve ser informado.")
                .MinimumLength(3).WithMessage("Campo titulo deve ter no minimo 3 caracteres.")
                .MaximumLength(15).WithMessage("Campo titulo deve ter no maximo 15 caracteres.");
        }
    }
}
