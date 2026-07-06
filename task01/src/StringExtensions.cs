using System;
using System.Linq;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        string cleanString = new string(input.ToLower()
            .Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c))
            .ToArray());

        if (cleanString.Length == 0) return false;


        string reversedString = new string(cleanString.Reverse().ToArray());

        return cleanString == reversedString;
    }
}