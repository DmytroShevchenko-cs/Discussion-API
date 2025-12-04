namespace Discussion.Core.DTO.Comment.AddComment;

using FluentValidation;
using Infrastructure.Common.Helpers;

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
            .WithMessage("Comment cannot exceed 2000 characters.")
            .Must(XhtmlValidator.IsValidXhtml)
            .WithMessage("Comment contains invalid HTML/XHTML")
            .Must(ContainsOnlyAllowedHtml)
            .WithMessage("Comment contains invalid or not allowed HTML.");;
    }
    
    private static readonly HashSet<string> AllowedTags = new(StringComparer.OrdinalIgnoreCase)
    {
        "a", "code", "i", "strong"
    };

    private bool ContainsOnlyAllowedHtml(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return true;

        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(text);

        foreach (var node in doc.DocumentNode.Descendants())
        {
            if (node.NodeType != HtmlAgilityPack.HtmlNodeType.Element)
            {
                continue;
            }
            
            if (!AllowedTags.Contains(node.Name))
            {
                return false;
            }
            
            if (node.Name.Equals("a", StringComparison.OrdinalIgnoreCase))
            {
                var href = node.GetAttributeValue("href", null);
                if (string.IsNullOrWhiteSpace(href)) return false;

                var title = node.GetAttributeValue("title", null);
                if (string.IsNullOrWhiteSpace(title)) return false;
                
            }
            
            if (node.Attributes.Any(a =>
                    node.Name != "a" || a.Name != "href" && a.Name != "title"))
            {
                return false;
            }
        }

        return true;
    }
}