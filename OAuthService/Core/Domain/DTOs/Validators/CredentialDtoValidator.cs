using FluentValidation;

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
                    .When(cr => cr.RequestType.ToLower() == "idtoken").WithMessage($"clientId{txt}");

                RuleFor(cr => cr.Email).NotNull()
                    .When(cr => cr.RequestType.ToLower() == "idtoken" 
                    || cr.RequestType.ToLower() == "register").WithMessage($"email{txt}").EmailAddress();

                RuleFor(cr => cr.Password)
                .NotNull()
                    .When(cr => 
                       cr.RequestType.ToLower() == "idtoken" 
                    || cr.RequestType.ToLower() == "register" 
                    || cr.RequestType.ToLower() == "changepassword").WithMessage($"password{txt}")
                .MinimumLength(8).WithMessage("password must have at least 8 caracters");

                RuleFor(cr => cr.NewPassword).NotNull()
                    .When(cr => 
                       cr.RequestType.ToLower() == "changepassword"
                    || cr.RequestType.ToLower() == "forgotpassword").WithMessage($"password{txt}")
                    .MinimumLength(8).WithMessage("password must have at least 8 caracters");

                RuleFor(cr => cr.RefreshToken).NotNull()
                    .When(cr => cr.RequestType.ToLower() == "refreshtoken").WithMessage($"refreshtoken{txt}");

                RuleFor(cr => cr.ClientSecret)
                .NotNull()
                    .When(cr => 
                       cr.RequestType.ToLower() == "idtoken" 
                    && cr.ClientId.ToLower() == "2").WithMessage($"clientsecret{txt} when you call it from mobile");//TODO: change clientId for other app

                RuleFor(cr => cr.ResetPasswordToken).NotNull()
                    .When(cr => cr.RequestType.ToLower() == "forgotpassword").WithMessage($"resetpasswordtoken{txt}");
            });
        }
    }
}
