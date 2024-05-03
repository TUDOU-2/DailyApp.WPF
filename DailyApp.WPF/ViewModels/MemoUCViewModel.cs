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
    class MemoUCViewModel : BindableBase
    {
        private List<MemoInfoDTO> _MemoList; // 备忘录列表
        private bool _IsRightDrawerOpen; // 右侧抽屉是否打开
        public DelegateCommand AddCommand { get; private set; }

        public bool IsRightDrawerOpen
        {
            get { return _IsRightDrawerOpen; }
            set { _IsRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        public List<MemoInfoDTO> MemoList
        {
            get { return _MemoList; }
            set { _MemoList = value; RaisePropertyChanged(); }
        }
        public MemoUCViewModel() // 构造函数
        {
            CreateMemoList();
            AddCommand = new DelegateCommand(Add);
        }
        private void CreateMemoList()
        {
            _MemoList = new List<MemoInfoDTO>();
            for (int i = 0; i < 10; i++)
            {
                _MemoList.Add(new MemoInfoDTO() { Title = "备忘录" + i, Content = "正在处理中...." });
            }
        }

        private void Add()
        {
            IsRightDrawerOpen = true;
        }
    }
}
