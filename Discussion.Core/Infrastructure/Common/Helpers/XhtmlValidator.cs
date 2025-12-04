namespace Discussion.Core.Infrastructure.Common.Helpers;

using HtmlAgilityPack;

public class XhtmlValidator
{
    public static bool IsValidXhtml(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        try
        {
            var doc = new HtmlDocument
            {
                OptionFixNestedTags = false
            };
            doc.LoadHtml(input);
            
            return !doc.ParseErrors.Any();
        }
        catch
        {
            return false;
        }
    }
}