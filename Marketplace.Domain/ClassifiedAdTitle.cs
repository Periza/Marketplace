using System;
using System.Text.RegularExpressions;
using Marketplace.Framework;

namespace Marketplace.Domain
{
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
                .Replace(oldValue: "</b>", newValue: "**");

            var value = Regex.Replace(input: supportedTagsReplaced, pattern: "<.*?>", replacement: string.Empty);
            CheckValidity(value);

            return new ClassifiedAdTitle(value);
        }

        public string Value { get; }

        internal ClassifiedAdTitle(string value) => Value = value;

        public static implicit operator string(ClassifiedAdTitle title) =>
            title.Value;

        private static void CheckValidity(string value)
        {
            if (value.Length > 100)
                throw new ArgumentOutOfRangeException(
                    paramName: "Title cannot be longer that 100 characters",
                    message: nameof(value));
        }
    }
}