﻿using Microsoft.VisualBasic;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RusEngDictionary
{
    public class DicionaryViewModel : INotifyPropertyChanged
    {
        static string connStr = null;
        CollectionView view;
       public MySqlConnection conn = new MySqlConnection(connStr);
        public ObservableCollection<DictionaryER> DBitems { get; set; } = new ObservableCollection<DictionaryER>();
        public ObservableCollection<DictionaryER> items { get; set; } 
        public ObservableCollection<DictionaryER> favorite { get; set; } = new ObservableCollection<DictionaryER>();
        private RelayCommand addCommand;
        private RelayCommand removeCommand;
        private RelayCommand dictconnCommand;
        private RelayCommand favorCommand;
        private RelayCommand unfavorCommand;
        string _pattern;
        private bool isConnected;
        private bool isVisibleDBtable;
        private string _dbTabHeader;
        Brush _color;
        string databaseTableName;
        public string DBTabHeader
        {
            get => _dbTabHeader;
            set
            {
                _dbTabHeader = value;
                OnPropertyChanged("DBTabHeader");

            }
        }
        public bool visibleDB
        {
            get => isVisibleDBtable;
            set
            {
                isVisibleDBtable = value;
                OnPropertyChanged("visibleDB");

            }
        }
        public bool ColorConn
        {
            get => isConnected;
            set
            {
                isConnected = value;
                OnPropertyChanged("ColorConn");

            }
        }
  
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
                      favorite.Add(items[_selected.Id]);

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
                      favorite.Remove(_selected);

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
                              DBitems.Add(DictionaryObj);
                          }
                          else
                          {
                              DictionaryER DictionaryObj = wde.DictionaryObj;
                              items.Add(new DictionaryER {Id= items.Count, Word=DictionaryObj.Word, Translation = DictionaryObj.Translation, Definition = DictionaryObj.Definition });
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
                              
                              string query = $"DELETE FROM {databaseTableName} WHERE Word = '{_selected.Word}'";
                              MySqlCommand command = new MySqlCommand(query, conn);
                              command.ExecuteNonQuery();
                              DBitems.RemoveAt(_selected.Id);
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
                          try
                          {
                              //  DictionaryER DictionaryObj = wdbe.DictionaryObj;
                              connStr = $"server={wdbe.server.Text};user=root;database={wdbe.databaseName.Text};password={wdbe.password.Text};";
                              conn = new MySqlConnection(connStr);
                              conn.Open();
                              databaseTableName = wdbe.nameTable.Text;


                              string sql = $"SELECT * FROM {databaseTableName}";
                              // объект для выполнения SQL-запроса
                              MySqlCommand command1 = new MySqlCommand(sql, conn);
                              // объект для чтения ответа сервера
                              MySqlDataReader reader = command1.ExecuteReader();
                              
                              // читаем результат
                              while (reader.Read())
                              {
                                  // элементы массива [] - это значения столбцов из запроса SELECT
                                  DBitems.Add(new DictionaryER() { IsFavorite = false, Word = reader[1].ToString(), Translation = reader[2].ToString(), Definition = reader[3].ToString(), Id = items.Count });

                                  //(reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString());
                              }

                              reader.Close(); // закрываем reader

                              MessageBox.Show("Данные успешно добавленны в словарь");
                              ColorConn = true;
                              DBTabHeader = databaseTableName;
                              visibleDB = true;
                          }
                          catch(Exception ex) 
                          {
                              MessageBox.Show("Не удалось подключиться к базе данных","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                          }
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
