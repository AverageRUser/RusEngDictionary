﻿using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RusEngDictionary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
     
       
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new DicionaryViewModel();
           // TabDict.Items.Add();
    
    
        }

        private void dList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var listBox = sender as ListBox;
            listBox.ScrollIntoView(listBox.SelectedItem);
        }
      
    }
}
