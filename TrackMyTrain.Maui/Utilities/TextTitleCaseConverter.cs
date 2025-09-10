
using System.Globalization;
using System.Text;

namespace TrackMyTrain.Maui.Utilities
{
    public class TextTitleCaseConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return string.Empty;
            }
            if (value is string text)
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    // Create a StringBuilder object to store the result.
                    StringBuilder result = new();

                    // Get the TextInfo object for the current culture.
                    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                    // Flag to track if we are at the beginning of a new word.
                    bool newWord = true;

                    // Iterate over each character in the string.
                    for (int i = 0; i < text.Length; i++)
                    {
                        char currentChar = text[i];

                        // If the current character is a letter or digit.
                        if (char.IsLetterOrDigit(currentChar))
                        {
                            // If we are at the beginning of a new word, convert the character to uppercase.
                            if (newWord)
                            {
                                result.Append(textInfo.ToUpper(currentChar));
                                newWord = false;
                            }
                            // Otherwise, add the character as is for uppercase or convert to lowercase for other characters.
                            else
                            {
                                result.Append(i < text.Length - 1 && char.IsUpper(currentChar) && char.IsLower(text[i + 1]) ? currentChar : char.ToLowerInvariant(currentChar));
                            }
                        }
                        // If the current character is not a letter or digit, we are at the beginning of a new word.
                        else
                        {
                            newWord = true;
                            result.Append(" ");
                        }

                        // If the current character is a lowercase letter and the next character is an uppercase letter,
                        // we are at the beginning of a new word.
                        if (i < text.Length - 1 && char.IsLower(text[i]) && char.IsUpper(text[i + 1]))
                        {
                            newWord = true;
                        }
                    }

                    // Return the result as a string.
                    return result.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}