﻿using System;
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
using System.Diagnostics;
namespace WindowsProgramming
{
    /// <summary>
    /// Interaction logic for DetailUpdateWindow.xaml
    /// </summary>
    public partial class DetailUpdateWindow : Window
    {
        public DetailUpdateWindow(IMethodArgs Args)
        {
            InitializeComponent();
           
            contentCtrl.DataContext = Args;
            UpdateWindow.Title = Args.methodType;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            
            DialogResult = true;
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (sender as ComboBox)?.SelectedItem;
            var arg = ((sender as ComboBox)?.DataContext as NewCaseArgs);
            arg.selectedStyle = selected as string;
        }
    }
}

