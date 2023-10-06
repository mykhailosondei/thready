using ApplicationCommon.DTOs.User;
using FluentValidation;

namespace group_project_thread.Validators;

public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
{
    public UpdateUserDTOValidator()
    {
        {
            RuleFor(u => u.Bio).Cascade(CascadeMode.Stop)
                .MaximumLength(200).WithMessage("Max length of bio exceeded");
        
            RuleFor(u => u.Location).Cascade(CascadeMode.Stop)
                .MaximumLength(30).WithMessage("Max length of location exceeded");
        }
    }
}