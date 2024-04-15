using Microsoft.VisualBasic;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RusEngDictionary
{
    public class DicionaryViewModel : INotifyPropertyChanged
    {
        static string connStr = null;
        CollectionView view;
        MySqlConnection conn = new MySqlConnection(connStr);
 
        public ObservableCollection<DictionaryER> items { get; set; }
     
        private RelayCommand addCommand;
        private RelayCommand removeCommand;
        private RelayCommand dictconnCommand;
        private RelayCommand favorCommand;
        private RelayCommand unfavorCommand;
        string _pattern;
        string databaseTableName;
   
        //Свойство для получения и установки _pattern
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
        
        public void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            field = value;

           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RelayCommand FavorCommand
        {
            get
            {

                return favorCommand ??
                  (favorCommand = new RelayCommand(obj =>
                  {
                      items[_selected.Id].IsFavorite = true;
                     

                  }));
            }

        }
        public RelayCommand UnFavorCommand
        {
            get
            {

                return unfavorCommand ??
                  (unfavorCommand = new RelayCommand(obj =>
                  {
                      items[_selected.Id].IsFavorite = false;
                      


                  }));
            }

        }
        public RelayCommand AddCommand
        {
            get
            {
             
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {

                      WindowEntry wde = new WindowEntry(new DictionaryER());
                      
                      if (wde.ShowDialog() == true)
                      {
                          if (conn.Ping() == true && connStr != null)
                          {
                              DictionaryER DictionaryObj = wde.DictionaryObj;

                              string query = $"INSERT {databaseTableName} (Word,Translation,Definitions) VALUES ('{DictionaryObj.Word}', '{DictionaryObj.Translation}','{DictionaryObj.Definition}')";
                              MySqlCommand command = new MySqlCommand(query, conn);
                              command.ExecuteNonQuery();
                              items.Add(DictionaryObj);
                          }
                          else
                          {
                              DictionaryER DictionaryObj = wde.DictionaryObj;
                              items.Add(DictionaryObj);
                          }

                      }

                  }));
            }
       
        }
        public RelayCommand RemoveCommand
        {
            get
            {

                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                
                      if (items.Count != 0)
                      {
                          if (conn.State == ConnectionState.Open && connStr != null)
                          {
                              string query = $"DELETE FROM dictionaryER WHERE Word = '{_selected.Word}'";
                              MySqlCommand command = new MySqlCommand(query, conn);
                              command.ExecuteNonQuery();
                              items.RemoveAt(_selected.Id);
                          }
                          else
                          {
                              items.RemoveAt(_selected.Id);
                          }
                      }
                      else
                      {
                          MessageBox.Show("Нет элементов в списке");
                      }


                  }));
                
            }

        }
        public RelayCommand DictConnCommand
        {
            get
            {

                return dictconnCommand ??
                  (dictconnCommand = new RelayCommand(obj =>
                  {

                      WindowDBentry wdbe = new WindowDBentry();
                      if (wdbe.ShowDialog() == true)
                      {
                          //  DictionaryER DictionaryObj = wdbe.DictionaryObj;
                          connStr = $"server=localhost;user=root;database={wdbe.databaseName.Text};password={wdbe.password.Text};";
                          conn = new MySqlConnection(connStr);
                          conn.Open();
                          databaseTableName = wdbe.nameTable.Text;
                          int i = 3;

                          string sql = $"SELECT * FROM {databaseTableName}";
                          // объект для выполнения SQL-запроса
                          MySqlCommand command1 = new MySqlCommand(sql, conn);
                          // объект для чтения ответа сервера
                          MySqlDataReader reader = command1.ExecuteReader();
                          // читаем результат
                          while (reader.Read())
                          {
                              // элементы массива [] - это значения столбцов из запроса SELECT
                              items.Add(new DictionaryER() { IsFavorite = false, Word = reader[1].ToString(), Translation = reader[2].ToString(), Definition = reader[3].ToString() });
                              i++;
                              //(reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString());
                          }

                          reader.Close(); // закрываем reader
                      }

                  }));

            }

        }
        public DicionaryViewModel()
        {
     
            items = new ObservableCollection<DictionaryER>
            {
                new DictionaryER() { Word = "Ball", Translation = "Мяч" ,Id = 0},
                new DictionaryER() { Word = "Candy", Translation = "Конфета", Id = 1 },
                new DictionaryER() { Word = "Album", Translation = "Альбом", Id =2 }

            };
          
           

            view = (CollectionView)CollectionViewSource.GetDefaultView(items);
            view.SortDescriptions.Add(new SortDescription("IsFavorite", ListSortDirection.Descending));
            view.SortDescriptions.Add(new SortDescription("Word", ListSortDirection.Ascending));
       
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
    
    
}
