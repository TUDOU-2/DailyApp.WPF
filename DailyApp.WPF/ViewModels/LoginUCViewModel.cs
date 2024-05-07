using DailyApp.WPF.DTOs;
using DailyApp.WPF.HttpClients;
using DailyApp.WPF.MsgEvents;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyApp.WPF.ViewModels
{
    public class LoginUCViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; } = "我的日常";

        public event Action<IDialogResult> RequestClose;


        public DelegateCommand LoginCmm { get; set; }
        public DelegateCommand<string> ShowRegisterInfoCmm { get; set; }
        public DelegateCommand RegCmm { get; set; }
        private readonly HttpRestClient _HttpRestClient;
        private readonly IEventAggregator _aggregator;
        private string _SelectedIndex = "0";
        private string _Account;


        private string _Pwd;
        private AccountInfoDTO _AccountInfoDTO;

        public AccountInfoDTO AccountInfoDTO
        {
            get { return _AccountInfoDTO; }
            set { _AccountInfoDTO = value; RaisePropertyChanged(); }
        }

        public string Account
        {
            get { return _Account; }
            set { _Account = value; }
        }

        public string Pwd
        {
            get { return _Pwd; }
            set { _Pwd = value; }
        }

        public string SelectedIndex
        {
            get { return _SelectedIndex; }
            set { _SelectedIndex = value; RaisePropertyChanged(); }
        }

        // 构造函数
        public LoginUCViewModel(HttpRestClient HttpRestClient, IEventAggregator aggregator)
        {
            LoginCmm = new DelegateCommand(Login);
            ShowRegisterInfoCmm = new DelegateCommand<string>(SwitchDisplayInfo);
            RegCmm = new DelegateCommand(Reg);
            AccountInfoDTO = new AccountInfoDTO();
            _HttpRestClient = HttpRestClient;
            _aggregator = aggregator;
        }

        private void Reg() // 注册
        {
            // 数据验证
            if (string.IsNullOrEmpty(AccountInfoDTO.Account) || string.IsNullOrEmpty(AccountInfoDTO.Name) ||
                string.IsNullOrEmpty(AccountInfoDTO.Pwd) || string.IsNullOrEmpty(AccountInfoDTO.ConfirmPwd))
            {
                _aggregator.GetEvent<MsgEvent>().Publish("请填写完整信息");
                return;
            }
            if (AccountInfoDTO.Pwd != AccountInfoDTO.ConfirmPwd)
            {
                _aggregator.GetEvent<MsgEvent>().Publish("两次密码不一致");
                return;
            }

            // 调用API
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.POST;
            apiRequest.Route = "Account/Reg";

            AccountInfoDTO.Pwd = Md5Hepler.GetMd5(AccountInfoDTO.Pwd);// 密码加密
            AccountInfoDTO.ConfirmPwd = Md5Hepler.GetMd5(AccountInfoDTO.ConfirmPwd);// 密码加密
            apiRequest.Parameters = AccountInfoDTO;


            ApiResponse response = _HttpRestClient.Execute(apiRequest); // 请求API
            if (response.ResultCode == 1)
            {
                _aggregator.GetEvent<MsgEvent>().Publish(response.Msg);
                SelectedIndex = "0";
            }
            else
            {
                _aggregator.GetEvent<MsgEvent>().Publish(response.Msg);
            }
        }
        private async void Login() // 登录
        {
            // 数据验证
            if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(Pwd))
            {
                _aggregator.GetEvent<MsgEvent>().Publish("请填写完整信息");
                return;
            }

            // 调用API
            ApiRequest apiRequest = new ApiRequest();
            apiRequest.Method = RestSharp.Method.GET;
            Pwd = Md5Hepler.GetMd5(Pwd);// 密码加密
            apiRequest.Route = $"Account/Login?account={Account}&pwd={Pwd}"; // 路由+传参
            ApiResponse response = await _HttpRestClient.ExecuteAsync(apiRequest); // 请求API
            if (response.ResultCode == 1)
            {
                if (RequestClose != null)
                {
                    // 反序列化
                    AccountInfoDTO accountInfoDTO = JsonConvert.DeserializeObject<AccountInfoDTO>(response.ResultData.ToString());
                    
                    DialogParameters patas = new DialogParameters();
                    patas.Add("LoginName", accountInfoDTO.Name);
                    RequestClose(new DialogResult(ButtonResult.OK,patas));
                }
            }
            else
            {
                _aggregator.GetEvent<MsgEvent>().Publish(response.Msg);
            }
        }

        private void SwitchDisplayInfo(string indexStr) // 切换显示的信息
        {
            SelectedIndex = indexStr;
        }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
