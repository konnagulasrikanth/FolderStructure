using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderStructure
{
    public partial class Form1 : Form
    {
        private string filePath = "D:\\Folder Structure Creator";
        private bool isFile = false;
        private string currentlySelectedItemName = "";
        public Form1()
        {
            InitializeComponent();
            treeView1.NodeMouseClick += treeView1_NodeMouseClick;
            PopulateTreeView();
        }
        //private void PopulateTreeView()
        //{
        //    TreeNode rootNode;
        //    DirectoryInfo info = new DirectoryInfo(@"../..");
        //    if (info.Exists)
        //    {
        //        rootNode = new TreeNode(info.Name);
        //        rootNode.Tag = info;
        //        GetDirectories(info.GetDirectories(), rootNode);
        //        treeView1.Nodes.Add(rootNode);
        //    }
        //}
        private void PopulateTreeView()
        {
            string rootPath = @"D:\Folder Structure Creator";

            // Check if the root path exists
            if (!Directory.Exists(rootPath))
            {
                MessageBox.Show("Root directory does not exist.");
                return;
            }

            // Create the root node for the tree view
            TreeNode rootNode = new TreeNode(rootPath);
            rootNode.Tag = rootPath;

            // Populate the tree view with the directory structure
            PopulateTreeView(rootPath, rootNode);

            // Add the root node to the tree view
            treeView1.Nodes.Add(rootNode);
        }

        private void PopulateTreeView(string directoryPath, TreeNode parentNode)
        {
            try
            {
                // Get all directories within the current directory
                string[] directories = Directory.GetDirectories(directoryPath);

                // Traverse each directory recursively
                foreach (string directory in directories)
                {
                    // Create a directory node
                    TreeNode directoryNode = new TreeNode(Path.GetFileName(directory));
                    directoryNode.Tag = directory;

                    // Recursively populate the subtree
                    PopulateTreeView(directory, directoryNode);

                    // Add the directory node to the parent node
                    parentNode.Nodes.Add(directoryNode);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Handle unauthorized access exceptions if necessary
                parentNode.Nodes.Add("Access denied");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                parentNode.Nodes.Add($"Error: {ex.Message}");
            }
        }

        private void GetDirectories(DirectoryInfo[] subDirs, TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = filePath;
            loadFilesAndDirectories();
        }
        public void loadFilesAndDirectories()
        {
            DirectoryInfo fileList;
            string tempFilePath = "";
            FileAttributes fileAttr;
            try
            {

                if (isFile)
                {
                    tempFilePath = filePath + "/" + currentlySelectedItemName;
                    FileInfo fileDetails = new FileInfo(tempFilePath);
                    //fileNameLabel.Text = fileDetails.Name;
                    //fileTypeLabel.Text = fileDetails.Extension;
                    fileAttr = File.GetAttributes(tempFilePath);
                    Process.Start(tempFilePath);
                }
                else
                {
                    fileAttr = File.GetAttributes(filePath);

                }

                if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    fileList = new DirectoryInfo(filePath);
                    FileInfo[] files = fileList.GetFiles(); // GET ALL THE FILES
                    DirectoryInfo[] dirs = fileList.GetDirectories(); // GET ALL THE DIRS
                    string fileExtension = "";
                    listView1.Items.Clear();

                    for (int i = 0; i < files.Length; i++)
                    {
                        fileExtension = files[i].Extension.ToUpper();
                        switch (fileExtension)
                        {
                            case ".MP3":
                            case ".MP2":
                                listView1.Items.Add(files[i].Name, 5);
                                break;
                            case ".EXE":
                            case ".COM":
                                listView1.Items.Add(files[i].Name, 7);
                                break;

                            case ".MP4":
                            case ".AVI":
                            case ".MKV":
                                listView1.Items.Add(files[i].Name, 6);
                                break;
                            case ".PDF":
                                listView1.Items.Add(files[i].Name, 4);
                                break;
                            case ".DOC":
                            case ".DOCX":
                                listView1.Items.Add(files[i].Name, 3);
                                break;
                            case ".PNG":
                            case ".JPG":
                            case ".JPEG":
                                listView1.Items.Add(files[i].Name, 9);
                                break;

                            default:
                                listView1.Items.Add(files[i].Name, 8);
                                break;
                        }

                    }

                    for (int i = 0; i < dirs.Length; i++)
                    {
                        listView1.Items.Add(dirs[i].Name, 10);
                    }
                }
                else
                {
                    //fileNameLabel.Text = this.currentlySelectedItemName;
                }
            }
            catch (Exception e)
            {

            }
        }
        public void loadButtonAction()
        {
            removeBackSlash();
            filePath = textBox1.Text;
            loadFilesAndDirectories();
            isFile = false;
        }
        public void removeBackSlash()
        {
            string path = textBox1.Text;
            if (path.LastIndexOf("/") == path.Length - 1)
            {
               textBox1.Text = path.Substring(0, path.Length - 1);
            }
        }

        public void goBack()
        {
            try
            {
                removeBackSlash();
                string path = textBox1.Text;
                path = path.Substring(0, path.LastIndexOf("/"));
                this.isFile = false;
                textBox1.Text = path;
                removeBackSlash();
            }
            catch (Exception e)
            {

            }
        }
        private void button4_Click(object sender, EventArgs e)  //back button
        {

            
            goBack();
            loadButtonAction();

        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            currentlySelectedItemName = e.Item.Text;

            FileAttributes fileAttr = File.GetAttributes(filePath + "/" + currentlySelectedItemName);
            if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                isFile = false;
                textBox1.Text = filePath + "/" + currentlySelectedItemName;
            }
            else
            {
                isFile = true;
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            loadButtonAction();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadButtonAction();

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //TreeNode newSelected = e.Node;
            //listView1.Items.Clear();
            //DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
            //ListViewItem.ListViewSubItem[] subItems;
            //ListViewItem item = null;
            //foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
            //{
            //    item = new ListViewItem(dir.Name, 0);
            //    subItems = new ListViewItem.ListViewSubItem[]
            //    {new ListViewItem.ListViewSubItem(item,"Directory"),
            //    new ListViewItem.ListViewSubItem(item,
            //    dir.LastAccessTime.ToShortDateString())
            //    };
            //    item.SubItems.AddRange(subItems);
            //    listView1.Items.Add(item);
            //}
            //foreach (FileInfo file in nodeDirInfo.GetFiles())
            //{
            //    item = new ListViewItem(file.Name, 1);
            //    subItems = new ListViewItem.ListViewSubItem[]
            //    {new ListViewItem.ListViewSubItem(item,"File"),
            //    new ListViewItem.ListViewSubItem(item,
            //    file.LastAccessTime.ToShortDateString())
            //    };
            //    item.SubItems.AddRange(subItems);
            //    listView1.Items.Add(item);
            //}
            //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            TreeNode selectedNode = e.Node;
            listView1.Items.Clear();

            if (selectedNode.Tag is string directoryPath) // Check if Tag is a string (directory path)
            {
                try
                {
                    // Attempt to create a DirectoryInfo object from the directory path
                    DirectoryInfo nodeDirInfo = new DirectoryInfo(directoryPath);

                    // Populate the ListView with directories
                    foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
                    {
                        ListViewItem item = new ListViewItem(dir.Name, 0);
                        ListViewItem.ListViewSubItem[] subItems =
                        {
                    new ListViewItem.ListViewSubItem(item, "Directory"),
                    new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToShortDateString())
                };
                        item.SubItems.AddRange(subItems);
                        listView1.Items.Add(item);
                    }

                    // Populate the ListView with files
                    foreach (FileInfo file in nodeDirInfo.GetFiles())
                    {
                        ListViewItem item = new ListViewItem(file.Name, 1);
                        ListViewItem.ListViewSubItem[] subItems =
                        {
                    new ListViewItem.ListViewSubItem(item, "File"),
                    new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString())
                };
                        item.SubItems.AddRange(subItems);
                        listView1.Items.Add(item);
                    }

                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error accessing directory: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Tag is not a valid directory path.");
            }
        }

        private void button2_Click(object sender, EventArgs e)  //create folder button
        {
            try
            {
                // Get the current file path from the text box
                string currentPath = textBox1.Text.Trim();

                // Check if the current path is a valid directory
                if (!Directory.Exists(currentPath))
                {
                    MessageBox.Show("Invalid directory path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Prompt the user to enter a new folder name
                string newFolderName = PromptForFolderName("Enter new folder name:");
                if (string.IsNullOrEmpty(newFolderName))
                {
                    MessageBox.Show("Folder name cannot be empty.", "Invalid Folder Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create the new folder within the current path
                string newFolderPath = Path.Combine(currentPath, newFolderName);
                Directory.CreateDirectory(newFolderPath);

                // Create default subfolders within the new folder
                string[] subfolderNames = { "Requirement", "Build&Design", "Testing","Deployment" };

                foreach (string subfolderName in subfolderNames)
                {
                    string subfolderPath = Path.Combine(newFolderPath, subfolderName);
                    Directory.CreateDirectory(subfolderPath);
                }

                // Refresh the file list after creating folders
                loadFilesAndDirectories();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating folders: {ex.Message}", "Folder Creation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string PromptForFolderName(string promptText)
        {
            using (var form = new Form())
            {
                form.Text = "Enter Folder Name";
                var label = new Label() { Left = 20, Top = 20, Text = promptText };
                var textBox = new TextBox() { Left = 20, Top = 50, Width = 200 };
                var button = new Button() { Text = "OK", Left = 230, Top = 48, DialogResult = DialogResult.OK };
                button.Click += (sender, e) => { form.Close(); };
                form.Controls.Add(label);
                form.Controls.Add(textBox);
                form.Controls.Add(button);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    return textBox.Text.Trim();
                }
            }

            return null; // User canceled or input was empty
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current file path from the text box
                string currentPath = textBox1.Text.Trim();

                // Check if the current path is a valid directory
                if (!Directory.Exists(currentPath))
                {
                    MessageBox.Show("Invalid directory path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Prompt the user to enter a new folder name
                string newFolderName = PromptForFolderName("Enter new folder name:");
                if (string.IsNullOrEmpty(newFolderName))
                {
                    MessageBox.Show("Folder name cannot be empty.", "Invalid Folder Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create the new folder within the current path
                string newFolderPath = Path.Combine(currentPath, newFolderName);
                Directory.CreateDirectory(newFolderPath);

                // Create default subfolders within the new folder
                string[] subfolderNames = { "Requirement", "Build&Design", "Testing", "Deployment" };

                foreach (string subfolderName in subfolderNames)
                {
                    string subfolderPath = Path.Combine(newFolderPath, subfolderName);
                    Directory.CreateDirectory(subfolderPath);
                }

                // Refresh the file list after creating folders
                loadFilesAndDirectories();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating folders: {ex.Message}", "Folder Creation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void listView1_KeyDown(object sender, KeyEventArgs e) //delete code in listview
        {

            if (e.KeyCode == Keys.Delete)
            {
                // Check if there is a selected item in the ListView
                if (listView1.SelectedItems.Count > 0)
                {
                    // Loop through each selected item (can be multiple if selection mode is set to MultiSimple or MultiExtended)
                    foreach (ListViewItem selectedItem in listView1.SelectedItems)
                    {
                        // Get the name of the selected item
                        string itemName = selectedItem.Text;

                        // Get the full path of the item (assuming it's a file or directory)
                        string itemPath = Path.Combine(filePath, itemName);

                        try
                        {
                            // Check if the selected item is a file or directory
                            if (File.Exists(itemPath))
                            {
                                // It's a file, so delete it
                                File.Delete(itemPath);
                            }
                            else if (Directory.Exists(itemPath))
                            {
                                // It's a directory, so delete it recursively
                                Directory.Delete(itemPath, true);
                            }

                            // Remove the selected item from the ListView
                            listView1.Items.Remove(selectedItem);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting '{itemName}': {ex.Message}", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
// Custom comparer class for sorting ListView items
public class ListViewItemComparer : IComparer<ListViewItem>
{
    private int columnIndex;

    public ListViewItemComparer(int columnIndex)
    {
        this.columnIndex = columnIndex;
    }

    public int Compare(ListViewItem x, ListViewItem y)
    {
        // Compare ListView items based on the specified column index
        return StringComparer.CurrentCultureIgnoreCase.Compare(x.SubItems[columnIndex].Text, y.SubItems[columnIndex].Text);
    }
}