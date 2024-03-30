using Microsoft.VisualBasic;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RusEngDictionary
{
    public class DicionaryViewModel : INotifyPropertyChanged
    {
        static string connStr = "server=localhost;user=root;database=dictionaryEngRus;password=yourpassword;";
        CollectionView view;
        MySqlConnection conn = new MySqlConnection(connStr);
        public ObservableCollection<DictionaryER> items { get; set; }
        private RelayCommand addCommand;     
        private RelayCommand searchCommand;
        string _pattern;
        //Метод для получения и установки _pattern
        public string Pattern
        {
            get => _pattern;
            set
            {
                
                Set(ref _pattern, value);
               
                Selected = items.FirstOrDefault(s =>   s.Word.StartsWith(Pattern)    );
            }
        }

        DictionaryER _selected;
        public DictionaryER Selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }
        //Функция
        public void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            field = value;

           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RelayCommand AddCommand
        {
            get
            {
             
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      int i = 3;
                      string sql = "SELECT * FROM dictionaryER";
                      // объект для выполнения SQL-запроса
                      MySqlCommand command1 = new MySqlCommand(sql, conn);
                      // объект для чтения ответа сервера
                      MySqlDataReader reader = command1.ExecuteReader();
                      // читаем результат
                      while (reader.Read())
                      {
                          // элементы массива [] - это значения столбцов из запроса SELECT
                          items.Add(new DictionaryER() { Id=i , Word = reader[1].ToString(), Translation = reader[2].ToString(), Definition = reader[3].ToString() });
                          i++;
                          //(reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString());
                      }

                      reader.Close(); // закрываем reader
                 
                  }));
            }
       
        }
        /*
        public RelayCommand Search
        {            get
            {
                return searchCommand ??
                    (searchCommand = new RelayCommand(obj =>
                    {
                 foreach(var i in items)
                        {
                           if(i.Word == "")
                            {
                              //  view.;
                            }
                        }
                       
                        
                    }));
            }
        }
        */
        public DicionaryViewModel()
        {
            conn.Open();
          
            items = new ObservableCollection<DictionaryER>
            {
                new DictionaryER() { Word = "Ball", Translation = "Мяч" ,Id = 0},
                new DictionaryER() { Word = "Candy", Translation = "Конфета", Id = 1 },
                new DictionaryER() { Word = "Album", Translation = "Альбом", Id =2 }

            };
            view = (CollectionView)CollectionViewSource.GetDefaultView(items);
            view.SortDescriptions.Add(new SortDescription("Word", ListSortDirection.Ascending));
            //  items = new ObservableCollection<DictionaryER>(items.OrderBy(i => i.Word));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
    
    
}
