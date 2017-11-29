using System;
using System.IO;

namespace Harpokrat.Constants
{
    public class Variables
    {
        public static bool KeyFromFile             { get; set; }
        public static string SourceFolder          { get; set; }
        public static string DestinationFolder     { get; set; }
        public static bool FileWatcherEnabled      { get; set; }

        // Storing the reference to filesystem watcher instead of having multiple fileSystemwatcher 
        // (not a Singleton)
        public static FileSystemWatcher FileSystem { get; set; }

        public static byte[] EncryptionKey         { get; set; }
        //public static byte[] DecryptionKey         { get; set; }
    }
}
