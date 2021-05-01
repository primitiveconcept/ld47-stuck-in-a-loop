namespace Spineless.Extensions.Strings
{
    using System.Globalization;


    public static class StringExtensions
    {
        public static string LowerFirstChar(this string value)
        {
            char[] charArray = value.ToCharArray();
            charArray[0] = char.ToLower(charArray[0]);

            return new string(charArray);
        }


        /// <summary>
        /// Convert a string to camelCase.
        /// </summary>
        /// <param name="value">String to convert.</param>
        /// <returns>String converted to camelCase.</returns>
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            value = value.ToPascalCase();

            return value.LowerFirstChar();
        }


        /// <summary>
        /// Convert a string to kebab-case.
        /// </summary>
        /// <param name="value">String to convert.</param>
        /// <returns>String converted to kebab-case.</returns>
        public static string ToKebabCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value
                .ToLower()
                .Replace("_", "-")
                .Replace(" ", "-");
        }


        /// <summary>
        /// Convert a string to PascalCase.
        /// </summary>
        /// <param name="value">String to convert.</param>
        /// <returns>String converted to PascalCase.</returns>
        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            value = value
                .Replace("_", " ")
                .Replace("-", " ")
                .Trim();

            if (value.Contains(" "))
            {
                value = info.ToTitleCase(value.ToLower())
                    .Replace(" ", string.Empty);
            }
            else
            {
                value = value.UpperFirstChar();
            }

            return value;
        }


        public static string UpperFirstChar(this string value)
        {
            char[] charArray = value.ToCharArray();
            charArray[0] = char.ToUpper(charArray[0]);

            return new string(charArray);
        }
    }
}