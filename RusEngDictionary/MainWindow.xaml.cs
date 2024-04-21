using MySqlConnector;
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
            
    
    
        }

        private void dList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var listBox = sender as ListBox;
            listBox.ScrollIntoView(listBox.SelectedItem);
        }

        private void favoriteList_Click(object sender, RoutedEventArgs e)
        {
            fList.Visibility = Visibility.Visible;
            favoriteList.Visibility = Visibility.Collapsed;
            CloseFavorList.Visibility = Visibility.Visible;
        }

        private void CloseFavorList_Click(object sender, RoutedEventArgs e)
        {
            fList.Visibility = Visibility.Collapsed;
            favoriteList.Visibility = Visibility.Visible;
            CloseFavorList.Visibility = Visibility.Collapsed;
        }
    }
}
