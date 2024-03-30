using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RusEngDictionary
{
   public class DictionaryER : INotifyPropertyChanged
    {
        private string word;
        private int id;
        private string translation;
        private string definition;
        public int Id
        {
            get => id;
            set
            {
               id = value;
                NotifyPropertyChanged();
            }
        }
        public string Word 
        {
            get => word;
            set
            {
                word = value;
                NotifyPropertyChanged();
            }
        }
        public string Translation
        {
            get => translation;
            set
            {
                translation = value;
                NotifyPropertyChanged();
            }
        }
        public string Definition
        {
            get => definition;
            set
            {
                definition = value;
                NotifyPropertyChanged();
            }
        }



        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
