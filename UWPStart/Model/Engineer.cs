using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace UWPStart.Model
{
    public class Engineer : INotifyPropertyChanged
    {
        public string ID { get; set; }
        public string Name { get; set; }
        private string _forum;
        public string Forum
        {
            get { return _forum; }
            set { _forum = value; RaisePropertyChanged("Forum"); }
        }

        private int _solved;
        private int _unsolved;
        private int _followUp;
        private int _escalate;
        public int Solved
        {
            get { return _solved; }
            set { _solved = value; RaisePropertyChanged("Solved"); }
        }
        public int UnSolved
        {
            get { return _unsolved; }
            set { _unsolved = value; RaisePropertyChanged("UnSolved"); }
        }
        public int FollowUp
        {
            get { return _followUp; }
            set { _followUp = value; RaisePropertyChanged("FollowUp"); }
        }
        public int Escalate
        {
            get { return _escalate; }
            set { _escalate = value; RaisePropertyChanged("Escalate"); }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string PropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
    }
}
