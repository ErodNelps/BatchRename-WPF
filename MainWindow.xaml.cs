﻿using System;
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
using WindowsProgramming;

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

        public class FileInformation : INotifyPropertyChanged
        {
            public string filePath { get; set; }
            public string fileName { get; set; }
            public string fileExtension { get; set; }
            public string newName { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;

            public void UpdateData(string originName)
            {
                this.filePath.Replace(originName, fileName);
                RaiseChangeEvent("filePath");
                RaiseChangeEvent("fileName");
                RaiseChangeEvent("fileExtension");
            }

            void RaiseChangeEvent(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class FolderInformation : INotifyPropertyChanged
        {
            public string parent { get; set; }
            public string folderName { get; set; }
            public string newName { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;

            void RaiseChangeEvent(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
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
            
            if (multipleSelectedFilePath.Length > 1)
            {
                for (int i = 0; i < multipleSelectedFilePath.Length; i++)
                {
                    file = new FileInfo(multipleSelectedFilePath[i]);
                    if (file.Exists)
                    {
                        if (selectedFileList.Count() == 0)
                        {
                            selectedFileList.Add(new FileInformation() { fileName = file.Name, newName = file.Name, filePath = multipleSelectedFilePath[i], fileExtension = file.Extension });
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
                            selectedFileList.Add(new FileInformation() { fileName = file.Name, newName = file.Name, filePath = multipleSelectedFilePath[i], fileExtension = file.Extension });
                        BREAK:;
                        }
                    }
                    else
                    {
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
                    selectedFileList.Add(new FileInformation() { fileName = file.Name, newName = file.Name, filePath = selectedFilePath, fileExtension = file.Extension });
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
        /// Adding method into action list (Add method button event)
        /// </summary>
        BindingList<IMethodAction> methodList = new BindingList<IMethodAction>() { };

        private void MethodMenuItemClicked(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            string methodName = item.Header.ToString();
            
            switch (methodName) {
                case "New Case":
                    methodList.Add(new NewCaseAction() { methodArgs = new NewCaseArgs() { }, MethodName = methodName, IsChecked = true });
                    break;
                case "Remove Pattern":
                    methodList.Add(new RemoveAction() { methodArgs = new RemovePatternArgs() { Pattern = "" }, MethodName = methodName, IsChecked = true });
                    break;
                case "Replace":
                    methodList.Add(new ReplaceAction() { methodArgs = new ReplaceArgs() { Target = "", Replacer = "" }, MethodName = methodName, IsChecked = true });
                    break;
                case "Trim":
                    methodList.Add(new TrimAction() { methodArgs = new TrimArgs() { trimChars = ";.,-"}, MethodName = methodName, IsChecked = true });                
                    break;
                case "Move":
                    methodList.Add(new MoveAction() { methodArgs = new MoveArgs() { FromPos = 0, ToPos = 0, Length = 0 }, MethodName = methodName, IsChecked = true });                   
                    break;
                case "New Name":
                    methodList.Add(new NewNameAction() { methodArgs = new NewNameArgs() { NewName = "Default"}, MethodName = methodName, IsChecked = true });                    
                    break;
                default:
                    break;
            }
            
        }

        private void RemoveMethodlButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMethod = MethodListView.SelectedItem as IMethodAction;

            for (int i = 0; i < AddMethodMenuItem.Items.Count; i++)
            {
                MenuItem selectedItem = (MenuItem)AddMethodMenuItem.Items[i];
                if (selectedItem.Header.ToString() == selectedMethod.MethodName)
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
            var selectedMethod = MethodListView.SelectedItem as ReplaceAction;
            selectedMethod.IsChecked = true;
        }

        
        //METHODS
        
        private static int FindFirstAlphabetChar(string str)
        {
            if (str == null)
                return -1;

            for (var i = 0; i < str.Length; i += 1)
            {
                if ((str.ElementAt(i) >= 'A' && str.ElementAt(i) <= 'Z') || (str.ElementAt(i) >= 'a' && str.ElementAt(i) <= 'z'))
                {
                    return i;
                }
            }
            return -1;
        }

        private static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            int indexOfFirstChar = FindFirstAlphabetChar(str);
            if (indexOfFirstChar == -1)
                return str;

            if (str.Length > 1)
            {
                return (str.Substring(0, indexOfFirstChar) + char.ToUpper(str[indexOfFirstChar]) + str.Substring(indexOfFirstChar + 1));
            }

            return str.ToUpper();
        }

        private static string FullNameNormalize(string source)
        {
            if (source == null)
                return null;

            //Remove extra space
            source = String.Join(" ", source.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            source = FirstLetterToUpper(source);

            return source;
        }

        private static string UniqueName(string source)
        {
            if (source == null)
                return null;

            string guid = Guid.NewGuid().ToString();

            return guid;
        }

        private void UpdateDetail_Clicked(object sender, RoutedEventArgs e)
        {
            var item = MethodListView.SelectedItem as IMethodAction;
            item.ShowUpdateDetailWindow();
        }

        private void Starting_Batch(object sender, RoutedEventArgs e)
        {
            for(int i=0; i < selectedFileList.Count; i++)
            {
                string originalPath = selectedFileList[i].filePath;
                string originalName = selectedFileList[i].fileName;
                foreach (var action in methodList)
                {
                    selectedFileList[i].fileName = action.Process(selectedFileList[i].fileName);
                }
                selectedFileList[i].filePath = selectedFileList[i].filePath.Replace(originalName, selectedFileList[i].fileName);
                File.Move(originalPath, selectedFileList[i].filePath);
                selectedFileList[i].UpdateData(originalName);
            }
        }

        
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            methodList.Clear();
            selectedFileList.Clear();
            selectedFolderList.Clear();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            methodList.Clear();
        }

        private void SwapFile(int indexA, int indexB)
        {
            var temp = selectedFileList[indexA];
            selectedFileList[indexA] = selectedFileList[indexB];
            selectedFileList[indexB] = temp;
        }

        private void UpFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathListBinding.SelectedItem == null)
            {
                MessageBox.Show("Please select item!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int selectedIndex = FilePathListBinding.SelectedIndex;

            if (selectedIndex == 0)
            {
                FilePathListBinding.SelectedItem = selectedFileList[0];
                return;
            }
            else
            {
                SwapFile(selectedIndex, selectedIndex - 1);
                FilePathListBinding.SelectedItem = selectedFileList[selectedIndex - 1];
            } 
        }

        private void DownFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathListBinding.SelectedItem == null)
            {
                MessageBox.Show("Please select item!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int selectedIndex = FilePathListBinding.SelectedIndex;

            if (selectedIndex == selectedFileList.Count - 1)
            {
                FilePathListBinding.SelectedItem = selectedFileList[selectedFileList.Count - 1];
                return;
            }
            else
            {
                SwapFile(selectedIndex, selectedIndex + 1);
                FilePathListBinding.SelectedItem = selectedFileList[selectedIndex + 1];
            }
        }

        private void UpMostFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathListBinding.SelectedItem == null)
            {
                MessageBox.Show("Please select item!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int selectedIndex = FilePathListBinding.SelectedIndex;

            if (selectedIndex == 0)
            {
                return;
            }
            else
            {
                selectedFileList.Insert(0, selectedFileList[selectedIndex]);
                selectedFileList.RemoveAt(selectedIndex + 1);
                FilePathListBinding.SelectedItem = selectedFileList[0];
            }
        }

        private void DownMostFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathListBinding.SelectedItem == null)
            {
                MessageBox.Show("Please select item!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int selectedIndex = FilePathListBinding.SelectedIndex;

            if (selectedIndex == selectedFileList.Count - 1)
            {
                return;
            }
            else
            {
                selectedFileList.Insert(selectedFileList.Count, selectedFileList[selectedIndex]);
                selectedFileList.RemoveAt(selectedIndex);
                FilePathListBinding.SelectedItem = selectedFileList[selectedFileList.Count - 1];
            }
        }

        private void SwapFolder(int indexA, int indexB)
        {
            var temp = selectedFolderList[indexA];
            selectedFolderList[indexA] = selectedFolderList[indexB];
            selectedFolderList[indexB] = temp;
        }

        private void UpFolderButton_click(object sender, RoutedEventArgs e)
        {
            if (FolderListBinding.SelectedItem == null)
            {
                MessageBox.Show("Please select item!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int selectedIndex = FolderListBinding.SelectedIndex;

            if (selectedIndex == 0)
            {
                FolderListBinding.SelectedItem = selectedFolderList[0];
                return;
            }
            else
            {
                SwapFolder(selectedIndex, selectedIndex - 1);
                FolderListBinding.SelectedItem = selectedFolderList[selectedIndex - 1];
            }
        }

        private void DownFolderButton_click(object sender, RoutedEventArgs e)
        {
            if (FolderListBinding.SelectedItem == null)
            {
                MessageBox.Show("Please select item!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int selectedIndex = FolderListBinding.SelectedIndex;

            if (selectedIndex == selectedFolderList.Count - 1)
            {
                FolderListBinding.SelectedItem = selectedFolderList[selectedFolderList.Count - 1];
                return;
            }
            else
            {
                SwapFolder(selectedIndex, selectedIndex + 1);
                FolderListBinding.SelectedItem = selectedFolderList[selectedIndex + 1];
            }
        }

        private void UpMostFolderButton_click(object sender, RoutedEventArgs e)
        {
            if (FolderListBinding.SelectedItem == null)
            {
                MessageBox.Show("Please select item!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int selectedIndex = FolderListBinding.SelectedIndex;

            if (selectedIndex == 0)
            {
                return;
            }
            else
            {
                selectedFolderList.Insert(0, selectedFolderList[selectedIndex]);
                selectedFolderList.RemoveAt(selectedIndex + 1);
                FolderListBinding.SelectedItem = selectedFolderList[0];
            }
        }

        private void DownMostFolderButton_click(object sender, RoutedEventArgs e)
        {
            if (FolderListBinding.SelectedItem == null)
            {
                MessageBox.Show("Please select item!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int selectedIndex = FolderListBinding.SelectedIndex;

            if (selectedIndex == selectedFolderList.Count - 1)
            {
                return;
            }
            else
            {
                selectedFolderList.Insert(selectedFolderList.Count, selectedFolderList[selectedIndex]);
                selectedFolderList.RemoveAt(selectedIndex);
                FolderListBinding.SelectedItem = selectedFolderList[selectedFolderList.Count - 1];
            }
        }
    }
}
