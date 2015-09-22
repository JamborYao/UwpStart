using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPStart.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;


namespace UWPStart.ViewModels
{
    public class ViewModel
    {
        public ViewModel()
        {
             AddCommand =new MyCommand<object>(AddCommandClick);
            //AddCommand = new DelegateCommand<Engineer>(AddMethod);

        }
        public ObservableCollection<Engineer> Engineers { get; set; }

        public void GetEngineers()
        {
            ObservableCollection<Engineer> list = new ObservableCollection<Engineer>();
            list.Add(new Engineer {Name="one",UnSolved=10,Solved=10,FollowUp=10,Escalate=10 });
            list.Add(new Engineer { Name = "two", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 });
         
            Engineers = list;
        }
        public MyCommand<object> AddCommand { get; set; }

     

        void AddCommandClick(object sender)
        {
            Engineers.Add(new Engineer { Name = "jambor", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 });
        }
        private void AddMethod(Engineer employee)
        {

           // EmpList.Add(new Employee { Age = 24, EmpName = "White" });

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
