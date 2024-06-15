using System.Text.RegularExpressions;
using Marketplace.Framework;

namespace Marketplace.Domain.Value_objects;

public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
{
    public static ClassifiedAdTitle FromString(string title)
    {
        CheckValidity(title);
        return new ClassifiedAdTitle(title);
    }

    public static ClassifiedAdTitle FromHtml(string htmlTitle)
    {
        var supportedTagsReplaced = htmlTitle
            .Replace(oldValue: "<i>", newValue: "*")
            .Replace(oldValue: "</i>", newValue: "*")
            .Replace(oldValue: "<b>", newValue: "**")
            .Replace(oldValue: "</b>", newValue:" **");
        
        string value =  Regex.Replace(
            input: supportedTagsReplaced, pattern: "<.*?>", replacement: string.Empty);
        
        CheckValidity(value: value);

        return new ClassifiedAdTitle(value);
    }

    private string Value { get; }

    internal ClassifiedAdTitle(string value) => Value = value;

    private static void CheckValidity(string value)
    {
        if (value.Length > 100)
            throw new ArgumentOutOfRangeException(paramName: nameof(value), message: "Title cannot be longer than 100 characters");
    }

    public static implicit operator string(ClassifiedAdTitle title) => title.Value;
}