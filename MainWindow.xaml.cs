using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using Winforms = System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace WindowProjects
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FilePathListBinding.ItemsSource = selectedFileList;
            FolderListBinding.ItemsSource = selectedFolderList;
            //Add Method
            MethodListView.ItemsSource = methodList;           
            DataContext = this;
        }

        BindingList<FileInformation> selectedFileList = new BindingList<FileInformation>();
        BindingList<FolderInformation> selectedFolderList = new BindingList<FolderInformation>();

        public class FileInformation
        {
            public string filePath { get; set; }
            public string fileName { get; set; }
            public string fileExtension { get; set; }
            
        }

        public class FolderInformation
        {
            public string parent { get; set; }
            public string folderName { get; set; }
            public string newName { get; set; }
        }

        private void Add_File_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openAddFileDialog = new OpenFileDialog();
            openAddFileDialog.Filter = "All Files (*.*)|*.*";
            openAddFileDialog.Multiselect = true;
            FileInfo file = new FileInfo("C:/");
            string selectedFilePath;
            string[] multipleSelectedFilePath;
            if (openAddFileDialog.ShowDialog() == true)
            {
                selectedFilePath = openAddFileDialog.FileName.ToString();
                multipleSelectedFilePath = openAddFileDialog.FileNames;
            }
            else
            {
                selectedFilePath = string.Empty;
                return;
            }
            
            if (multipleSelectedFilePath.Length > 1) {
                for (int i = 0; i < multipleSelectedFilePath.Length; i++)
                {
                    file = new FileInfo(multipleSelectedFilePath[i]);
                    if (file.Exists)
                    {
                        if (selectedFileList.Count() == 0)
                        { 
                            selectedFileList.Add(new FileInformation(){ fileName = file.Name, filePath = multipleSelectedFilePath[i], fileExtension = file.Extension });  
                        }
                        else
                        {
                            //check files already in the list
                            for (int j = 0; j < selectedFileList.Count; j++)
                            {
                                if (string.Compare(file.Name, selectedFileList[j].fileName) == 0)
                                {
                                    MessageBox.Show(file.Name + "is already selected!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    goto BREAK;
                                }
                            }
                            selectedFileList.Add(new FileInformation() { fileName = file.Name, filePath = multipleSelectedFilePath[i], fileExtension = file.Extension });
                            BREAK:;
                        }
                    }
                    else {
                        MessageBox.Show("File(s) selected doesn't exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
                }
            }
            else
            {
                file = new FileInfo(selectedFilePath);
                if (file.Exists)
                {
                    for (int i = 0; i < selectedFileList.Count; i++)
                    {
                        if (string.Compare(file.Name, selectedFileList[i].fileName) == 0)
                        {
                            MessageBox.Show(file.Name + " is already selected!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                    selectedFileList.Add(new FileInformation() { fileName = file.Name, filePath = selectedFilePath, fileExtension = file.Extension });
                }
                else
                {
                    MessageBox.Show("File(s) selected doesn't exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        string previousSelectedFolder = " ";
        private void Add_Folder_Clicked(object sender, RoutedEventArgs e)
        {
            Winforms.FolderBrowserDialog folderBrowser = new Winforms.FolderBrowserDialog();
            Winforms.DialogResult result = folderBrowser.ShowDialog();
            string selectedDirPath;

            if (result == Winforms.DialogResult.OK)
            {
                selectedDirPath = folderBrowser.SelectedPath;
            }
            else
            {
                selectedDirPath = string.Empty;
                return;
            }

            if (string.Compare(selectedDirPath, previousSelectedFolder) == 0)
            {
                MessageBox.Show("Folder is already selected!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            previousSelectedFolder = selectedDirPath;
            DirectoryInfo selectedDirectory = new DirectoryInfo(selectedDirPath);
            if (selectedDirectory.Exists)
            {
                DirectoryInfo[] subDirectories = selectedDirectory.GetDirectories();
                foreach (object subDir in subDirectories)
                {
                    selectedFolderList.Add(new FolderInformation() { folderName = subDir.ToString(), parent = selectedDirPath, newName = "" });
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Add Method Class
        /// </summary>
        class Method
        {       
            private string methodName;
            private bool isChecked;

            public string MethodName
            {
                get => methodName;
                set
                {
                    methodName = value;
                    RaiseChangeEvent();
                }
            }

            public bool IsChecked
            {
                get => isChecked;
                set
                {
                    isChecked = value;
                    RaiseChangeEvent();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            void RaiseChangeEvent([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        BindingList<Method> methodList = new BindingList<Method>();

        private bool isMethodExist(string methodName)
        {
            bool isExist = false;
            foreach (var item in methodList)
            {
                if (methodName == item.MethodName)
                {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        }

        private void MethodMenuItemClicked(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            string methodName = mi.Header.ToString();

            if (methodList.Count == 0)
            {
                methodList.Add(new Method() { MethodName = methodName, IsChecked = false });
                mi.IsChecked = true;
            }
            else
            {
                if (isMethodExist(methodName))
                {
                    MessageBox.Show("Method '" + methodName + "' Already Exist!!", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    methodList.Add(new Method() { MethodName = methodName, IsChecked = false });
                    mi.IsChecked = true;
                }
            }           
        }

        private void RemoveMethodlButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMethod = MethodListView.SelectedItem as Method;   
           
            for (int i = 0; i < AddMethodMenuItem.Items.Count; i++)
            {
                MenuItem selectedItem = (MenuItem)AddMethodMenuItem.Items[i];
                if ((string)selectedItem.Header == selectedMethod.MethodName)
                {
                    selectedItem.IsChecked = false;
                    break;
                }
            }

            methodList.Remove(selectedMethod);
        }

        //CheckBox class Method
        private void MethodCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var selectedMethod = MethodListView.SelectedItem as Method;
            //MessageBox.Show(selectedMethod.MethodName + ' ' + selectedMethod.IsChecked);
            selectedMethod.IsChecked = true;
        }
    }
}
