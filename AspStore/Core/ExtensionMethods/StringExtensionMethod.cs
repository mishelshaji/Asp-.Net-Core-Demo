namespace AspStore.Core.ExtensionMethods
{
    public static class StringExtensionMethod
    {
        public static string GetAsFileUploadPath(this string data)
        {
            return data.StartsWith("/") ? data : $"/{data}";
        }
    }
}
