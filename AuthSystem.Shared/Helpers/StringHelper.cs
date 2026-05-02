namespace AuthSystem.Shared.Helpers;

public static class StringHelper
{
    public static bool HasText(string? input) => !string.IsNullOrWhiteSpace(input);
}
