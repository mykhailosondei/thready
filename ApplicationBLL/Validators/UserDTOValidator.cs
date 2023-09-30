using ApplicationCommon.DTOs.User;
using FluentValidation;

namespace group_project_thread.Validators;

public class UserDTOValidator: AbstractValidator<UserDTO>
{
    public UserDTOValidator()
    {
        RuleFor(u => u.Username).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Username can not be empty")
            .MaximumLength(15).WithMessage("Length of your username is invalid");
        
        RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("You can not have an empty email")
            .Length(4, 200).WithMessage("Length of your Email is invalid")
            .Must(BeValidEmail).WithMessage("Please enter a valid email");
        
        RuleFor(u => u.Password).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password can not be empty")
            .Length(8, 200).WithMessage("Length of your password is invalid");

        RuleFor(u => u.Bio).Cascade(CascadeMode.Stop)
            .MaximumLength(200).WithMessage("Max length of bio exceeded");
        
        RuleFor(u => u.Location).Cascade(CascadeMode.Stop)
            .MaximumLength(30).WithMessage("Max length of location exceeded");
    }
    
    protected bool BeValidEmail(string email)
    {
        var atSymbolPosition = email.LastIndexOf('@');
        return !(atSymbolPosition < 0 || email.LastIndexOf('.') < atSymbolPosition || email.Length - atSymbolPosition < 4);   
    }
}