using ApplicationCommon.DTOs.Comment;
using FluentValidation;

namespace group_project_thread.Validators;

public class CommentUpdateValidator : AbstractValidator<CommentUpdateDTO>
{
    public CommentUpdateValidator()
    {
        RuleFor(p => p.TextContent).Cascade(CascadeMode.Stop).NotEmpty().When(p => p.Images.Count == 0)
            .WithMessage("You can not update post with no content")
            .MaximumLength(200).WithMessage("More than characters max amount");
        
        RuleFor(p => p.Images).Cascade(CascadeMode.Stop).NotEmpty()
            .When(p => p.TextContent == String.Empty).WithMessage("You can not update comment with no content");

        RuleFor(p => p.Images.Count).InclusiveBetween(0, 10)
            .WithMessage("You can not add more than 10 pictures to your comment");
    }
}