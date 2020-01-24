using FluentValidation;

namespace OAuthService.Core.Domain.DTOs.Validators
{
    public class CredentialDtoValidator : AbstractValidator<CredentialDto>
    {
        public CredentialDtoValidator()
        {
            string txt = " field is required";
            RuleFor(cr => cr.ClientId).NotNull().WithMessage($"clientId{txt}");
            RuleFor(cr => cr.GrantType).NotNull().WithMessage($"grantType{txt}");
            RuleFor(cr => cr.Email).NotNull()
                .When(cr => cr.GrantType == "tokenid").WithMessage($"email{txt}").EmailAddress();
            RuleFor(cr => cr.Password).NotNull()
                .When(cr => cr.GrantType == "tokenid").WithMessage($"password{txt}").MinimumLength(8);
            RuleFor(cr => cr.RefreshToken).NotNull()
                .When(cr => cr.GrantType == "refreshtoken").WithMessage($"refreshtoken{txt}");
        }
    }
}
