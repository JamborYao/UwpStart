using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPStart.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace UWPStart.ViewModels
{
    public class ViewModel
    {
        public ViewModel()
        {
            AddCommand =new RelayComand(AddCommandClick);
        }
        public ObservableCollection<Engineer> Engineers { get; set; }

        public void GetEngineers()
        {
            ObservableCollection<Engineer> list = new ObservableCollection<Engineer>();
            list.Add(new Engineer {Name="one",UnSolved=10,Solved=10,FollowUp=10,Escalate=10 });
            list.Add(new Engineer { Name = "two", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 });
         
            Engineers = list;
        }
        public RelayComand AddCommand { get; set; }

        public void AddCommandClick(Object sender)
        {
            Engineers.Add(new Engineer { Name = "jambor", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 });
        }
    }
    public class RelayComand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action<Object> _action;

        public RelayComand(Action<Object> action)
        {
            if (action != null)
            {
                _action = action;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
