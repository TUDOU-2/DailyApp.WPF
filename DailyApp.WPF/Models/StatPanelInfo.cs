using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.Models
{
    class StatPanelInfo : BindableBase
    {
        public string Icon { get; set; }
        public string ItemName { get; set; }
        public string _Result { get; set; }

        public string Result
        {
            get { return _Result; }
            set { _Result = value; RaisePropertyChanged(); }
        }

        public string BackColor { get; set; }
        public string ViewName { get; set; }
    }
}
