using System;
using System.Windows.Forms;
using Harpokrat.Constants;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace Harpokrat
{
    public partial class Form1 : Form
    {
        // Context for dinamically changing algorithms in run-time
        public static EncryptionAlgorithms.Context context = new EncryptionAlgorithms.Context();

        public Form1()
        {
            InitializeComponent();

            //Initializations of constant variables
            Methods.InitializeVariables();

            srcFolderTextBox.Text                    = Variables.SourceFolder;
            dstFolderTextBox.Text                    = Variables.DestinationFolder;
            loadKeyFromFileToolStripMenuItem.Checked = true;
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
            Methods.StartFileSystemWatcher();

            MessageBox.Show("Watcher attached to selected source folder.", "Watcher started.",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }

        private void stopListeningWatcherButton_Click(object sender, EventArgs e)
        {
            Methods.StopFileSystemWatcher();

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
            context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy());
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

        // Set Encryption Key
        private void button8_Click(object sender, EventArgs e)
        {
            UTF8Encoding encoding   = new UTF8Encoding();
            Variables.EncryptionKey = encoding.GetBytes(textBox3.Text.ToLower());
            context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy());
        }

        // Set Decryption key
        private void button9_Click(object sender, EventArgs e)
        {
            UTF8Encoding encoding   = new UTF8Encoding();
            Variables.EncryptionKey = encoding.GetBytes(textBox4.Text.ToLower());
            context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy(encoding.GetBytes(textBox4.Text.ToLower())));
        }

        // File to encrypt
        private void button10_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();

            DialogResult userClickedOK = fileDialog.ShowDialog();

            if(userClickedOK == DialogResult.OK)
            {
                textBox5.Text = fileDialog.FileName;
                string result = "";
                var sw = Stopwatch.StartNew();
                try
                {
                    using (FileStream fs = File.OpenRead(fileDialog.FileName))
                    {
                        byte[] text = new byte[16*1024];
                        UTF8Encoding encoding = new UTF8Encoding();
                        byte[] key = encoding.GetBytes(textBox3.Text.ToLower());
                        while(fs.Read(text, 0, text.Length) > 0)
                        {
                            context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy(key));
                            context.Message = System.Text.Encoding.Default.GetString(text);
                            result += context.Encrypt();
                        }

                        File.WriteAllText(Path.GetDirectoryName(fileDialog.FileName) + @"\encrypted.txt", result);
                    }
                    sw.Stop();
                    MessageBox.Show("Ellapsed Miliseconds: " + sw.ElapsedMilliseconds.ToString());
                } catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
