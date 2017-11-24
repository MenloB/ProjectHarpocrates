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
        }

        #endregion

        #region FileSystemWatcher
        [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
        public static void StartFileSystemWatcher()
        {
            FileSystemWatcher fileSystem = new FileSystemWatcher();

            // Where to look
            fileSystem.Path = Variables.SourceFolder;

            // What do we want to watch
            fileSystem.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite |
                NotifyFilters.DirectoryName;

            // look ONLY for .txt files
            fileSystem.Filter = "*.txt";

            // Give the permission to track and watch the system (fancy way of saying: BEGIN WATCHING)
            fileSystem.EnableRaisingEvents = true;

            // Handle the event of creation
            fileSystem.Created += new FileSystemEventHandler(onCreated);
        }

        #region ON_CREATED_EVENT_HANDLER
        public static void onCreated(object sender, FileSystemEventArgs args)
        {
            MessageBox.Show(args.Name + " created");
        }
        #endregion
        
        #endregion

    }
}
