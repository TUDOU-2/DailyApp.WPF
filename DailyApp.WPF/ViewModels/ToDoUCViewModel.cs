using DailyApp.WPF.DTOs;
using DailyApp.WPF.HttpClients;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.WPF.ViewModels
{
    class ToDoUCViewModel : BindableBase, INavigationAware
    {
        private List<ToDoInfoDTO> _ToDoList; // 待办事项列表
        private bool _IsRightDrawerOpen; // 右侧抽屉是否打开
        private Visibility _visibility; // 是否显示图片
        public ToDoInfoDTO toDoInfoDTO { get; set; } = new ToDoInfoDTO(); // 待办事项信息

        private readonly HttpRestClient httpClient; // 请求api的客户端

        public string SearchToDoTitle { get; set; } // 查询条件(标题)
        private int _SearchToDoIndex; // 查询条件(状态)

        public DelegateCommand AddCommand { get; private set; } // 显示添加待办事项命令
        public DelegateCommand QueryToDoListCmm { get; private set; } // 查询待办事项命令
        public DelegateCommand AddToDoCmm { get; private set; } // 添加待办事项命令
        public DelegateCommand<ToDoInfoDTO> DelCmm { get; private set; } // 删除待办事项命令

        public Visibility visibility
        {
            get { return _visibility; }
            set { _visibility = value; RaisePropertyChanged(); }
        }

        public int SearchToDoIndex
        {
            get { return _SearchToDoIndex; }
            set { _SearchToDoIndex = value; RaisePropertyChanged(); }
        }

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
        public ToDoUCViewModel(HttpRestClient HttpClient) // 构造函数
        {
            AddCommand = new DelegateCommand(Add);
            httpClient = HttpClient;
            QueryToDoList();
            QueryToDoListCmm = new DelegateCommand(QueryToDoList);
            AddToDoCmm = new DelegateCommand(AddToDo);
            DelCmm = new DelegateCommand<ToDoInfoDTO>(Del);
        }

        /// <summary>
        /// 删除待办事项
        /// </summary>
        /// <param name="dTO"></param>
        private void Del(ToDoInfoDTO dTO)
        {
            var result = MessageBox.Show("确定删除吗？", "删除", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                ApiRequest apiRequest = new ApiRequest();
                apiRequest.Method = RestSharp.Method.DELETE;
                apiRequest.Route = $"ToDo/DelToDo?toDoID={dTO.WaitID}";
                ApiResponse response = httpClient.Execute(apiRequest);
                if (response.ResultCode == 1)
                {
                    QueryToDoList();
                }
                else
                {
                    MessageBox.Show(response.Msg);
                }
            }
        }

        private void AddToDo()
        {
            if (string.IsNullOrEmpty(toDoInfoDTO.Title) || string.IsNullOrEmpty(toDoInfoDTO.Content))
            {
                MessageBox.Show("待办事项信息不全");
                return;
            }
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.POST;
            apiRequest.Parameters = toDoInfoDTO;
            apiRequest.Route = "ToDo/AddWait";
            ApiResponse response = httpClient.Execute(apiRequest);
            if (response.ResultCode == 1)
            {
                QueryToDoList();
                IsRightDrawerOpen = false;
            }
            else
            {
                MessageBox.Show(response.Msg);
            }
        }

        /// <summary>
        /// 查询待办事项数据
        /// </summary>
        private void QueryToDoList()
        {
            int? status = null;
            if (SearchToDoIndex == 1)
            {
                status = 0;
            }
            if (SearchToDoIndex == 2)
            {
                status = 1;
            }
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Route = $"ToDo/QueryToDo?title={SearchToDoTitle}&status={status}";

            ApiResponse response = httpClient.Execute(apiRequest);
            if (response.ResultCode == 1)
            {
                ToDoList = JsonConvert.DeserializeObject<List<ToDoInfoDTO>>(response.ResultData.ToString());
                visibility = (ToDoList.Count > 0) ? Visibility.Hidden : Visibility.Visible;
            }
            else
            {
                ToDoList = new List<ToDoInfoDTO>();
            }
        }

        private void Add()
        {
            IsRightDrawerOpen = true;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("SelectedIndex"))
            {
                SearchToDoIndex = navigationContext.Parameters.GetValue<int>("SelectedIndex");
            }
            else
            {
                SearchToDoIndex = 0;
            }
            QueryToDoList();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
