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

            // Init FileSystemWatcher - begin to monitor folder specified in Constants.Variables.SourceFolder
            Methods.StartFileSystemWatcher();
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

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog();

            DialogResult userClickedOK = fileDialog.ShowDialog();

            if (userClickedOK == DialogResult.OK)
                Variables.SourceFolder = fileDialog.SelectedPath;

            srcFolderTextBox.Text = Variables.SourceFolder;
        }
    }
}
