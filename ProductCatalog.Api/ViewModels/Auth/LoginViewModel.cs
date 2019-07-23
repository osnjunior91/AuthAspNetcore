using FluentValidation;

namespace ProductCatalog.Api.ViewModels.Auth
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginValidate : AbstractValidator<LoginViewModel>
    {
        public LoginValidate()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Campo Username deve ser informado.")
                .MinimumLength(3).WithMessage("Campo Username deve ter no minimo 3 caracteres.")
                .MaximumLength(15).WithMessage("Campo Username deve ter no maximo 15 caracteres.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Campo Password deve ser informado.")
                .MinimumLength(6).WithMessage("Campo Password deve ter no minimo 6 caracteres.")
                .MaximumLength(12).WithMessage("Campo Password deve ter no maximo 12 caracteres.");
        }
    }
}
