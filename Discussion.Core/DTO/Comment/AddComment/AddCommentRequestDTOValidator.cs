namespace Discussion.Core.DTO.Comment.AddComment;

using FluentValidation;

public class AddCommentRequestDTOValidator : AbstractValidator<AddCommentRequestDTO>
{
    public AddCommentRequestDTOValidator()
    {
        RuleFor(r => r.UserName)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MaximumLength(100)
            .WithMessage("Username cannot exceed 100 characters.");
        
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Please provide a valid email address.");

        RuleFor(r => r.Text)
            .MaximumLength(2000)
            .WithMessage("Comment cannot exceed 2000 characters.");
    }
}