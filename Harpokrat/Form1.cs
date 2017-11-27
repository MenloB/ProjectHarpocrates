using System;
using System.Windows.Forms;
using Harpokrat.Constants;

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

            context.SetEncryptionStrategy(new EncryptionAlgorithms.SimpleSubstitutionStrategy());
        }

        // Encrypt button
        private void button4_Click(object sender, EventArgs e)
        {

            if (context.Test())
                MessageBox.Show("RADI.", "Strategy set.",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
            else
                MessageBox.Show("NE RADI.", "Strategy not set.",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1);
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

        }

        #endregion
    }
}
