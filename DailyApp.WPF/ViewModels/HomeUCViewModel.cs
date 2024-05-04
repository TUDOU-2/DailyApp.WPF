using DailyApp.WPF.DTOs;
using DailyApp.WPF.HttpClients;
using DailyApp.WPF.Models;
using DailyApp.WPF.Service;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.ViewModels
{
    class HomeUCViewModel : BindableBase, INavigationAware
    {
        private List<StatPanelInfo> _StatPanelList; // 统计面板列表
        private List<ToDoInfoDTO> _ToDoList; // 待办事项列表
        private List<MemoInfoDTO> _MemoList; // 备忘录列表
        private string _LoginInfo; // 用户登录信息
        private StatWaitDTO StatWaitDTO { get; set; } = new StatWaitDTO(); // 待办事项统计数据模型
        private readonly HttpRestClient _HttpClient; // 请求api的客户端
        private readonly DialogHostService dialogHostService; // 对话框服务

        public DelegateCommand ShowAddWaitDialogCmm { get; set; } // 添加待办事项命令


        public string LoginInfo
        {
            get { return _LoginInfo; }
            set { _LoginInfo = value; RaisePropertyChanged(); }
        }

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

        public HomeUCViewModel(HttpRestClient HttpClient, DialogHostService DialogHostService) // 构造函数
        {
            CreateStatPanelList();
            CreateToDoList();
            CreateMemoList();
            _HttpClient = HttpClient;
            dialogHostService = DialogHostService;
            ShowAddWaitDialogCmm = new DelegateCommand(ShowAddWaitDialog);           
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


        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("LoginName"))
            {
                DateTime now = DateTime.Now;
                string[] week = new string[7] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                string loginName = navigationContext.Parameters.GetValue<string>("LoginName");
                LoginInfo = $"{loginName}，你好！今天是{now.ToString("yyyy年MM月dd日")} {week[(int)now.DayOfWeek]}";
                CallStatWait();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) // 是否能导航
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        /// <summary>
        /// 调用api获取待办事项统计数据
        /// </summary>
        private void CallStatWait()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Route = "ToDo/StatWait";

            ApiResponse response = _HttpClient.Execute(apiRequest);
            if (response.ResultCode == 1)
            {
                StatWaitDTO = JsonConvert.DeserializeObject<StatWaitDTO>(response.ResultData.ToString());
                RefreshWaitStat();
            }
        }

        /// <summary>
        /// 更新待办事项统计面板
        /// </summary>
        private void RefreshWaitStat()
        {
            StatPanelList[0].Result = StatWaitDTO.TotalCount.ToString();
            StatPanelList[1].Result = StatWaitDTO.FinishedCount.ToString();
            StatPanelList[2].Result = StatWaitDTO.FinishPercent;
        }

        /// <summary>
        /// 显示添加待办事项对话框
        /// </summary>
        private async void ShowAddWaitDialog()
        {
           var result = await dialogHostService.ShowDialog("AddWaitUC", null);
            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.ContainsKey("AddToDoInfo"))
                {
                    var addModel = result.Parameters.GetValue<ToDoInfoDTO>("AddToDoInfo"); // 接收数据

                }
            }
        }
    }
}
