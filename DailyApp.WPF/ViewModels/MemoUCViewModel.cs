using DailyApp.WPF.DTOs;
using DailyApp.WPF.HttpClients;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.ViewModels
{
    class MemoUCViewModel : BindableBase
    {
        private List<MemoInfoDTO> _MemoList; // 备忘录列表
        private bool _IsRightDrawerOpen; // 右侧抽屉是否打开
        private readonly HttpRestClient httpClient; // 请求api的客户端
        public string SearchMemoTitle { get; set; } // 查询条件(标题)

        public DelegateCommand AddCommand { get; private set; } // 显示添加备忘录命令
        public DelegateCommand QueryMemoListCmm { get; private set; } // 查询备忘录命令



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

        public MemoUCViewModel(HttpRestClient HttpClient) // 构造函数
        {
            AddCommand = new DelegateCommand(Add);
            httpClient = HttpClient;
            QueryMemoList();
            QueryMemoListCmm = new DelegateCommand(QueryMemoList);
        }

        /// <summary>
        /// 查询备忘录数据
        /// </summary>
        private void QueryMemoList()
        {
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            apiRequest.Route = $"Memo/QueryMemo?title={SearchMemoTitle}";

            ApiResponse response = httpClient.Execute(apiRequest);
            if (response.ResultCode == 1)
            {
                MemoList = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(response.ResultData.ToString());
            }
            else
            {
                MemoList = new List<MemoInfoDTO>();
            }
        }

        private void Add()
        {
            IsRightDrawerOpen = true;
        }
    }
}
