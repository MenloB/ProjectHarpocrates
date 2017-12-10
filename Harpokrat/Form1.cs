using System;
using System.Threading;
using System.Windows.Forms;
using Harpokrat.Constants;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Security.Permissions;

namespace Harpokrat
{
    public partial class Form1 : Form
    {
        // Context for dinamically changing algorithms in run-time
        public static EncryptionAlgorithms.Context context = new EncryptionAlgorithms.Context();
        private FileSystemWatcher fsw;
        public string KeyTextBox
        {
            get
            {
                return textBox3.Text;
            }
            set
            {
                textBox3.Text = value;
            }
        }

        private object ThreadLock;
        private object AlgorithmLock;
        private object FSWLock;

        public Form1()
        {
            InitializeComponent();

            //Initializations of constant variables
            Methods.InitializeVariables();

            ThreadLock    = new object();
            AlgorithmLock = new object();
            FSWLock       = new object();

            //Thread.CurrentThread.Name = "Main Thread";

            srcFolderTextBox.Text                    = Variables.SourceFolder;
            dstFolderTextBox.Text                    = Variables.DestinationFolder;
            loadKeyFromFileToolStripMenuItem.Checked = true;
            radioButton1.Checked                     = true;

            fsw = new FileSystemWatcher();
        }

        #region Form_Load
        private void Form1_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();

            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;

            toolTip.ShowAlways = true;

            toolTip.SetToolTip(this.button10, "Select file you want to encrypt. (it will save it in the same destination)");
            toolTip.SetToolTip(this.textBox5, "Select file you want to encrypt. (it will save it in the same destination)");
        }
        #endregion

        #region MenuStrip_item_function
        private void loadKeyFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Variables.KeyFromFile = !Variables.KeyFromFile;

            if (Variables.KeyFromFile)
                loadKeyFromFileToolStripMenuItem.Checked = true;
            else
                loadKeyFromFileToolStripMenuItem.Checked = false;
        }

        private void fileSystemWatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //negation of bool
            Variables.FileWatcherEnabled = !Variables.FileWatcherEnabled;

            if (Variables.FileWatcherEnabled)
                fileSystemWatcherToolStripMenuItem.Checked = true;
            else
                fileSystemWatcherToolStripMenuItem.Checked = false;
        }
        #endregion

        #region FileSystemWatcher_Group

        // Button for selecting Source Folder
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog();

            DialogResult userClickedOK = fileDialog.ShowDialog();

            if (userClickedOK == DialogResult.OK)
                Variables.SourceFolder = fileDialog.SelectedPath;

            srcFolderTextBox.Text = Variables.SourceFolder;
        }

        // Button for selecting destination folder
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog();

            DialogResult userClickedOK = fileDialog.ShowDialog();

            if (userClickedOK == DialogResult.OK)
                Variables.DestinationFolder = fileDialog.SelectedPath;

            dstFolderTextBox.Text = Variables.DestinationFolder;
        }

        // Button to monitor selected source folder
        private void button3_Click(object sender, EventArgs e)
        {
            // Init FileSystemWatcher - begin to monitor folder specified in Constants.Variables.SourceFolder
            StartFileSystemWatcher();

            MessageBox.Show("Watcher attached to selected source folder.", "Watcher started.",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }

        private void stopListeningWatcherButton_Click(object sender, EventArgs e)
        {
            StopFileSystemWatcher();

            MessageBox.Show("Watcher deatached from selected source folder.", "Watcher stoped.",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }
        #endregion

        #region Encrypt_section_buttons
        //Encryption key button
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            DialogResult clickedOK = openFileDialog.ShowDialog();

            if (clickedOK == DialogResult.OK)
            {
                MessageBox.Show(openFileDialog.FileName);
                encKeyText.Text = openFileDialog.FileName;
                Variables.EncryptionKey = Methods.OpenFile(openFileDialog.FileName);
            }

            // sets strategy for our context
            switch(Variables.Algorithm)
            {
                case 0:
                    context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy());
                    break;
                case 1:
                    context.SetEncryptionStrategy(new EncryptionAlgorithms.A51());
                    break;
                case 2:
                    context.SetEncryptionStrategy(new EncryptionAlgorithms.XTEA());
                    break;
                case 3:
                    context.SetEncryptionStrategy(new EncryptionAlgorithms.RSA());
                    break;
            }
        }

        // Encrypt button
        private void button4_Click(object sender, EventArgs e)
        {
            //var sw = Stopwatch.StartNew();
            context.Message = textBox1.Text;
            MessageBox.Show(context.Encrypt());
            textBox2.Text = context.Encrypt();
            //sw.Stop();
            //MessageBox.Show("Encrypted within {0}", sw.ElapsedMilliseconds.ToString());
        }
        #endregion

        #region Decrypt_section_buttons
        // Button for selecting decryption key file
        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            DialogResult userClickedOK = fileDialog.ShowDialog();

            if (userClickedOK == DialogResult.OK)
                decKeyText.Text = fileDialog.FileName;
        }

        // Button for decrypting
        private void button5_Click(object sender, EventArgs e)
        {
            // sets private field of context to a message
            context.Message = textBox2.Text;
            // decrypts using decrypt from strategy
            MessageBox.Show(context.Decrypt());
            // outputs the result
            textBox1.Text = context.Decrypt();
        }

        #endregion

        #region SET_KEYS
        // Set Encryption Key
        private void button8_Click(object sender, EventArgs e)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Variables.EncryptionKey = encoding.GetBytes(textBox3.Text.ToLower());
            switch (Variables.Algorithm)
            {
                case 0:
                    context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy());
                    break;
                case 1:
                    context.SetEncryptionStrategy(new EncryptionAlgorithms.A51());
                    break;
                case 2:
                    context.SetEncryptionStrategy(new EncryptionAlgorithms.XTEA());
                    break;
                case 3:
                    context.SetEncryptionStrategy(new EncryptionAlgorithms.RSA());
                    break;
            }
        }

        // Set Decryption key
        private void button9_Click(object sender, EventArgs e)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Variables.EncryptionKey = encoding.GetBytes(textBox4.Text.ToLower());
            switch (Variables.Algorithm)
            {
                case 0:
                    context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy(encoding.GetBytes(textBox4.Text.ToLower())));
                    break;
                case 1:
                    context.SetEncryptionStrategy(new EncryptionAlgorithms.A51());
                    break;
                case 2:
                    context.SetEncryptionStrategy(new EncryptionAlgorithms.XTEA());
                    break;
                case 3:
                    context.SetEncryptionStrategy(new EncryptionAlgorithms.RSA());
                    break;
            }
        }
        #endregion

        // File to encrypt
        private void button10_Click(object sender, EventArgs e)
        {
            if(!(textBox3.Text.Trim().Equals(""))) { 
                var fileDialog = new OpenFileDialog();
                DialogResult userClickedOK = fileDialog.ShowDialog();

                if (userClickedOK == DialogResult.OK)
                {
                    textBox5.Text = fileDialog.FileName;
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] key = encoding.GetBytes(textBox3.Text.ToLower());

                    Thread t = new Thread(() => EncryptAFile(fileDialog.FileName, key));
                    t.Name = "EncryptAFile thread";
                    t.Start();

                }
                else
                {
                    MessageBox.Show("You need to enter a key.");
                }
            }
        }

        // File to decrypt
        private void button11_Click(object sender, EventArgs e)
        {
            if(!(textBox4.Text.Trim().Equals("")))
            {
                var fileDialog = new OpenFileDialog();
                DialogResult userClickedOK = fileDialog.ShowDialog();

                if(userClickedOK == DialogResult.OK)
                {
                    textBox6.Text = fileDialog.FileName;
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] key = encoding.GetBytes(textBox4.Text);

                    Thread t = new Thread(() => DecryptAFile(fileDialog.FileName, key));
                    t.Start();
                }
            }
        }

        #region Helper functions
        // Helper functions
        public void EncryptAFile(string file, byte[] key)
        {
            int count = 1;
            string result = "";
            var sw = Stopwatch.StartNew();

            Monitor.Enter(AlgorithmLock);
            Monitor.Enter(ThreadLock);

            try
            {
                using (FileStream fs = File.OpenRead(file))
                {
                    byte[] text = new byte[16 * 1024];
                    while (fs.Read(text, 0, text.Length) > 0)
                    {
                        switch (Variables.Algorithm)
                        {
                            case 0:
                                context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy(key));
                                break;
                            case 1:
                                context.SetEncryptionStrategy(new EncryptionAlgorithms.A51());
                                break;
                            case 2:
                                context.SetEncryptionStrategy(new EncryptionAlgorithms.XTEA());
                                break;
                            case 3:
                                context.SetEncryptionStrategy(new EncryptionAlgorithms.RSA());
                                break;
                        }
                        context.Message = System.Text.Encoding.Default.GetString(text);
                        result += context.Encrypt();
                    }

                    while (File.Exists(Path.GetDirectoryName(file) + @"\encrypted_" + count.ToString() + ".txt"))
                        count++;
                    File.WriteAllText(Path.GetDirectoryName(file) + @"\encrypted_" + count.ToString() + ".txt", result);
                }
                sw.Stop();
                MessageBox.Show("Ellapsed Miliseconds: " + sw.ElapsedMilliseconds.ToString());
                MessageBox.Show("Thread: " + Thread.CurrentThread.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Monitor.Exit(ThreadLock);
                Monitor.Exit(AlgorithmLock);
            }
        }

        public void EncryptAFile(string file, byte[] key, string destination)
        {
            int count = 1;
            string result = "";
            var sw = Stopwatch.StartNew();

            Monitor.Enter(AlgorithmLock);
            Monitor.Enter(ThreadLock);

            try
            {
                using (FileStream fs = File.OpenRead(file))
                {
                    byte[] text = new byte[16 * 1024];
                    while (fs.Read(text, 0, text.Length) > 0)
                    {
                        switch (Variables.Algorithm)
                        {
                            case 0:
                                context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy(key));
                                break;
                            case 1:
                                context.SetEncryptionStrategy(new EncryptionAlgorithms.A51());
                                break;
                            case 2:
                                context.SetEncryptionStrategy(new EncryptionAlgorithms.XTEA());
                                break;
                            case 3:
                                context.SetEncryptionStrategy(new EncryptionAlgorithms.RSA());
                                break;
                        }
                        context.Message = System.Text.Encoding.Default.GetString(text);
                        result += context.Encrypt();
                    }

                    while (File.Exists(destination + @"\encrypted_" + count.ToString() + ".txt"))
                        count++;
                    File.WriteAllText(destination + @"\encrypted_" + count.ToString() + ".txt", result);
                }
                sw.Stop();
                MessageBox.Show("Ellapsed Miliseconds: " + sw.ElapsedMilliseconds.ToString());
                MessageBox.Show("Thread: " + Thread.CurrentThread.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Monitor.Exit(ThreadLock);
                Monitor.Exit(AlgorithmLock);
            }
        }

        public void DecryptAFile(string file, byte[] key)
        {
            int count = 1;
            string result = "";
            var sw = Stopwatch.StartNew();

            Monitor.Enter(AlgorithmLock);
            Monitor.Enter(ThreadLock);

            try
            {
                using (FileStream fs = File.OpenRead(file))
                {
                    byte[] text = new byte[16 * 1024];
                    while (fs.Read(text, 0, text.Length) > 0)
                    {
                        switch (Variables.Algorithm)
                        {
                            case 0:
                                context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy(key));
                                break;
                            case 1:
                                context.SetEncryptionStrategy(new EncryptionAlgorithms.A51());
                                break;
                            case 2:
                                context.SetEncryptionStrategy(new EncryptionAlgorithms.XTEA());
                                break;
                            case 3:
                                context.SetEncryptionStrategy(new EncryptionAlgorithms.RSA());
                                break;
                        }

                        context.Message = System.Text.Encoding.Default.GetString(text);
                        // when it wrong key is used the output file will be blank
                        result = context.Decrypt();
                    }

                    while (File.Exists(Path.GetDirectoryName(file) + @"\decrypted_" + count.ToString() + ".txt"))
                        count++;
                    File.WriteAllText(Path.GetDirectoryName(file) + @"\decrypted_" + count.ToString() + ".txt", result);
                }
            }
            catch(IOException e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                Monitor.Exit(ThreadLock);
                Monitor.Exit(AlgorithmLock);
            }
        }

        [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
        public void StartFileSystemWatcher()
        {
            try
            {
                fsw.Path = Variables.SourceFolder;

                fsw.NotifyFilter = NotifyFilters.FileName |
                    NotifyFilters.DirectoryName |
                    NotifyFilters.LastAccess |
                    NotifyFilters.LastWrite;

                fsw.Filter = "*.*";
                fsw.EnableRaisingEvents = true;
                fsw.Created += new FileSystemEventHandler(onCreated);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void onCreated(object sender, FileSystemEventArgs args)
        {
            try
            {
                Thread t = new Thread(() => EncryptAFile(args.FullPath, Variables.EncryptionKey, Variables.DestinationFolder));
                t.Name = "FSW Thread.";
                t.Start();
            }

            catch (NullReferenceException e)
            {
                MessageBox.Show(this, "Invalid key.", "Null Reference Exception",
                    MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }

        public void StopFileSystemWatcher()
        {
            fsw.EnableRaisingEvents = false;
            fsw.Created -= new FileSystemEventHandler(onCreated);
        }
        #endregion

        #region RADIO BUTTONS
        // Radio buttons that initialize Variables.Algorithm (initially is always 0 (SimpleSubstitution)
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Monitor.Enter(AlgorithmLock);
            try
            {
                Variables.Algorithm = 0;
            }
            finally
            {
                Monitor.Exit(AlgorithmLock);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Monitor.Enter(AlgorithmLock);
            try
            {
                Variables.Algorithm = 1;
            }
            finally
            {
                Monitor.Exit(AlgorithmLock);
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Monitor.Enter(AlgorithmLock);
            try
            {
                Variables.Algorithm = 2;
            }
            finally
            {
                Monitor.Exit(AlgorithmLock);
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Monitor.Enter(AlgorithmLock);
            try
            {
                Variables.Algorithm = 3;
            }
            finally
            {
                Monitor.Exit(AlgorithmLock);
            }
        }
        #endregion
    }
}
