using FluentValidation;

namespace OAuthService.Core.Domain.DTOs.Validators
{
    public class CredentialDtoValidator : AbstractValidator<CredentialDto>
    {
        public CredentialDtoValidator()
        {
            string txt = " field is required";

            RuleFor(cr => cr.GrantType).NotNull().WithMessage($"grantType{txt}");

            RuleFor(cr => cr.ClientId).NotEmpty()
                .When(cr => cr.GrantType.ToLower() == "idtoken").WithMessage($"clientId{txt}");

            RuleFor(cr => cr.Email).NotNull()
                .When(cr => cr.GrantType.ToLower() == "idtoken" || cr.GrantType.ToLower() == "register" || cr.GrantType.ToLower() == "forgotpassword").WithMessage($"email{txt}").EmailAddress();

            RuleFor(cr => cr.Password).NotNull()
                .When(cr => cr.GrantType.ToLower() == "idtoken" || cr.GrantType.ToLower() == "register").WithMessage($"password{txt}").MinimumLength(8).WithMessage("password must have at least 8 caracters");

            RuleFor(cr => cr.RefreshToken).NotNull()
                .When(cr => cr.GrantType.ToLower() == "refreshtoken").WithMessage($"refreshtoken{txt}");

            RuleFor(cr => cr.ClientSecret).NotEmpty()
                .When(cr => cr.GrantType.ToLower() == "idtoken" && cr.ClientId.ToLower() == "2").WithMessage($"clientsecret{txt} when you call it from mobile");

        }
    }
}
