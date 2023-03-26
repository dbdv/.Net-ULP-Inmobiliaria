namespace testNetMVC.Helpers
{
    public static class Helper
    {
        public static void executeSafe(string label, Action func)
        {
            try
            {
                Console.WriteLine("Executing " + label);
                func();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing " + label + " " + ex);
            }
        }
    }
}