using System;
using System.Text.RegularExpressions;

namespace Gemstone.StringExtensions.StringMatcher;

/// <summary>
/// Represents a string matcher that can perform various types of string matching operations.
/// </summary>
public class StringMatcher
{
    /// <summary>
    /// Gets the <see cref="StringMatchingMode"/> used.
    /// </summary>
    public StringMatchingMode MatchMode { get; }

    /// <summary>
    /// Gets the string that is being matched.
    /// </summary>
    public string MatchText { get; }

    /// <summary>
    /// Gets a flag indicating whether the match is case sensitive.
    /// </summary>
    public bool CaseSensitive { get; }

    private readonly Regex? m_matchRegex;

    /// <summary>
    /// Generates a new <see cref="StringMatcher"/> instance.
    /// </summary>
    /// <param name="mode"> Sets the <see cref="StringMatchingMode"/> used to match.</param>
    /// <param name="value"> The string being matched. </param>
    /// <param name="caseSensitive">A flag indicating if the match is case sensitive. </param>
    public StringMatcher(StringMatchingMode mode, string value, bool caseSensitive = false)
    {
        MatchMode = mode;
        MatchText = value;
        CaseSensitive = caseSensitive;

        RegexOptions options = (caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase) | RegexOptions.Compiled;

        m_matchRegex = MatchMode switch
        {
            StringMatchingMode.Regex => new Regex(MatchText, options),
            StringMatchingMode.Contains => new Regex(Regex.Escape(MatchText), options),
            _ => null
        };
    }

    /// <summary>
    /// Compares the specified <paramref name="value"/> against the match criteria.
    /// </summary>
    /// <param name="value">The string that is being matched.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public bool IsMatch(string value)
    {
        StringComparison comparison = CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        return MatchMode switch
        {
            StringMatchingMode.Exact => value.Equals(MatchText, comparison),
            StringMatchingMode.StartsWith => value.StartsWith(MatchText, comparison),
            StringMatchingMode.EndsWith => value.EndsWith(MatchText, comparison),
            StringMatchingMode.Contains => m_matchRegex?.IsMatch(value) ?? false,
            StringMatchingMode.Regex => m_matchRegex?.IsMatch(value) ?? false,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
