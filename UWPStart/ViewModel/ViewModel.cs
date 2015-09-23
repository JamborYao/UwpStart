using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPStart.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Windows.UI.Popups;

namespace UWPStart.ViewModels
{
    public class ViewModel
    {
        public MyCommand<object> AddCommand { get; set; }
        public MyCommand<object> GetCommand { get; set; }

        public ObservableCollection<Engineer> Engineers { get; set; }
        public ViewModel()
        {
            AddCommand = new MyCommand<object>(AddCommandClick);
            GetCommand = new MyCommand<object>(GetCommandClick);
            //AddCommand = new DelegateCommand<Engineer>(AddMethod);
            GetEngineers();
        }

        private async void GetCommandClick(object obj)
        {
            IEnumerable<Engineer> readTask = await App.MobileService.GetTable<Engineer>().ReadAsync();
            ObservableCollection<Engineer> engineer = new ObservableCollection<Engineer>(readTask);
            Engineers.Add(engineer[0]);
            Engineers.Add(new Engineer { Name = "karen", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 });
        }



        public void GetEngineers()
        {
            ObservableCollection<Engineer> list = new ObservableCollection<Engineer>();
            list.Add(new Engineer { Name = "one", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 });
            list.Add(new Engineer { Name = "two", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 });

            Engineers = list;
        }



        async void AddCommandClick(object sender)
        {
            if (Engineers != null)
            {
                Engineers.Add(new Engineer { Name = "jambor", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 });
            }
            Item item = new Item { Text = "Awesome item" };
            Engineer insertEntity = new Engineer { ID = "1", Forum = "UWP", Name = "jambor", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 };
            await App.MobileService.GetTable<Engineer>().InsertAsync(insertEntity);
            var dialog = new MessageDialog("add successful");
            await dialog.ShowAsync();
        }
        


    }
    public class MyCommand<T> : ICommand
    {
        readonly Action<T> callback;

        public MyCommand(Action<T> callback)
        {
            this.callback = callback;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (callback != null) { callback((T)parameter); }
        }
    }




}
