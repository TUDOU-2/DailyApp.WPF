using DailyApp.WPF.DTOs;
using DailyApp.WPF.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.ViewModels
{
    class HomeUCViewModel : BindableBase
    {
		private List<StatPanelInfo> _StatPanelList; // 统计面板列表
        private List<ToDoInfoDTO> _ToDoList; // 待办事项列表
        private List<MemoInfoDTO> _MemoList; // 备忘录列表

        public List<MemoInfoDTO> MemoList 
        {
            get { return _MemoList; }
            set { _MemoList = value; RaisePropertyChanged(); }
        }
        public List<ToDoInfoDTO> ToDoList
        {
            get { return _ToDoList; }
            set { _ToDoList = value; RaisePropertyChanged(); }
        }

        public List<StatPanelInfo> StatPanelList
        {
            get { return _StatPanelList; }
            set { _StatPanelList = value; RaisePropertyChanged(); }
        }
        public HomeUCViewModel() // 构造函数
        {
            CreateStatPanelList();
            CreateToDoList();
            CreateMemoList();
        }

        private void CreateStatPanelList()
		{
            StatPanelList = new List<StatPanelInfo>();
            StatPanelList.Add(new StatPanelInfo() { Icon = "ClockFast", ItemName = "汇总", Result = "9", BackColor = "#FF0CA0FF", ViewName = "ToDoUC" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "ClockCheckOutline", ItemName = "已完成", Result = "9", BackColor = "#FF1ECA3A", ViewName = "ToDoUC" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "ChartLineVariant", ItemName = "完成比例", Result = "100%", BackColor = "#FF02C6DC", ViewName = "" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "PlaylistStar", ItemName = "备忘录", Result = "19", BackColor = "#FFFFA000", ViewName = "MemoUC" });
        }

        private void CreateToDoList()
        {
            _ToDoList = new List<ToDoInfoDTO>();
            for (int i = 0; i < 10; i++)
            {
                _ToDoList.Add(new ToDoInfoDTO() { Title = "待办" + i, Content = "正在处理中...." });
            }
        }

        private void CreateMemoList()
        {
            _MemoList = new List<MemoInfoDTO>();
            for (int i = 0; i < 10; i++)
            {
                _MemoList.Add(new MemoInfoDTO() { Title = "备忘" + i, Content = "我的密码" });
            }
        }
    }
}
