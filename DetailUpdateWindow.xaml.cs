using System;
using System.Collections.Generic;
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
using WindowsProgramming;

namespace WindowsProgamming
{
    /// <summary>
    /// Interaction logic for DetailUpdateWindow.xaml
    /// </summary>
    public partial class DetailUpdateWindow : Window
    {
        public DetailUpdateWindow(IMethodArgs replaceArgs)
        {
            DataContext = replaceArgs;
            InitializeComponent();
            
        }

        //public DetailUpdateWindow(RemovePatternArgs removePatternArgs)
        //{
        //    DataContext = removePatternArgs;
        //    InitializeComponent();
            
        //}

        //public DetailUpdateWindow(TrimArgs trimArgs)
        //{
        //    DataContext = trimArgs;
        //    InitializeComponent();
        //}

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}

