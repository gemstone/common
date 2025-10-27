namespace Gemstone.StringExtensions.StringMatcher;

/// <summary>
/// Defines a set of string matching modes.
/// </summary>
public enum StringMatchingMode
{
    /// <summary>
    /// Exact string matching.
    /// </summary>
    Exact,
    /// <summary>
    /// String starts with matching.
    /// </summary>
    StartsWith,
    /// <summary>
    /// String ends with matching.
    /// </summary>
    EndsWith,
    /// <summary>
    /// String contains matching.
    /// </summary>
    Contains,
    /// <summary>
    /// Regular expression string matching.
    /// </summary>
    Regex
}
