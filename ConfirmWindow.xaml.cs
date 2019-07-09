using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static WindowProjects.MainWindow;

namespace WindowsProgramming
{
    /// <summary>
    /// Interaction logic for ConfirmWindow.xaml
    /// </summary>
    public partial class ConfirmWindow : Window
    {
        public ConfirmWindow(BindingList<FileInformation> fileList, BindingList<FolderInformation> folderList)
        {
            InitializeComponent();
            FileListBinding.ItemsSource = fileList;
            FolderListBinding.ItemsSource = folderList;
        }

        private void StartButton_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
