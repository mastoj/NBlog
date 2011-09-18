namespace TJ.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull<T>(this T input) where T : class
        {
            return input == null;
        }

        public static bool IsNotNull<T>(this T input) where T : class
        {
            return input != null;
        }
    }
}
