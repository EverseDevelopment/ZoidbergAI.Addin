using System.IO;

namespace Common.Utils
{
    public class DirectoryUtilities
    {
        /// <summary>
        /// Create Directory if it does not exists
        /// </summary>
        public static bool CreateDirectoryIfNotExists(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Delete Directory if it does exists
        /// </summary>
        public static void DeleteDirectoryIfExists(string folder)
        {
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
        }
    }
}
