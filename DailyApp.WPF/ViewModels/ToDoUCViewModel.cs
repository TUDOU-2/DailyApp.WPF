using DailyApp.WPF.DTOs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.ViewModels
{
    class ToDoUCViewModel : BindableBase
    {
        private List<ToDoInfoDTO> _ToDoList; // 待办事项列表
        private bool _IsRightDrawerOpen; // 右侧抽屉是否打开
        public DelegateCommand AddCommand { get; private set; }

        public bool IsRightDrawerOpen
        {
            get { return _IsRightDrawerOpen; }
            set { _IsRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        public List<ToDoInfoDTO> ToDoList
        {
            get { return _ToDoList; }
            set { _ToDoList = value; RaisePropertyChanged(); }
        }
        public ToDoUCViewModel() // 构造函数
        {           
           CreateToDoList();
            AddCommand = new DelegateCommand(Add);
        }
        private void CreateToDoList()
        {
            _ToDoList = new List<ToDoInfoDTO>();
            for (int i = 0; i < 10; i++)
            {
                _ToDoList.Add(new ToDoInfoDTO() { Title = "待办" + i, Content = "正在处理中...." });
            }
        }

        private void Add()
        {
            IsRightDrawerOpen = true;
        }
    }
}
