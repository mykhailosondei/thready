using ApplicationCommon.DTOs.Comment;
using FluentValidation;

namespace group_project_thread.Validators;

public class CommentDTOValidator : AbstractValidator<CommentDTO>
{
    public CommentDTOValidator()
    {
        RuleFor(c => c.TextContent ).NotEmpty().
            When(p => p.Images.Count == 0).WithMessage("You can not create comment with no content");
        
        RuleFor(p => p.TextContent).MaximumLength(200)
            .WithMessage("More than characters max amount");
        
        RuleFor(p => p.Images).NotEmpty()
            .When(p => p.TextContent == String.Empty).WithMessage("You can not create comment with no content");

        RuleFor(p => p.Images.Count).InclusiveBetween(0, 5)
            .WithMessage("You can not add more than 10 pictures to your comment");
    }
}