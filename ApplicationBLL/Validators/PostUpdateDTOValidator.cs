using ApplicationCommon.DTOs.Post;
using FluentValidation;

namespace group_project_thread.Validators;

public class PostUpdateDTOValidator : AbstractValidator<PostUpdateDTO>
{
    public PostUpdateDTOValidator()
    {
        RuleFor(p => p.TextContent).Cascade(CascadeMode.Stop).NotEmpty().When(p => p.Images.Count == 0)
            .WithMessage("You can not update post with no content")
            .MaximumLength(500).WithMessage("More than characters max amount");
        
        RuleFor(p => p.Images).Cascade(CascadeMode.Stop).NotEmpty()
            .When(p => p.TextContent == String.Empty).WithMessage("You can not update post with no content");

        RuleFor(p => p.Images.Count).InclusiveBetween(0, 4)
            .WithMessage("You can not add more than 4 pictures to your post");
    }
}