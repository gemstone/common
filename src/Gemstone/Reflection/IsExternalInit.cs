#if !NET

// This allows { get; init; } to be used in .NET Standard

// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices;

internal class IsExternalInit
{
}

#endif
