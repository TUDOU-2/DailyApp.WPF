using DailyApp.WPF.HttpClients;
using DailyApp.WPF.ViewModels;
using DailyApp.WPF.Views;
using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Configuration;
using System.Data;
using System.Windows;

namespace DailyApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWin>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWin, MainWinViewModel>();
            containerRegistry.RegisterDialog<LoginUC, LoginUCViewModel>(); // 登录窗口

            containerRegistry.GetContainer().Register<HttpRestClient>(made:Parameters.Of.Type<string>(serviceKey:"webUrl")); // 请求地址

            // 导航页
            containerRegistry.RegisterForNavigation<HomeUC, HomeUCViewModel>();
            containerRegistry.RegisterForNavigation<ToDoUC, ToDoUCViewModel>();
            containerRegistry.RegisterForNavigation<MemoUC, MemoUCViewModel>();
            containerRegistry.RegisterForNavigation<SettingsUC, SettingsUCViewModel>();

            containerRegistry.RegisterForNavigation<PersonalUC, PersonalUCViewModel>(); // 个性化
            containerRegistry.RegisterForNavigation<SysSetUC>(); // 系统设置
            containerRegistry.RegisterForNavigation<AboutUsUC>(); // 关于更多
        }

        /// <summary>
        /// 初始化
        /// </summary>
        //override protected void OnInitialized()
        //{
        //    var dialog = Container.Resolve<IDialogService>();
        //    dialog.ShowDialog("LoginUC", callback =>
        //    {
        //        if (callback.Result != ButtonResult.OK)
        //        {
        //            Environment.Exit(0);
        //            return;
        //        }

        //        base.OnInitialized();
        //    });
        //}
    }

}
