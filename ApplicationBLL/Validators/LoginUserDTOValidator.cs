using ApplicationCommon.DTOs.User;
using FluentValidation;

namespace group_project_thread.Validators;

public class LoginUserDTOValidator: AbstractValidator<LoginUserDTO>
{
    public LoginUserDTOValidator()
    {
        RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("You can not have an empty email")
            .Length(4, 200).WithMessage("Length of your Email is invalid")
            .Must(BeValidEmail).WithMessage("Please enter a valid email");
        
        RuleFor(u => u.Password).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password can not be empty")
            .Length(8, 200).WithMessage("Length of your password is invalid");
    }
    
    protected bool BeValidEmail(string email)
    {
        var atSymbolPosition = email.LastIndexOf('@');
        return !(atSymbolPosition < 0 || email.LastIndexOf('.') < atSymbolPosition || email.Length - atSymbolPosition < 4);   
    }
}