using SeContCoreLib;
using SeContCoreLib.FileStructure;
using System.Text.Json;

namespace SeCont
{
    public partial class MainForm : Form
    {
        private const string FILE_ICON = "file-icon";
        private const string ENCRYPTED_FILE_ICON = "locker-icon";
        private const string CONFIG_PATH = "./config.json";
        private const string STARTUP_MESSAGE_TEXT = "";

        private static MainForm? _instance;
        private static MainForm Instance
        {
            get
            {
                if (_instance == null)
                    throw new NullReferenceException();
                return _instance;
            }
        }

        private TreeNode SelectedTreeNode => mainTreeView.SelectedNode;

        private static JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            IncludeFields = true,
        };

        private Config _config = new Config();

        private SecurityContainer _securityContainer = new SecurityContainer();
        public static IReadOnlyList<EncryptedObject> EncryptedObjects { get => Instance._securityContainer.EncryptedObjects; }

        public MainForm()
        {
            _instance = this;
            InitializeComponent();
            mainTreeView.MouseDown += MainTreeView_MouseDown;
            mainTreeView.AfterLabelEdit += MainTreeView_AfterLabelEdit;
            _config = GetConfig();
            ShowFirstStartupMessageGUI();
        }



        #region TreeView

        private void MainTreeView_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;
            var hitTest = mainTreeView.HitTest(e.Location);
            if (hitTest.Node != null)
                mainTreeView.SelectedNode = hitTest.Node;
        }

        private void MainTreeView_AfterLabelEdit(object? sender, NodeLabelEditEventArgs e)
        {
            if (e.Node == null)
                return;

            object encryptedObject = e.Node.Tag;
            string newName = e.Label ?? string.Empty;
            if (string.IsNullOrEmpty(newName))
            {
                e.CancelEdit = true;
                return;
            }
            if (encryptedObject is EncryptedFile encryptedFile)
            {
                RenameFile(encryptedFile, newName);
            }
            else if (encryptedObject is EncryptedDirectory encryptedDirectory)
            {
                RenameDirectory(encryptedDirectory, newName);
            }
        }

        private void BuildTreeView(IEnumerable<EncryptedObject> encryptedObjects)
        {
            foreach (EncryptedObject encryptedObject in encryptedObjects)
            {
                if (encryptedObject is EncryptedDirectory encryptedDirectory)
                {
                    TreeNode treeNode = CreateTreeNode(encryptedDirectory);
                    mainTreeView.Nodes.Add(treeNode);
                }
                else if (encryptedObject is EncryptedFile encryptedFile)
                {
                    TreeNode treeNode = CreateTreeNode(encryptedFile);
                    mainTreeView.Nodes.Add(treeNode);
                }
            }
        }

        private void BuildTreeView(SecurityContainer container)
        {
            BuildTreeView(container.EncryptedObjects);
        }

        private void ClearTreeView() => mainTreeView.Nodes.Clear();

        private void RebuildTreeView(SecurityContainer container)
        {
            ClearTreeView();
            BuildTreeView(container);
        }

        private void RebuildTreeView() => RebuildTreeView(_securityContainer);

        private TreeNode CreateTreeNode(EncryptedDirectory encryptedDirectory)
        {
            TreeNode treeNode = new TreeNode(encryptedDirectory.Name);
            treeNode.Tag = encryptedDirectory;
            treeNode.ContextMenuStrip = directoryMenuStrip;
            foreach (var innerDirectory in encryptedDirectory.GetEncryptedDirectories())
            {
                treeNode.Nodes.Add(CreateTreeNode(innerDirectory));
            }
            foreach (var file in encryptedDirectory.GetEncryptedFiles())
            {
                TreeNode treeNodeFile = CreateTreeNode(file);
                treeNode.Nodes.Add(treeNodeFile);
            }
            return treeNode;
        }

        private TreeNode CreateTreeNode(EncryptedFile encryptedFile)
        {
            TreeNode treeNode = new TreeNode(encryptedFile.Name);
            treeNode.Tag = encryptedFile;
            treeNode.ContextMenuStrip = fileMenuStrip;
            if (!encryptedFile.IsDataEncrypted)
                treeNode.ImageKey = FILE_ICON;
            else
                treeNode.ImageKey = ENCRYPTED_FILE_ICON;
            treeNode.SelectedImageKey = treeNode.ImageKey;
            return treeNode;
        }

        #endregion

        #region MenuStrip

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openSecontFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            PasswordForm passwordForm = new PasswordForm();
            passwordForm.ShowDialog();
            if (passwordForm.canceled)
                return;
            byte[] key = passwordForm.PasswordHash;

            string filePath = openSecontFileDialog.FileName;
            SecurityContainer container;
            try
            {
                container = OpenContainer(key, filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _securityContainer = container;
            RebuildTreeView();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasswordForm passwordForm = new PasswordForm();
            passwordForm.ShowDialog();
            if (passwordForm.canceled)
                return;
            byte[] key = passwordForm.PasswordHash;

            if (saveSecontFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            string filePath = saveSecontFileDialog.FileName;
            try
            {
                SaveContainer(key, filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region TreeMenuStrip

        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFilesGUI(mainTreeView.Nodes, _securityContainer);
        }

        private void addDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDirectoryGUI(mainTreeView.Nodes, _securityContainer);
        }

        private void createDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateDirectoryGUI(mainTreeView.Nodes, _securityContainer);
        }

        #endregion

        #region Save/Open container

        private SecurityContainer OpenContainer(byte[] key, string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("Specified file doesn't exist.");

            byte[] fileData = File.ReadAllBytes(filePath);
            SecurityContainer container = SeContFile.Deserialize(key, fileData);
            return container;
        }

        private void SaveContainer(byte[] key, string filePath)
        {
            byte[] fileData = SeContFile.Serialize(key, _securityContainer);
            File.WriteAllBytes(filePath, fileData);
        }

        #endregion

        #region FileMenuStrip

        private void extractFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtractFileGUI();
        }

        private void renameFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameFileGUI();
        }

        private void encryptFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EncryptFileGUI();
        }

        private void decryptFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DecryptFileGUI();
        }

        private void removeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveFileGUI();
        }

        #endregion

        #region DirectoryMenuStrip

        private void addFilesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EncryptedDirectory? encryptedDirectory = GetSelectedEncryptedObject<EncryptedDirectory>(out TreeNode selectedNode);
            if (encryptedDirectory == null)
                return;

            AddFilesGUI(selectedNode.Nodes, encryptedDirectory);

            selectedNode.Expand();
        }

        private void addDirectoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EncryptedDirectory? encryptedDirectory = GetSelectedEncryptedObject<EncryptedDirectory>(out TreeNode selectedNode);
            if (encryptedDirectory == null)
                return;

            AddDirectoryGUI(selectedNode.Nodes, encryptedDirectory);

            selectedNode.Expand();
        }

        private void extractDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtractDirectoryGUI();
        }

        private void renameDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameDirectoryGUI();
        }

        private void createDirectoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EncryptedDirectory? encryptedDirectory = GetSelectedEncryptedObject<EncryptedDirectory>(out TreeNode selectedNode);
            if (encryptedDirectory == null)
                return;

            CreateDirectoryGUI(selectedNode.Nodes, encryptedDirectory);

            selectedNode.Expand();
        }

        private void removeDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveDirectoryGUI();
        }

        #endregion

        #region GUI Methods

        private void AddFilesGUI(TreeNodeCollection parentNodeCollection, IEncryptedStorage encryptedStorage)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            string[] filePaths = openFileDialog.FileNames;
            foreach (string filePath in filePaths)
            {
                EncryptionAlgorithm? encryptionAlgorithm = null;
                byte[] key = Array.Empty<byte>();
                if (filePaths.Length == 1)
                {
                    GetEncryptionAndKey(true, out encryptionAlgorithm, out key);
                }

                EncryptedFile encryptedFile = AddFile(filePath, key, encryptionAlgorithm, encryptedStorage);
                TreeNode fileNode = CreateTreeNode(encryptedFile);
                parentNodeCollection.Add(fileNode);
            }
        }

        private void AddDirectoryGUI(TreeNodeCollection parentNodeCollection, IEncryptedStorage encryptedStorage)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel)
                return;
            string directoryPath = folderBrowserDialog.SelectedPath;
            EncryptedDirectory encryptedDirectory = AddDirectory(directoryPath, encryptedStorage, true);
            TreeNode directoryNode = CreateTreeNode(encryptedDirectory);
            parentNodeCollection.Add(directoryNode);
        }

        private void CreateDirectoryGUI(TreeNodeCollection parentNodeCollection, IEncryptedStorage encryptedStorage, string directoryName = "New directory")
        {
            EncryptedDirectory encryptedDirectory = new EncryptedDirectory(directoryName);
            TreeNode directoryNode = CreateTreeNode(encryptedDirectory);
            encryptedStorage.AddEncryptedDirectory(encryptedDirectory);
            parentNodeCollection.Add(directoryNode);
        }



        private void ExtractFileGUI()
        {
            EncryptedFile? encryptedFile = GetSelectedEncryptedObject<EncryptedFile>(out _);
            if (encryptedFile == null)
                return;

            byte[] key = Array.Empty<byte>();
            if (encryptedFile.IsDataEncrypted)
            {
                PasswordForm passwordForm = new PasswordForm();
                passwordForm.ShowDialog();
                if (passwordForm.canceled)
                    return;
                key = passwordForm.PasswordHash;
            }

            saveFileDialog.FileName = encryptedFile.Name;
            DialogResult saveFileDialogResult = saveFileDialog.ShowDialog();
            string filePath = saveFileDialog.FileName;
            saveFileDialog.FileName = string.Empty;

            if (saveFileDialogResult == DialogResult.Cancel)
                return;

            try
            {
                if (!encryptedFile.IsDataEncrypted)
                {
                    ExtractFile(encryptedFile, filePath);
                    return;
                }
                ExtractFile(encryptedFile, key, filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenameFileGUI()
        {
            TreeNode selectedNode = SelectedTreeNode;
            if (selectedNode == null)
                return;

            selectedNode.BeginEdit();
        }

        private void EncryptFileGUI()
        {
            EncryptedFile? encryptedFile = GetSelectedEncryptedObject<EncryptedFile>(out TreeNode selectedNode);
            if (encryptedFile == null)
                return;

            try
            {
                if (EncryptFileData(encryptedFile))
                {
                    selectedNode.ImageKey = ENCRYPTED_FILE_ICON;
                    selectedNode.SelectedImageKey = selectedNode.ImageKey;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DecryptFileGUI()
        {
            EncryptedFile? encryptedFile = GetSelectedEncryptedObject<EncryptedFile>(out TreeNode selectedNode);
            if (encryptedFile == null)
                return;

            try
            {
                if (DecryptFileData(encryptedFile))
                {
                    selectedNode.ImageKey = FILE_ICON;
                    selectedNode.SelectedImageKey = selectedNode.ImageKey;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveFileGUI()
        {
            EncryptedFile? encryptedFile = GetSelectedEncryptedObject<EncryptedFile>(out TreeNode selectedNode);
            if (encryptedFile == null)
                return;

            encryptedFile.Remove();
            selectedNode.Remove();
        }



        private void ExtractDirectoryGUI()
        {
            EncryptedDirectory? encryptedDirectory = GetSelectedEncryptedObject<EncryptedDirectory>(out TreeNode selectedNode);
            if (encryptedDirectory == null)
                return;

            if (folderBrowserDialog.ShowDialog() == DialogResult.Cancel)
                return;
            string parentDirectoryPath = folderBrowserDialog.SelectedPath;
            ExtractDirectory(encryptedDirectory, parentDirectoryPath);
        }

        private void RenameDirectoryGUI()
        {
            TreeNode selectedNode = SelectedTreeNode;
            if (selectedNode == null)
                return;

            selectedNode.BeginEdit();
        }

        private void RemoveDirectoryGUI()
        {
            EncryptedDirectory? encryptedDirectory = GetSelectedEncryptedObject<EncryptedDirectory>(out TreeNode selectedNode);
            if (encryptedDirectory == null)
                return;

            encryptedDirectory.Remove();
            selectedNode.Remove();
        }

        private void ShowFirstStartupMessageGUI()
        {
            if (_config.ShowStartupMessage)
            {
                MessageBox.Show(STARTUP_MESSAGE_TEXT, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _config.ShowStartupMessage = false;
                SaveConfig(_config);
            }
        }


        #endregion




        #region Helper methods

        private Config GetConfig()
        {
            try
            {
                if (!File.Exists(CONFIG_PATH))
                {
                    Config newConfig = new Config();
                    SaveConfig(newConfig);
                    return newConfig;
                }
                string configJson = File.ReadAllText(CONFIG_PATH);
                Config config = JsonSerializer.Deserialize<Config>(configJson, JsonOptions) ?? new Config();
                return config;
            }
            catch
            {
                return new Config();
            }
        }

        private void SaveConfig(Config config)
        {
            try
            {
                if (File.Exists(CONFIG_PATH))
                    File.Delete(CONFIG_PATH);
                string configJson = JsonSerializer.Serialize(config, JsonOptions);
                File.WriteAllText(CONFIG_PATH, configJson);
            }
            catch { }
        }

        private void GetEncryptionAndKey(bool showEncryptionSelection, out EncryptionAlgorithm? encryptionAlgorithm, out byte[] key)
        {
            if (showEncryptionSelection)
            {
                EncryptionAlgorithmSelectionForm encryptionSelectionForm = new();
                encryptionSelectionForm.ShowDialog();
                encryptionAlgorithm = encryptionSelectionForm.SelectedEncryptionAlgorithm;
                if (encryptionAlgorithm == null)
                {
                    key = Array.Empty<byte>();
                    return;
                }
            }
            else
            {
                encryptionAlgorithm = null;
            }
            PasswordForm passwordForm = new PasswordForm();
            passwordForm.ShowDialog();
            if (passwordForm.canceled)
                key = Array.Empty<byte>();
            else
                key = passwordForm.PasswordHash;
        }

        private T? GetSelectedEncryptedObject<T>(out TreeNode selectedNode) where T : EncryptedObject
        {
            selectedNode = SelectedTreeNode;
            if (selectedNode == null)
                return null;

            return selectedNode.Tag as T;
        }

        private void RenameFile(EncryptedFile encryptedFile, string newName)
        {
            encryptedFile.Name = newName;
        }

        private void RenameDirectory(EncryptedDirectory encryptedDirectory, string newName)
        {
            encryptedDirectory.Name = newName;
        }

        private EncryptedFile AddFile(string filePath, byte[] key, EncryptionAlgorithm? encryptionAlgorithm, IEncryptedStorage encryptedStorage)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException("Specified file doesn't exist.");

            byte[] fileData = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            EncryptedFile encryptedFile = new EncryptedFile(fileName, encryptionAlgorithm);

            if (key.Length == 0 && !encryptedFile.IsDataEncrypted)
                encryptedFile.FileData = fileData;
            else
                encryptedFile.EncryptFileData(key, fileData);

            encryptedStorage.AddEncryptedFile(encryptedFile);

            return encryptedFile;
        }

        private EncryptedFile AddFile(string filePath, IEncryptedStorage encryptedStorage)
        {
            return AddFile(filePath, Array.Empty<byte>(), null, encryptedStorage);
        }

        private EncryptedDirectory AddDirectory(string directoryPath, IEncryptedStorage encryptedStorage, bool recursive = true)
        {
            string directoryName = Path.GetFileName(directoryPath) ?? "Unnamed directory";
            IEnumerable<string> filesPaths = Directory.EnumerateFiles(directoryPath);
            EncryptedDirectory encryptedDirectory = new EncryptedDirectory(directoryName);
            EncryptedFile[] encryptedFiles = new EncryptedFile[filesPaths.Count()];
            for (int i = 0; i < encryptedFiles.Length; i++)
            {
                string filePath = filesPaths.ElementAt(i);
                string fileName = Path.GetFileName(filePath);
                byte[] fileData = File.ReadAllBytes(filePath);
                EncryptedFile encryptedFile = new EncryptedFile(fileName, null);
                encryptedFile.FileData = fileData;
                encryptedFiles[i] = encryptedFile;
            }
            encryptedDirectory.AddEncryptedFiles(encryptedFiles);
            encryptedStorage.AddEncryptedDirectory(encryptedDirectory);

            if (!recursive)
                return encryptedDirectory;

            IEnumerable<string> innerDirectoriesPaths = Directory.EnumerateDirectories(directoryPath);
            foreach (string innerDirectoryPath in innerDirectoriesPaths)
            {
                AddDirectory(innerDirectoryPath, encryptedDirectory, true);
            }

            return encryptedDirectory;
        }

        private void ExtractFile(EncryptedFile encryptedFile, byte[] key, string filePath)
        {
            byte[] fileData;
            if (key.Length == 0 && !encryptedFile.IsDataEncrypted)
                fileData = encryptedFile.FileData;
            else
                fileData = encryptedFile.DecryptFileData(key);
            File.WriteAllBytes(filePath, fileData);
        }

        private void ExtractFile(EncryptedFile encryptedFile, string filePath)
        {
            ExtractFile(encryptedFile, Array.Empty<byte>(), filePath);
        }

        private bool EncryptFileData(EncryptedFile encryptedFile)
        {
            if (encryptedFile.IsDataEncrypted)
            {
                MessageBox.Show("File data is already encrypted, no need to encrypt it again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            GetEncryptionAndKey(true, out var encryptionAlgorithm, out var key);
            if (encryptionAlgorithm == null || key.Length == 0)
                return false;
            encryptedFile.EncryptFileDataInside(key, encryptionAlgorithm);
            return true;
        }

        private bool DecryptFileData(EncryptedFile encryptedFile)
        {
            if (!encryptedFile.IsDataEncrypted)
            {
                MessageBox.Show("File data is not encrypted, no need to decrypt it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            GetEncryptionAndKey(false, out _, out var key);
            if (key.Length == 0)
                return false;
            encryptedFile.DecryptFileDataInside(key);
            return true;
        }

        private void ExtractDirectory(EncryptedDirectory encryptedDirectory, string parentDirectoryPath)
        {
            string directoryName = NormalizeFileName(encryptedDirectory.Name);
            string directoryPath = Path.Combine(parentDirectoryPath, directoryName);
            EncryptedDirectory[] innerEncryptedDirectories = encryptedDirectory.GetEncryptedDirectories();
            EncryptedFile[] encryptedFiles = encryptedDirectory.GetEncryptedFiles();

            Directory.CreateDirectory(directoryPath);

            foreach (var encryptedFile in encryptedFiles)
            {
                string fileName = NormalizeFileName(encryptedFile.Name);
                string filePath = Path.Combine(directoryPath, fileName);
                if (encryptedFile.IsDataEncrypted)
                {
                    MessageBox.Show($"The file \"{encryptedFile.Name}\" is encrypted and should be decrypted before extracting.\r\n" +
                        $"Type the password for this file in the next window.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    GetEncryptionAndKey(false, out _, out var key);
                    if (key.Length == 0)
                    {
                        MessageBox.Show("No password was typed. The file will be skipped.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }
                    try
                    {
                        ExtractFile(encryptedFile, key, filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }
                }
                else
                {
                    ExtractFile(encryptedFile, filePath);
                }
            }

            foreach (var innerEncryptedDirectory in innerEncryptedDirectories)
            {
                ExtractDirectory(innerEncryptedDirectory, directoryPath);
            }
        }

        private static string NormalizeFileName(string fileName, char replaceChar = '_')
        {
            var forbiddenChars = Path.GetInvalidFileNameChars();
            foreach (char forbiddenChar in forbiddenChars)
            {
                fileName = fileName.Replace(forbiddenChar, replaceChar);
            }
            return fileName;
        }

        #endregion


    }
}
