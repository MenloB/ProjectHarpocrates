using System.IO;
using System.Security.Permissions;

namespace Harpokrat.Constants
{
    public static class Methods
    {

        // By default variables are:
        // *FileSystemWatcher = false
        // *SourceFolder      = C:\
        // *DestinationFolder = C:\
        //
        public static void InitializeVariables()
        {
            Variables.FileWatcherEnabled = true;
            Variables.SourceFolder       = "C:\\";
            Variables.DestinationFolder  = "C:\\";
        }

        [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
        public static void StartFileSystemWatcher()
        {
            FileSystemWatcher fileSystem = new FileSystemWatcher();
        }
    }
}
