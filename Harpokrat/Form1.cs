using System;
using System.Windows.Forms;
using Harpokrat.Constants;

namespace Harpokrat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Initializations of constant variables
            Methods.InitializeVariables();

            srcFolderTextBox.Text = Variables.SourceFolder;
            dstFolderTextBox.Text = Variables.DestinationFolder;
        }

        private void sourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog();

            DialogResult userClickedOK = fileDialog.ShowDialog();

            if (userClickedOK == DialogResult.OK)
                Variables.SourceFolder = fileDialog.SelectedPath;
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

        #endregion

        //Encryption key button
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            DialogResult clickedOK = openFileDialog.ShowDialog();

            if (clickedOK == DialogResult.OK)
            {
                MessageBox.Show(openFileDialog.FileName);
                encKeyText.Text = openFileDialog.FileName;

                Variables.EncryptionKey = openFileDialog.FileName;
            }
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
    }
}
