using System;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

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
            Variables.KeyFromFile        = true;
            Variables.FileWatcherEnabled = true;
            Variables.SourceFolder       = "C:\\";
            Variables.DestinationFolder  = "C:\\";
            Variables.FileSystem         = new FileSystemWatcher();
            Variables.Algorithm          = 0; //Initially SimpleSubstitutionCypher
        }

        #endregion

        #region FileSystemWatcher
        [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
        public static void StartFileSystemWatcher()
        {
            // Where to look
            Variables.FileSystem.Path = Variables.SourceFolder;

            // What do we want to watch
            Variables.FileSystem.NotifyFilter = NotifyFilters.FileName | 
                NotifyFilters.LastAccess | 
                NotifyFilters.LastWrite |
                NotifyFilters.DirectoryName;

            // look ONLY for .txt files
            Variables.FileSystem.Filter = "*.txt";

            // Give the permission to track and watch the system (fancy way of saying: BEGIN WATCHING)
            Variables.FileSystem.EnableRaisingEvents = true;

            // Handle the event of creation
            Variables.FileSystem.Created += new FileSystemEventHandler(onCreated);
        }

        // Stop FileSystemWatcher and dispose of it
        public static void StopFileSystemWatcher()
        {
            Variables.FileSystem.EnableRaisingEvents = false;
            Variables.FileSystem.Created -= new FileSystemEventHandler(onCreated);
            Variables.FileSystem.Dispose();
        }

        #region ON_CREATED_EVENT_HANDLER
        public static void onCreated(object sender, FileSystemEventArgs args)
        {
            // TODO: Call encrypt function from strategy or context
            MessageBox.Show(sender.ToString());
        }
        #endregion

        #endregion

        #region FileOpenMethods
        public static byte[] OpenFile(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                byte[] key = new byte[26];
                string temp;
                UTF8Encoding encoding = new UTF8Encoding(true);
                while(fs.Read(key, 0, key.Length) > 0)
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
