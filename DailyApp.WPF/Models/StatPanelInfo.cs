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
        public string Icon { get; set; } // 统计面板图标
        public string ItemName { get; set; } // 统计项名称
        public string _Result { get; set; } // 统计结果

        public string Result
        {
            get { return _Result; }
            set { _Result = value; RaisePropertyChanged(); }
        }

        public string BackColor { get; set; } // 背景颜色
        public string ViewName { get; set; } // 点击跳转的视图名称

        // 鼠标悬浮提示
        public string Hand
        {
            get
            {
                if(ItemName == "完成比例")
                {
                    return "";
                }
                else
                {
                    return "Hand";
                }
            }
        }
    }
}
