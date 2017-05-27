namespace Xamarin.Core.Utils
{
    public static class Logger
    {
        public static void Debug(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public static void Debug(string tag, string message)
        {
            string log = string.Format("{0}: {1}", tag, message);
            System.Diagnostics.Debug.WriteLine(log);
        }
    }
}
