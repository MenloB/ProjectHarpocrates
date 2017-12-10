using System.IO;
using System.Text;

namespace Harpokrat.Constants
{
    public static class Methods
    {

        // By default variables are:
        // *FileSystemWatcher = false
        // *SourceFolder      = C:\\
        // *DestinationFolder = C:\\

        #region InitializeVariables
        public static void InitializeVariables()
        {
            Variables.KeyFromFile = true;
            Variables.FileWatcherEnabled = true;
            Variables.SourceFolder = "C:\\";
            Variables.DestinationFolder = "C:\\";
            Variables.FileSystem = new FileSystemWatcher();
            Variables.Algorithm = 0; //Initially SimpleSubstitutionCypher
        }

        #endregion

        #region FileOpenMethods
            public static byte[] OpenFile(string path)
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] key = new byte[26];
                    string temp;
                    UTF8Encoding encoding = new UTF8Encoding(true);
                    while (fs.Read(key, 0, key.Length) > 0)
                    {
                        temp = encoding.GetString(key);
                        return encoding.GetBytes(temp);
                    }
                }

                return null;
            }
            #endregion
        }
    }
