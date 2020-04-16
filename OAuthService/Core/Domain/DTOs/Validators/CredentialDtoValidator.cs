using FluentValidation;
using static OAuthService.CONSTANTS;

namespace OAuthService.Core.Domain.DTOs.Validators
{
    public class CredentialDtoValidator : AbstractValidator<CredentialDto>
    {
        public CredentialDtoValidator()
        {
            string txt = " field is required";

            RuleFor(cr => cr.RequestType).NotNull().WithMessage($"RequestType{txt}").DependentRules(() =>
            {
                RuleFor(cr => cr.ClientId).NotNull()
                    .When(cr => cr.RequestType.ToLower() == REQUEST_TYPE.ID_TOKEN).WithMessage($"clientId{txt}");

                RuleFor(cr => cr.Email).NotNull()
                    .When(cr => cr.RequestType.ToLower() == REQUEST_TYPE.ID_TOKEN
                    || cr.RequestType.ToLower() == REQUEST_TYPE.REGISTER).WithMessage($"email{txt}").EmailAddress();

                RuleFor(cr => cr.Password)
                .NotNull()
                    .When(cr =>
                       cr.RequestType.ToLower() == REQUEST_TYPE.ID_TOKEN
                    || cr.RequestType.ToLower() == REQUEST_TYPE.REGISTER
                    || cr.RequestType.ToLower() == REQUEST_TYPE.CHANGE_PASSWORD).WithMessage($"password{txt}")
                .MinimumLength(8).WithMessage("password must have at least 8 caracters");

                RuleFor(cr => cr.NewPassword).NotNull()
                    .When(cr =>
                       cr.RequestType.ToLower() == REQUEST_TYPE.CHANGE_PASSWORD
                    || cr.RequestType.ToLower() == REQUEST_TYPE.FORGOT_PASSWORD).WithMessage($"password{txt}")
                    .MinimumLength(8).WithMessage("password must have at least 8 caracters");

                RuleFor(cr => cr.RefreshToken).NotNull()
                    .When(cr => cr.RequestType.ToLower() == REQUEST_TYPE.REFRESH_TOKEN).WithMessage($"refreshtoken{txt}");

                RuleFor(cr => cr.ClientSecret)
                .NotNull()
                    .When(cr =>
                       cr.RequestType.ToLower() == REQUEST_TYPE.ID_TOKEN
                    && cr.ClientId!.ToLower() == "2").WithMessage($"clientsecret{txt} when you call it from mobile");//TODO: change clientId for other app

                RuleFor(cr => cr.ResetPasswordToken).NotNull()
                    .When(cr => cr.RequestType.ToLower() == REQUEST_TYPE.FORGOT_PASSWORD).WithMessage($"resetpasswordtoken{txt}");
            });
        }
    }
}
