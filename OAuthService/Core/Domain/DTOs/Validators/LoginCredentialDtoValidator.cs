using FluentValidation;

namespace OAuthService.Core.Domain.DTOs.Validators
{
    public class LoginCredentialDtoValidator: AbstractValidator<LoginCredentialDto>
    {
        public LoginCredentialDtoValidator()
        {
            string txt = " field is required";
            RuleFor(cr => cr.ClientId).NotNull().WithMessage($"clientId{txt}");
            RuleFor(cr => cr.GrantType).NotNull().WithMessage($"grantType{txt}");
            RuleFor(cr => cr.Email).NotNull().When(cr => cr.GrantType == "idtoken").WithMessage($"email{txt}");
            RuleFor(cr => cr.Password).NotNull().When(cr => cr.GrantType == "idtoken").WithMessage($"password{txt}");
            RuleFor(cr => cr.RefreshToken).NotNull().When(cr => cr.GrantType == "refreshToken").WithMessage($"refreshtoken{txt}");
        }
    }
}
