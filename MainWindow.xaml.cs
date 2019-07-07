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
using WindowsProgramming;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Globalization;
using System.Threading;

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

            //CREATE A FILE CONTAINS PRESETS
            FileInfo newFile = new FileInfo("./PresetsFile.txt");
            if (!newFile.Exists)
            {
                FileStream fs = newFile.Create();
                fs.Close();
            }

            //PRESET COMBOBOX
            using (StreamReader sr = presetsPathFile.OpenText())
            {
                string PresetFilePath = "";
                while ((PresetFilePath = sr.ReadLine()) != null)
                {
                    FileInfo PresetFile = new FileInfo(PresetFilePath);
                    savedPresetFiles.Add(new FileInformation() { fileName = PresetFile.Name, filePath = PresetFilePath, originalExtension = PresetFile.Extension });
                }
            }

            PresetCombobox.ItemsSource = savedPresetFiles;
            PresetCombobox.DisplayMemberPath = "fileName";

            DataContext = this;
        }
        
        BindingList<FileInformation> selectedFileList = new BindingList<FileInformation>();
        BindingList<FolderInformation> selectedFolderList = new BindingList<FolderInformation>();
        BindingList<IMethodAction> methodList = new BindingList<IMethodAction>() { };
        public class FileInformation : INotifyPropertyChanged
        {
            public string filePath { get; set; }
            public string fileName { get; set; }
            //Name without extension
            public string realName { get; set; }
            public string originalExtension { get; set; }
            public string newExt { get; set; }
            public string newName { get; set; }
            public string fileError { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;

            public void UpdatePreview()
            {
                RaiseChangeEvent("newName");
                RaiseChangeEvent("newExt");
            }
            //public void UpdatePreview(string originName)
            //{
            //    this.filePath.Replace(originName, fileName);
            //    RaiseChangeEvent("filePath");
            //    RaiseChangeEvent("fileName");
            //    RaiseChangeEvent("fileExtension");
            //}

            void RaiseChangeEvent(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class FolderInformation : INotifyPropertyChanged
        {
            public string folderPath { get; set; }
            public string folderName { get; set; }
            public string newName { get; set; }
            public string folderError { get; set; }
            public event PropertyChangedEventHandler PropertyChanged;

            void RaiseChangeEvent(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            }
            public void UpdatePreview()
            {
                RaiseChangeEvent("newName");
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
                            selectedFileList.Add(new FileInformation() { fileName = file.Name, newName = file.Name, realName = file.Name.Replace(file.Extension.Length == 0 ? " " : file.Extension, ""), filePath = multipleSelectedFilePath[i], originalExtension = file.Extension, newExt = file.Extension, fileError = "OK" });
                            OnFileListChange();
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
                            selectedFileList.Add(new FileInformation() { fileName = file.Name, newName = file.Name, realName = file.Name.Replace(file.Extension.Length==0? " ": file.Extension, ""), filePath = multipleSelectedFilePath[i], originalExtension = file.Extension, newExt = file.Extension, fileError = "OK" });
                            OnFileListChange();
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
                    selectedFileList.Add(new FileInformation() { fileName = file.Name, newName = file.Name, realName = file.Name.Replace(file.Extension.Length == 0 ? " ": file.Extension, ""), filePath = selectedFilePath, originalExtension = file.Extension, newExt = file.Extension, fileError = "OK" });
                    OnFileListChange();
                }
                else
                {
                    MessageBox.Show("File(s) selected doesn't exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        string previousSelectedFolder = "";
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
                    selectedFolderList.Add(new FolderInformation() { folderName = subDir.ToString(), folderPath = selectedDirPath + subDir.ToString(), newName = subDir.ToString(), folderError = "OK" });
                }
                OnFolderListChange();
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Adding method into action list (Add method button event)
        /// </summary>


        private void MethodMenuItemClicked(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            string methodName = item.Header.ToString();

            switch (methodName) {
                case "New Case":
                    methodList.Add(new NewCaseAction() { methodArgs = new NewCaseArgs() { }, isApplyToName = true, MethodName = methodName, IsChecked = true });
                    WireEventHandlers(methodList.Last());
                    break;
                case "Remove Pattern":
                    methodList.Add(new RemoveAction() { methodArgs = new RemovePatternArgs() { Pattern = "" }, isApplyToName = true, MethodName = methodName, IsChecked = true });
                    WireEventHandlers(methodList.Last());
                    break;
                case "Replace":
                    methodList.Add(new ReplaceAction() { methodArgs = new ReplaceArgs() { Target = "", Replacer = "" }, isApplyToName = true, MethodName = methodName, IsChecked = true });
                    WireEventHandlers(methodList.Last());
                    break;
                case "Trim":
                    methodList.Add(new TrimAction() { methodArgs = new TrimArgs() { trimCharacters = "" }, isApplyToName = true, MethodName = methodName, IsChecked = true });
                    WireEventHandlers(methodList.Last());
                    break;
                case "Move":
                    methodList.Add(new MoveAction() { methodArgs = new MoveArgs() { FromPos = 0, Length = 0, ToPos = 0 }, isApplyToName = true, MethodName = methodName, IsChecked = true });
                    WireEventHandlers(methodList.Last());
                    break;
                case "New Name":
                    methodList.Add(new NewNameAction() { methodArgs = new NewNameArgs() { NewName = "Default" }, isApplyToName = true, MethodName = methodName, IsChecked = true });
                    WireEventHandlers(methodList.Last());
                    break;
                case "Fullname Normalize":
                    methodList.Add(new FullnameNormalizeAction() { methodArgs = new FullnameNormalizeArgs() { }, isApplyToName = true, MethodName = methodName, IsChecked = true });
                    WireEventHandlers(methodList.Last());
                    break;
                case "Unique ID":
                    methodList.Add(new UniqueIDAction() { methodArgs = new UniqueIDArgs() { }, isApplyToName = true, MethodName = methodName, IsChecked = true });
                    WireEventHandlers(methodList.Last());
                    break;
                default:
                    break;
            }

        }

        private void RemoveMethodlButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                methodList.Remove(MethodListView.SelectedItem as IMethodAction);
                foreach (var item in selectedFileList)
                {
                    item.newName = item.fileName;
                    item.newExt = item.originalExtension;
                    item.UpdatePreview();
                    OnMethodListChanged();
                }
                foreach (var item in selectedFolderList)
                {
                    item.newName = item.folderName;
                    item.UpdatePreview();
                    OnMethodListChanged();
                }
            }
            catch (Exception ex)
            {
                foreach (var item in selectedFileList)
                {
                    item.fileError = ex.ToString();
                    item.newName = item.fileName;
                    item.newExt = item.originalExtension;
                    item.UpdatePreview();
                    OnMethodListChanged();
                }
                foreach (var item in selectedFolderList)
                {
                    item.folderError = ex.ToString();
                    item.newName = item.folderName;
                    item.UpdatePreview();
                    OnMethodListChanged();
                }
            }
        }

        //CheckBox class Method
        private void MethodCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var selectedMethod = MethodListView.SelectedItem as IMethodAction;
            selectedMethod.IsChecked = true;
            foreach (var item in selectedFileList)
            {
                item.newName = item.fileName;
                item.newExt = item.originalExtension;
                item.realName = item.newName.Trim().Replace(item.newExt.Length == 0 ? " " : item.newExt, "");
                item.UpdatePreview();
                OnMethodListChanged();
            }
            foreach (var item in selectedFolderList)
            {
                item.newName = item.folderName;
                item.UpdatePreview();
                OnMethodListChanged();
            }
        }
        //Method Checkbox Events
        private void MethodCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var selectedMethod = MethodListView.SelectedItem as IMethodAction;
            selectedMethod.IsChecked = false;
            foreach (var item in selectedFileList)
            {
                item.newName = item.fileName;
                item.newExt = item.originalExtension;
                item.realName = item.newName.Trim().Replace(item.newExt.Length == 0 ? " " : item.newExt, "");
                item.UpdatePreview();
                OnMethodListChanged();
            }
            foreach (var item in selectedFolderList)
            {
                item.newName = item.folderName;
                item.UpdatePreview();
                OnMethodListChanged();
            }           
        }
        //Method Edit Events
        private void UpdateDetail_Clicked(object sender, RoutedEventArgs e)
        {
            var item = MethodListView.SelectedItem as IMethodAction;
            item.ShowUpdateDetailWindow();
        }
        //Form connection between windows (Subcribe function)
        private void WireEventHandlers(IMethodAction action)
        {
            UpdateEventHandler handler = new UpdateEventHandler(OnHandler1);
            action.newNameEvent += handler;
        }
        public void OnHandler1(object sender, UpdateEvent e)
        {
            OnMethodListChanged();
        }
        //File list Changed Event
        public void OnFolderListChange()
        {
            try
            {
                if (methodList.Count == 0)
                {
                    return;
                }
                OnMethodListChanged();
            }
            catch (Exception ex)
            {
                for (int i = 0; i < selectedFileList.Count; i++)
                {
                    selectedFolderList[i].folderError = ex.ToString();
                }
            }
        }
        public void OnFileListChange()
        {
            try
            {
                if (methodList.Count == 0)
                {
                    return;
                }
                OnMethodListChanged();
            }
            catch (Exception ex)
            {
                for (int i = 0; i < selectedFileList.Count; i++)
                {
                    selectedFileList[i].fileError = ex.ToString();
                }
            }
        }
        public void OnMethodListChanged()
        {
            try
            {
                for (int i = 0; i < selectedFileList.Count; i++)
                {
                    foreach (var action in methodList)
                    {
                        if (action.IsChecked == true)
                        {
                            if (action.isApplyToName == true)
                            {
                                int collisionCount = 0;
                                selectedFileList[i].realName = action.Process(selectedFileList[i].realName);
                                selectedFileList[i].newName = selectedFileList[i].realName + selectedFileList[i].originalExtension;
                                for (int j = 0; j < i; j++)
                                {
                                    if(string.Compare(selectedFileList[j].newName, selectedFileList[i].newName) == 0)
                                    {
                                        collisionCount++;
                                    }
                                }
                                if (collisionCount > 0)
                                {
                                    selectedFileList[i].newName = String.Format($"{selectedFileList[i].realName} ({collisionCount.ToString()}){ selectedFileList[i].newExt} ");
                                }
                            }
                            else if(action.isApplyToName == false)
                            {
                                selectedFileList[i].newExt = action.Process(selectedFileList[i].newExt);
                                selectedFileList[i].newName = selectedFileList[i].newName.Replace(selectedFileList[i].originalExtension, selectedFileList[i].newExt);
                            }

                        }
                    }
                    selectedFileList[i].UpdatePreview();
                }
                for (int i = 0; i < selectedFolderList.Count; i++)
                {
                    foreach (var action in methodList)
                    {
                        if (action.IsChecked == true)
                        {
                                selectedFolderList[i].newName = action.Process(selectedFolderList[i].newName);
                        }
                    }
                    selectedFolderList[i].UpdatePreview();
                }
            }
            catch (Exception ex)
            {
                for (int i = 0; i < selectedFileList.Count; i++)
                {
                    selectedFileList[i].fileError = ex.ToString();
                }
            }
        }

        private void Starting_Batch(object sender, RoutedEventArgs e)
        {
            if (!selectedFileList.Any() && !selectedFolderList.Any())
            {
                MessageBox.Show("Please add a file or folder!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!methodList.Any())
            {
                MessageBox.Show("Please add a method!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (selectedFileList.Any())
            {
                foreach (var item in selectedFileList)
                {
                    var originalName = item.fileName;
                    item.fileName = item.newName;
                    File.Move(item.filePath, item.filePath.Replace(originalName, item.newName));
                }
                selectedFileList.Clear();
            }
            if (selectedFolderList.Any())
            {
                foreach (var item in selectedFolderList)
                {
                    item.folderName = item.newName;
                    Directory.Move(item.folderPath + item, item.folderPath);
                }
                selectedFolderList.Clear();
                return;
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
            OnMethodListChanged();
        }

        //ARROW BUTTON HANDLER
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

        //SAVE AND LOAD PRESET
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        FileInfo presetsPathFile = new FileInfo("./PresetsFile.txt");
        BindingList<FileInformation> savedPresetFiles = new BindingList<FileInformation>();

        private void SavePresetButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Preset file (*.bin)| *.bin";
            saveFileDialog.DefaultExt = "*.bin";
            saveFileDialog.OverwritePrompt = true;

            //TODO: get preset name/location from user (save to saveLoc)
            string saveLoc = "./preset.bin";

            if (saveFileDialog.ShowDialog() == true)
            {
                saveLoc = saveFileDialog.FileName;

                //Write methodList to binary file
                WriteToBinaryFile<BindingList<IMethodAction>>(saveLoc, methodList, false);

                //Add file name into List
                FileInfo PresetFile = new FileInfo(saveLoc);
                bool isFileExist = false;

                if (savedPresetFiles.Count() == 0)
                {
                    savedPresetFiles.Add(new FileInformation() { fileName = PresetFile.Name, filePath = saveLoc, originalExtension = PresetFile.Extension });

                    //Write preset file path to .txt
                    using (StreamWriter sw = presetsPathFile.AppendText())
                    {
                        sw.WriteLine(saveLoc);
                    }
                }
                else
                {
                    foreach (var item in savedPresetFiles)
                    {
                        if (item.fileName.Equals(PresetFile.Name))
                        {
                            isFileExist = true;
                            break;
                        }
                    }

                    if (!isFileExist)
                    {
                        savedPresetFiles.Add(new FileInformation() { fileName = PresetFile.Name, filePath = saveLoc, originalExtension = PresetFile.Extension });

                        //Write preset file path to .txt
                        using (StreamWriter sw = presetsPathFile.AppendText())
                        {
                            sw.WriteLine(saveLoc);
                        }
                    }
                }
            }
        }

        private void PresetCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Load File
            FileInformation loadedPresetFilePath = (FileInformation)PresetCombobox.SelectedItem;
            BindingList<IMethodAction> tempMethodList = new BindingList<IMethodAction>();
            tempMethodList = ReadFromBinaryFile<BindingList<IMethodAction>>(loadedPresetFilePath.filePath);

            methodList.Clear();

            foreach (var item in tempMethodList)
            {
                methodList.Add(item);
            }
            OnMethodListChanged();
        }
        List<string> ApplyToList = new List<string> { "Name", "Extension" };
        private void ApplyToComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (!cmb.HasItems)
            {
                return;
            }
            string applyOption = cmb.SelectedValue.ToString();
            var action = MethodListView.SelectedItem as IMethodAction;
            if (applyOption == "Extension")
            {
                action.isApplyToName = false;
            }
            else if(applyOption == "Name")
            {
                action.isApplyToName = true;
            }
            foreach (var item in selectedFileList)
            {
                item.newName = item.fileName;
                item.newExt = item.originalExtension;
                item.realName = item.newName.Trim().Replace(item.newExt.Length == 0 ? " " : item.originalExtension, "");
            }
            foreach(var item in selectedFolderList)
            {
                item.newName = item.folderName;
            }
            OnMethodListChanged();
        }
        private void ClearFile_Click(object sender, RoutedEventArgs e)
        {
            selectedFileList.Clear();
            OnFileListChange();
        }

        private void ClearFolder_Click(object sender, RoutedEventArgs e)
        {
            selectedFolderList.Clear();
            OnFolderListChange();
        }
    }
}
