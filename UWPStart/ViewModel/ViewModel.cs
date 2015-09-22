using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPStart.Model;
using System.Collections.ObjectModel;

namespace UWPStart.ViewModels
{
    public class ViewModel
    {
        public ObservableCollection<Engineer> Engineers { get; set; }

        public void GetEngineers()
        {
            ObservableCollection<Engineer> list = new ObservableCollection<Engineer>();
            list.Add(new Engineer {Name="one",UnSolved=10,Solved=10,FollowUp=10,Escalate=10 });
            list.Add(new Engineer { Name = "two", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 });
            list.Add(new Engineer { Name = "three", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 });
            list.Add(new Engineer { Name = "four", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 });
            list.Add(new Engineer { Name = "five", UnSolved = 10, Solved = 10, FollowUp = 10, Escalate = 10 });
            Engineers = list;
        }
    }
}
