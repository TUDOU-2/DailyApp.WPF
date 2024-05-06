using DailyApp.WPF.HttpClients;
using DailyApp.WPF.Service;
using DailyApp.WPF.ViewModels;
using DailyApp.WPF.ViewModels.Dialogs;
using DailyApp.WPF.Views;
using DailyApp.WPF.Views.Dialogs;
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
            containerRegistry.RegisterForNavigation<AddWaitUC, AddWaitUCViewModel>(); // 添加待办事项窗口
            containerRegistry.RegisterForNavigation<EditWaitUC, EditWaitUCViewModel>(); // 编辑待办事项窗口
            containerRegistry.RegisterForNavigation<EditMemoUC, EditMemoUCViewModel>(); // 编辑备忘录窗口
            containerRegistry.RegisterForNavigation<AddMemoUC, AddMemoUCViewModel>(); // 添加备忘录窗口

            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl")); // 请求地址

            // 导航页
            containerRegistry.RegisterForNavigation<HomeUC, HomeUCViewModel>();
            containerRegistry.RegisterForNavigation<ToDoUC, ToDoUCViewModel>();
            containerRegistry.RegisterForNavigation<MemoUC, MemoUCViewModel>();
            containerRegistry.RegisterForNavigation<SettingsUC, SettingsUCViewModel>();

            containerRegistry.RegisterForNavigation<PersonalUC, PersonalUCViewModel>(); // 个性化
            containerRegistry.RegisterForNavigation<SysSetUC>(); // 系统设置
            containerRegistry.RegisterForNavigation<AboutUsUC>(); // 关于更多

            containerRegistry.Register<DialogHostService>(); // 自定义对话框服务
        }

        /// <summary>
        /// 初始化
        /// </summary>
        override protected void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginUC", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }

                var mainVM = Current.MainWindow.DataContext as MainWinViewModel; // 获取主界面上下文
                if (mainVM != null)
                {
                    if (callback.Parameters.ContainsKey("LoginName"))
                    {
                        string name = callback.Parameters.GetValue<string>("LoginName");
                        mainVM.SetDefultNav(name); // 设置默认导航
                    }
                }

                base.OnInitialized();
            });
        }
    }

}
