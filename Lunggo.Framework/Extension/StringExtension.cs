namespace Lunggo.Framework.Extension
{
    public static class StringExtension
    {
        public static string ReplaceFirstOccurrence(this string source, string find, string replace)
        {
            var index = source.IndexOf(find, System.StringComparison.Ordinal);
            return index < 0 ? source : source.Substring(0, index) + replace +
                 source.Substring(index + find.Length);
        }
    }
}
