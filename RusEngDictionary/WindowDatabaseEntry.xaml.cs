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
    /// Логика взаимодействия для WindowDatabaseEntry.xaml
    /// </summary>
    public partial class WindowDatabaseEntry : Window
    {
        public WindowDatabaseEntry()
        {
            InitializeComponent();
            DataContext = new DicionaryViewModel();
        }
    }
}
