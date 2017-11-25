using System.IO;
using System.Security.Permissions;
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
            Variables.FileWatcherEnabled = true;
            Variables.SourceFolder       = "C:\\";
            Variables.DestinationFolder  = "C:\\";
            Variables.FileSystem = new FileSystemWatcher();
        }

        #endregion

        #region FileSystemWatcher
        [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
        public static void StartFileSystemWatcher()
        {
            // Where to look
            Variables.FileSystem.Path = Variables.SourceFolder;

            // What do we want to watch
            Variables.FileSystem.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite |
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
            MessageBox.Show(args.Name + " created", "File Created.", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        #endregion
        
        #endregion

    }
}
