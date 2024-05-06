using DailyApp.WPF.DTOs;
using DailyApp.WPF.Extensions;
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
using System.Windows;

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
        private readonly IRegionManager _regionManager; // 区域管理器

        public DelegateCommand ShowAddWaitDialogCmm { get; set; } // 添加待办事项命令
        public DelegateCommand ShowAddMemoDialogCmm { get; set; } // 添加备忘录命令
        public DelegateCommand<ToDoInfoDTO> ChangedToDoStatusCmm { get; set; } // 改变待办事项状态命令
        public DelegateCommand<ToDoInfoDTO> ShowEditWaitDialogCmm { get; set; } // 显示编辑待办事项对话框命令
        public DelegateCommand<MemoInfoDTO> ShowEditMemoDialogCmm { get; set; } // 显示编辑备忘录对话框命令
        public DelegateCommand<StatPanelInfo> NavigateCmm { get; set; } // 导航命令

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

        public HomeUCViewModel(HttpRestClient HttpClient, DialogHostService DialogHostService, IRegionManager regionManager) // 构造函数
        {
            CreateStatPanelList();
            _HttpClient = HttpClient;
            CreateToDoList();
            CreateMemoList();
            CallStatMemo();
            dialogHostService = DialogHostService;
            _regionManager = regionManager;
            ShowAddWaitDialogCmm = new DelegateCommand(ShowAddWaitDialog);
            ShowAddMemoDialogCmm = new DelegateCommand(ShowAddMemoDialog);
            ChangedToDoStatusCmm = new DelegateCommand<ToDoInfoDTO>(ChangedToDoStatus);
            ShowEditWaitDialogCmm = new DelegateCommand<ToDoInfoDTO>(ShowEditWaitDialog);
            ShowEditMemoDialogCmm = new DelegateCommand<MemoInfoDTO>(ShowEditMemoDialog);
            NavigateCmm = new DelegateCommand<StatPanelInfo>(Navigate);
        }

        /// <summary>
        /// 统计面板导航
        /// </summary>
        /// <param name="info"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Navigate(StatPanelInfo info)
        {
            if (!string.IsNullOrEmpty(info.ViewName))
            {
                if (info.ItemName == "已完成")
                {
                    NavigationParameters paras = new NavigationParameters();
                    paras.Add("SelectedIndex", 2);
                    _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(info.ViewName, paras);
                }
                else
                {
                    _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(info.ViewName);
                }
            }
        }

        private void CreateStatPanelList()
        {
            StatPanelList = new List<StatPanelInfo>();
            StatPanelList.Add(new StatPanelInfo() { Icon = "ClockFast", ItemName = "汇总", Result = "9", BackColor = "#FF0CA0FF", ViewName = "ToDoUC" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "ClockCheckOutline", ItemName = "已完成", Result = "9", BackColor = "#FF1ECA3A", ViewName = "ToDoUC" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "ChartLineVariant", ItemName = "完成比例", Result = "100%", BackColor = "#FF02C6DC", ViewName = "" });
            StatPanelList.Add(new StatPanelInfo() { Icon = "PlaylistStar", ItemName = "备忘录", Result = "19", BackColor = "#FFFFA000", ViewName = "MemoUC" });
        }

        /// <summary>
        /// 获取待办状态的待办事项
        /// </summary>
        private void CreateToDoList()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Route = "ToDo/GetToDoList";

            ApiResponse response = _HttpClient.Execute(apiRequest);
            if (response.ResultCode == 1)
            {
                ToDoList = JsonConvert.DeserializeObject<List<ToDoInfoDTO>>(response.ResultData.ToString());
            }
            else
            {
                ToDoList = new List<ToDoInfoDTO>();
            }
        }

        /// <summary>
        /// 获取备忘录列表
        /// </summary>
        private void CreateMemoList()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Route = "Memo/QueryMemo";

            ApiResponse response = _HttpClient.Execute(apiRequest);
            if (response.ResultCode == 1)
            {
                MemoList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(response.ResultData.ToString());
            }
            else
            {
                MemoList = new List<MemoInfoDTO>();
            }
        }

        /// <summary>
        /// 导航到此页面时触发
        /// </summary>
        /// <param name="navigationContext">导航过程中的信息</param>
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
        /// 获取待办事项统计数据
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
                    ApiRequest apiRequest = new ApiRequest();
                    apiRequest.Method = RestSharp.Method.POST;
                    apiRequest.Parameters = addModel;
                    apiRequest.Route = "ToDo/AddWait";
                    ApiResponse response = _HttpClient.Execute(apiRequest);
                    if (response.ResultCode == 1)
                    {
                        CallStatWait();
                        CreateToDoList();
                    }
                    else
                    {
                        MessageBox.Show(response.Msg);
                    }
                }
            }
        }

        /// <summary>
        /// 编辑待办事项对话框
        /// </summary>
        private async void ShowEditWaitDialog(ToDoInfoDTO toDoInfoDTO)
        {
            DialogParameters paras = new DialogParameters();
            paras.Add("OldToDoInfo", toDoInfoDTO);
            var result = await dialogHostService.ShowDialog("EditWaitUC", paras);
            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.ContainsKey("NewToDoInfo"))
                {
                    var newModel = result.Parameters.GetValue<ToDoInfoDTO>("NewToDoInfo"); // 接收数据
                    ApiRequest apiRequest = new ApiRequest();
                    apiRequest.Method = RestSharp.Method.PUT;
                    apiRequest.Parameters = newModel;
                    apiRequest.Route = "ToDo/EditToDo";
                    ApiResponse response = _HttpClient.Execute(apiRequest);
                    if (response.ResultCode == 1)
                    {
                        CallStatWait();
                        CreateToDoList();
                    }
                    else
                    {
                        MessageBox.Show(response.Msg);
                    }
                }
            }
        }

        /// <summary>
        /// 修改待办事项状态
        /// </summary>
        /// <param name="dTO"></param>
        private void ChangedToDoStatus(ToDoInfoDTO dTO)
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.PUT;
            apiRequest.Parameters = dTO;
            apiRequest.Route = "ToDo/UpdateStatus";
            ApiResponse response = _HttpClient.Execute(apiRequest);
            if (response.ResultCode == 1)
            {
                CallStatWait();
                CreateToDoList();
            }
            else
            {
                MessageBox.Show(response.Msg);
            }
        }

        /// <summary>
        /// 获取备忘录统计数据
        /// </summary>
        private void CallStatMemo()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Route = "Memo/SataMemo";

            ApiResponse response = _HttpClient.Execute(apiRequest);
            if (response.ResultCode == 1)
            {
                StatPanelList[3].Result = response.ResultData.ToString();
            }
        }

        /// <summary>
        /// 显示添加备忘录对话框
        /// </summary>
        private async void ShowAddMemoDialog()
        {
            var result = await dialogHostService.ShowDialog("AddMemoUC", null);
            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.ContainsKey("AddMemoInfo"))
                {
                    var addModel = result.Parameters.GetValue<MemoInfoDTO>("AddMemoInfo"); // 接收数据
                    ApiRequest apiRequest = new ApiRequest();
                    apiRequest.Method = RestSharp.Method.POST;
                    apiRequest.Parameters = addModel;
                    apiRequest.Route = "Memo/AddMemo";
                    ApiResponse response = _HttpClient.Execute(apiRequest);
                    if (response.ResultCode == 1)
                    {
                        CallStatMemo();
                        CreateMemoList();
                    }
                    else
                    {
                        MessageBox.Show(response.Msg);
                    }
                }
            }
        }

        /// <summary>
        /// 显示编辑备忘录对话框
        /// </summary>
        /// <param name="memoInfoDTO"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void ShowEditMemoDialog(MemoInfoDTO memoInfoDTO)
        {
            DialogParameters paras = new DialogParameters();
            paras.Add("OldMemoInfo", memoInfoDTO);
            var result = await dialogHostService.ShowDialog("EditMemoUC", paras);
            if (result.Result == ButtonResult.OK)
            {
                if (result.Parameters.ContainsKey("NewMemoInfo"))
                {
                    var newModel = result.Parameters.GetValue<MemoInfoDTO>("NewMemoInfo"); // 接收数据
                    ApiRequest apiRequest = new ApiRequest();
                    apiRequest.Method = RestSharp.Method.PUT;
                    apiRequest.Parameters = newModel;
                    apiRequest.Route = "Memo/EditMemo";
                    ApiResponse response = _HttpClient.Execute(apiRequest);
                    if (response.ResultCode == 1)
                    {
                        CreateMemoList();
                    }
                    else
                    {
                        MessageBox.Show(response.Msg);
                    }
                }
            }
        }
    }
}
