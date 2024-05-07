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
using System.Windows;

namespace DailyApp.WPF.ViewModels
{
    class MemoUCViewModel : BindableBase
    {
        private List<MemoInfoDTO> _MemoList; // 备忘录列表
        private bool _IsRightDrawerOpen; // 右侧抽屉是否打开
        private Visibility _visibility; // 是否显示图片
        public MemoInfoDTO memoInfoDTO { get; set; } = new MemoInfoDTO(); // 备忘录信息


        private readonly HttpRestClient httpClient; // 请求api的客户端
        public string SearchMemoTitle { get; set; } // 查询条件(标题)

        public DelegateCommand AddCommand { get; private set; } // 显示添加备忘录命令
        public DelegateCommand QueryMemoListCmm { get; private set; } // 查询备忘录命令
        public DelegateCommand AddMemoCmm { get; private set; } // 添加备忘录命令
        public DelegateCommand<MemoInfoDTO> DelCmm { get; private set; } // 删除备忘录命令



        public bool IsRightDrawerOpen
        {
            get { return _IsRightDrawerOpen; }
            set { _IsRightDrawerOpen = value; RaisePropertyChanged(); }
        }
        public Visibility visibility
        {
            get { return _visibility; }
            set { _visibility = value; RaisePropertyChanged(); }
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
            AddMemoCmm = new DelegateCommand(AddMemo);
            DelCmm = new DelegateCommand<MemoInfoDTO>(Del);
        }

        /// <summary>
        /// 删除备忘录
        /// </summary>
        /// <param name="dTO"></param>
        private void Del(MemoInfoDTO dTO)
        {
            var result = MessageBox.Show("确定删除吗？", "删除", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                ApiRequest apiRequest = new ApiRequest();
                apiRequest.Method = RestSharp.Method.DELETE;
                apiRequest.Route = $"Memo/DelMemo?memoID={dTO.MemoID}";
                ApiResponse response = httpClient.Execute(apiRequest);
                if (response.ResultCode == 1)
                {
                    QueryMemoList();
                }
                else
                {
                    MessageBox.Show(response.Msg);
                }
            }
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        private void AddMemo()
        {
            if (string.IsNullOrEmpty(memoInfoDTO.Title) || string.IsNullOrEmpty(memoInfoDTO.Content))
            {
                MessageBox.Show("备忘录信息不全");
                return;
            }
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.POST;
            apiRequest.Parameters = memoInfoDTO;
            apiRequest.Route = "Memo/AddMemo";
            ApiResponse response = httpClient.Execute(apiRequest);
            if (response.ResultCode == 1)
            {
                QueryMemoList();
                IsRightDrawerOpen = false;
            }
            else
            {
                MessageBox.Show(response.Msg);
            }
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
                visibility = (MemoList.Count > 0) ? Visibility.Hidden : Visibility.Visible;
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
