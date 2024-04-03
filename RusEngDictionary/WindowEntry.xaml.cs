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

namespace RusEngDictionary
{
    /// <summary>
    /// Логика взаимодействия для WindowEntry.xaml
    /// </summary>
    public partial class WindowEntry : Window
    {
        public DictionaryER DictionaryObj { get; private set; }
        public WindowEntry(DictionaryER dictionaryObj)
        {
            InitializeComponent();
            DictionaryObj = dictionaryObj;
            DataContext = DictionaryObj;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
