using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.ViewModels
{
    public class LoginUCViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; } = "我的日常";

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand LoginCmm { get; set; }
        public DelegateCommand<string> ShowRegisterInfoCmm { get; set; }

        private string _SelectedIndex = "0";
        private string _Pwd;

        public string Pwd
        {
            get { return _Pwd; }
            set { _Pwd = value; RaisePropertyChanged(); }
        }


        public string SelectedIndex
        {
            get { return _SelectedIndex; }
            set { _SelectedIndex = value; RaisePropertyChanged(); }
        }

        public LoginUCViewModel()
        {
            LoginCmm = new DelegateCommand(Login);
            ShowRegisterInfoCmm = new DelegateCommand<string>(SwitchDisplayInfo);
        }

        private void Login()
        {
            string inputPwd = Pwd;
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
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
