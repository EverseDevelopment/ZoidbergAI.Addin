using System.IO;

namespace Common.Utils
{
    public static class InternetUtilities
    {
        const string UriToCheck = "https://www.google.com";

        /// <summary>
        /// Check if internet connection is available.
        /// </summary>
        public static bool Check()
        {
            try
            {
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    using (Stream stream = client.OpenRead(UriToCheck))
                    {
                        return true;
                    }
                }
            }
            catch (System.Net.WebException)
            {
                return false;
            }
        }
    }
}
