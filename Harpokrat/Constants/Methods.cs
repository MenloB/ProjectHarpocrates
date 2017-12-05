using System;
using System.Threading;
using System.Diagnostics;
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
            Variables.KeyFromFile = true;
            Variables.FileWatcherEnabled = true;
            Variables.SourceFolder = "C:\\";
            Variables.DestinationFolder = "C:\\";
            Variables.FileSystem = new FileSystemWatcher();
            Variables.Algorithm = 0; //Initially SimpleSubstitutionCypher
        }

        #endregion

        #region FileSystemWatcher
        //[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        //public static void StartFileSystemWatcher()
        //{
        //    // Where to look
        //    Variables.FileSystem.Path = Variables.SourceFolder;

        //    // What do we want to watch
        //    Variables.FileSystem.NotifyFilter = NotifyFilters.FileName |
        //        NotifyFilters.LastAccess |
        //        NotifyFilters.LastWrite |
        //        NotifyFilters.DirectoryName;

        //    // look ONLY for .txt files
        //    Variables.FileSystem.Filter = "*.txt";

        //    // Give the permission to track and watch the system (fancy way of saying: BEGIN WATCHING)
        //    Variables.FileSystem.EnableRaisingEvents = true;

        //    // Handle the event of creation
        //    Variables.FileSystem.Created += new FileSystemEventHandler(onCreated);
        //}

        // Stop FileSystemWatcher and dispose of it
        //public static void StopFileSystemWatcher()
        //{
        //    Variables.FileSystem.EnableRaisingEvents = false;
        //    Variables.FileSystem.Created -= new FileSystemEventHandler(onCreated);
        //    Variables.FileSystem.Dispose();
        //    MessageBox.Show(Thread.CurrentThread.Name);
        //}

        #region ON_CREATED_EVENT_HANDLER
        //public static void onCreated(object sender, FileSystemEventArgs args)
        //{
        //    // TODO: Call encrypt function from strategy or context
        //    EncryptionAlgorithms.Context context = new EncryptionAlgorithms.Context();
        //    context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy(Variables.EncryptionKey));

        //    MessageBox.Show(args.FullPath);

        //    if (Variables.EncryptionKey != null)
        //    {
        //        var fileDialog = new OpenFileDialog();
        //        int count = 1;

        //            string result = "";
        //            var sw = Stopwatch.StartNew();
        //            try
        //            {
        //                using (FileStream fs = File.OpenRead(fileDialog.FileName))
        //                {
        //                    byte[] text = new byte[16 * 1024];
        //                    UTF8Encoding encoding = new UTF8Encoding();
        //                    while (fs.Read(text, 0, text.Length) > 0)
        //                    {
        //                        switch (Variables.Algorithm)
        //                        {
        //                            case 0:
        //                                context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy(Variables.EncryptionKey));
        //                                break;
        //                            case 1:
        //                                context.SetEncryptionStrategy(new EncryptionAlgorithms.A51());
        //                                break;
        //                            case 2:
        //                                context.SetEncryptionStrategy(new EncryptionAlgorithms.XTEA());
        //                                break;
        //                            case 3:
        //                                context.SetEncryptionStrategy(new EncryptionAlgorithms.RSA());
        //                                break;
        //                        }
        //                        context.Message = System.Text.Encoding.Default.GetString(text);
        //                        result += context.Encrypt();
        //                    }

        //                    while (File.Exists(Variables.DestinationFolder + @"\encrypted_" + count.ToString() + ".txt"))
        //                        count++;
        //                    File.WriteAllText(Variables.DestinationFolder + @"\encrypted_" + count.ToString() + ".txt", result);
        //                }
        //                sw.Stop();
        //                MessageBox.Show("Ellapsed Miliseconds: " + sw.ElapsedMilliseconds.ToString());
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.ToString());
        //            }
        //        }
        //    }
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
