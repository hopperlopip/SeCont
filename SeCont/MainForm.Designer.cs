namespace SeCont
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            mainTreeView = new TreeView();
            treeMenuStrip = new ContextMenuStrip(components);
            addFilesToolStripMenuItem = new ToolStripMenuItem();
            addDirectoryToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            createDirectoryToolStripMenuItem = new ToolStripMenuItem();
            iconImageList = new ImageList(components);
            directoryMenuStrip = new ContextMenuStrip(components);
            addFilesToolStripMenuItem1 = new ToolStripMenuItem();
            addDirectoryToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            extractDirectoryToolStripMenuItem = new ToolStripMenuItem();
            renameDirectoryToolStripMenuItem = new ToolStripMenuItem();
            createDirectoryToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            removeDirectoryToolStripMenuItem = new ToolStripMenuItem();
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            openSecontFileDialog = new OpenFileDialog();
            saveSecontFileDialog = new SaveFileDialog();
            openFileDialog = new OpenFileDialog();
            fileMenuStrip = new ContextMenuStrip(components);
            extractFileToolStripMenuItem = new ToolStripMenuItem();
            renameFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            encryptFileToolStripMenuItem = new ToolStripMenuItem();
            decryptFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            removeFileToolStripMenuItem = new ToolStripMenuItem();
            saveFileDialog = new SaveFileDialog();
            folderBrowserDialog = new FolderBrowserDialog();
            treeMenuStrip.SuspendLayout();
            directoryMenuStrip.SuspendLayout();
            menuStrip.SuspendLayout();
            fileMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // mainTreeView
            // 
            mainTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainTreeView.ContextMenuStrip = treeMenuStrip;
            mainTreeView.ImageIndex = 0;
            mainTreeView.ImageList = iconImageList;
            mainTreeView.LabelEdit = true;
            mainTreeView.Location = new Point(12, 27);
            mainTreeView.Name = "mainTreeView";
            mainTreeView.SelectedImageIndex = 0;
            mainTreeView.Size = new Size(540, 329);
            mainTreeView.TabIndex = 0;
            // 
            // treeMenuStrip
            // 
            treeMenuStrip.Items.AddRange(new ToolStripItem[] { addFilesToolStripMenuItem, addDirectoryToolStripMenuItem, toolStripSeparator3, createDirectoryToolStripMenuItem });
            treeMenuStrip.Name = "treeMenuStrip";
            treeMenuStrip.Size = new Size(159, 76);
            // 
            // addFilesToolStripMenuItem
            // 
            addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
            addFilesToolStripMenuItem.Size = new Size(158, 22);
            addFilesToolStripMenuItem.Text = "Add files";
            addFilesToolStripMenuItem.Click += addFilesToolStripMenuItem_Click;
            // 
            // addDirectoryToolStripMenuItem
            // 
            addDirectoryToolStripMenuItem.Name = "addDirectoryToolStripMenuItem";
            addDirectoryToolStripMenuItem.Size = new Size(158, 22);
            addDirectoryToolStripMenuItem.Text = "Add directory";
            addDirectoryToolStripMenuItem.Click += addDirectoryToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(155, 6);
            // 
            // createDirectoryToolStripMenuItem
            // 
            createDirectoryToolStripMenuItem.Name = "createDirectoryToolStripMenuItem";
            createDirectoryToolStripMenuItem.Size = new Size(158, 22);
            createDirectoryToolStripMenuItem.Text = "Create directory";
            createDirectoryToolStripMenuItem.Click += createDirectoryToolStripMenuItem_Click;
            // 
            // iconImageList
            // 
            iconImageList.ColorDepth = ColorDepth.Depth32Bit;
            iconImageList.ImageStream = (ImageListStreamer)resources.GetObject("iconImageList.ImageStream");
            iconImageList.TransparentColor = Color.Transparent;
            iconImageList.Images.SetKeyName(0, "folder-icon");
            iconImageList.Images.SetKeyName(1, "file-icon");
            iconImageList.Images.SetKeyName(2, "enc-file-icon");
            iconImageList.Images.SetKeyName(3, "locker-icon");
            // 
            // directoryMenuStrip
            // 
            directoryMenuStrip.Items.AddRange(new ToolStripItem[] { addFilesToolStripMenuItem1, addDirectoryToolStripMenuItem1, toolStripSeparator4, extractDirectoryToolStripMenuItem, renameDirectoryToolStripMenuItem, createDirectoryToolStripMenuItem1, toolStripSeparator1, removeDirectoryToolStripMenuItem });
            directoryMenuStrip.Name = "nodeMenuStrip";
            directoryMenuStrip.Size = new Size(168, 148);
            // 
            // addFilesToolStripMenuItem1
            // 
            addFilesToolStripMenuItem1.Name = "addFilesToolStripMenuItem1";
            addFilesToolStripMenuItem1.Size = new Size(167, 22);
            addFilesToolStripMenuItem1.Text = "Add files";
            addFilesToolStripMenuItem1.Click += addFilesToolStripMenuItem1_Click;
            // 
            // addDirectoryToolStripMenuItem1
            // 
            addDirectoryToolStripMenuItem1.Name = "addDirectoryToolStripMenuItem1";
            addDirectoryToolStripMenuItem1.Size = new Size(167, 22);
            addDirectoryToolStripMenuItem1.Text = "Add directory";
            addDirectoryToolStripMenuItem1.Click += addDirectoryToolStripMenuItem1_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(164, 6);
            // 
            // extractDirectoryToolStripMenuItem
            // 
            extractDirectoryToolStripMenuItem.Name = "extractDirectoryToolStripMenuItem";
            extractDirectoryToolStripMenuItem.Size = new Size(167, 22);
            extractDirectoryToolStripMenuItem.Text = "Extract directory";
            extractDirectoryToolStripMenuItem.Click += extractDirectoryToolStripMenuItem_Click;
            // 
            // renameDirectoryToolStripMenuItem
            // 
            renameDirectoryToolStripMenuItem.Name = "renameDirectoryToolStripMenuItem";
            renameDirectoryToolStripMenuItem.Size = new Size(167, 22);
            renameDirectoryToolStripMenuItem.Text = "Rename directory";
            renameDirectoryToolStripMenuItem.Click += renameDirectoryToolStripMenuItem_Click;
            // 
            // createDirectoryToolStripMenuItem1
            // 
            createDirectoryToolStripMenuItem1.Name = "createDirectoryToolStripMenuItem1";
            createDirectoryToolStripMenuItem1.Size = new Size(167, 22);
            createDirectoryToolStripMenuItem1.Text = "Create directory";
            createDirectoryToolStripMenuItem1.Click += createDirectoryToolStripMenuItem1_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(164, 6);
            // 
            // removeDirectoryToolStripMenuItem
            // 
            removeDirectoryToolStripMenuItem.Name = "removeDirectoryToolStripMenuItem";
            removeDirectoryToolStripMenuItem.Size = new Size(167, 22);
            removeDirectoryToolStripMenuItem.Text = "Remove directory";
            removeDirectoryToolStripMenuItem.Click += removeDirectoryToolStripMenuItem_Click;
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(564, 24);
            menuStrip.TabIndex = 2;
            menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveAsToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(123, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(123, 22);
            saveAsToolStripMenuItem.Text = "Save As...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(123, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // openSecontFileDialog
            // 
            openSecontFileDialog.Filter = "SeCont files|*.secont";
            // 
            // saveSecontFileDialog
            // 
            saveSecontFileDialog.DefaultExt = "secont";
            saveSecontFileDialog.Filter = "SeCont files|*.secont";
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "All files|*.*";
            openFileDialog.Multiselect = true;
            // 
            // fileMenuStrip
            // 
            fileMenuStrip.Items.AddRange(new ToolStripItem[] { extractFileToolStripMenuItem, renameFileToolStripMenuItem, toolStripSeparator5, encryptFileToolStripMenuItem, decryptFileToolStripMenuItem, toolStripSeparator2, removeFileToolStripMenuItem });
            fileMenuStrip.Name = "fileMenuStrip";
            fileMenuStrip.Size = new Size(137, 126);
            // 
            // extractFileToolStripMenuItem
            // 
            extractFileToolStripMenuItem.Name = "extractFileToolStripMenuItem";
            extractFileToolStripMenuItem.Size = new Size(136, 22);
            extractFileToolStripMenuItem.Text = "Extract file";
            extractFileToolStripMenuItem.Click += extractFileToolStripMenuItem_Click;
            // 
            // renameFileToolStripMenuItem
            // 
            renameFileToolStripMenuItem.Name = "renameFileToolStripMenuItem";
            renameFileToolStripMenuItem.Size = new Size(136, 22);
            renameFileToolStripMenuItem.Text = "Rename file";
            renameFileToolStripMenuItem.Click += renameFileToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(133, 6);
            // 
            // encryptFileToolStripMenuItem
            // 
            encryptFileToolStripMenuItem.Name = "encryptFileToolStripMenuItem";
            encryptFileToolStripMenuItem.Size = new Size(136, 22);
            encryptFileToolStripMenuItem.Text = "Encrypt file";
            encryptFileToolStripMenuItem.Click += encryptFileToolStripMenuItem_Click;
            // 
            // decryptFileToolStripMenuItem
            // 
            decryptFileToolStripMenuItem.Name = "decryptFileToolStripMenuItem";
            decryptFileToolStripMenuItem.Size = new Size(136, 22);
            decryptFileToolStripMenuItem.Text = "Decrypt file";
            decryptFileToolStripMenuItem.Click += decryptFileToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(133, 6);
            // 
            // removeFileToolStripMenuItem
            // 
            removeFileToolStripMenuItem.Name = "removeFileToolStripMenuItem";
            removeFileToolStripMenuItem.Size = new Size(136, 22);
            removeFileToolStripMenuItem.Text = "Remove file";
            removeFileToolStripMenuItem.Click += removeFileToolStripMenuItem_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(564, 368);
            Controls.Add(menuStrip);
            Controls.Add(mainTreeView);
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SeCont";
            treeMenuStrip.ResumeLayout(false);
            directoryMenuStrip.ResumeLayout(false);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            fileMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView mainTreeView;
        private ContextMenuStrip treeMenuStrip;
        private ContextMenuStrip directoryMenuStrip;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private OpenFileDialog openSecontFileDialog;
        private SaveFileDialog saveSecontFileDialog;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem addFilesToolStripMenuItem;
        private ToolStripMenuItem createDirectoryToolStripMenuItem;
        private OpenFileDialog openFileDialog;
        private ContextMenuStrip fileMenuStrip;
        private SaveFileDialog saveFileDialog;
        private ToolStripMenuItem extractFileToolStripMenuItem;
        private ToolStripMenuItem removeFileToolStripMenuItem;
        private ImageList iconImageList;
        private ToolStripMenuItem addFilesToolStripMenuItem1;
        private ToolStripMenuItem createDirectoryToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem removeDirectoryToolStripMenuItem;
        private ToolStripMenuItem renameDirectoryToolStripMenuItem;
        private ToolStripMenuItem renameFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private FolderBrowserDialog folderBrowserDialog;
        private ToolStripMenuItem addDirectoryToolStripMenuItem;
        private ToolStripMenuItem addDirectoryToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem extractDirectoryToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem encryptFileToolStripMenuItem;
        private ToolStripMenuItem decryptFileToolStripMenuItem;
    }
}
