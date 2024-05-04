using DailyApp.WPF.Extensions;
using DailyApp.WPF.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace DailyApp.WPF.ViewModels
{
    internal class MainWinViewModel : BindableBase
    {
        private List<LeftMenulnfo> _LeftMenulnfo;
        private readonly IRegionManager _regionManager;
        public DelegateCommand<LeftMenulnfo> NavigeteCmm { get; set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        private IRegionNavigationJournal journal;

        public List<LeftMenulnfo> LeftMenulnfo
        {
            get { return _LeftMenulnfo; }
            set { _LeftMenulnfo = value; RaisePropertyChanged(); }
        }

        public MainWinViewModel(IRegionManager regionManager) // 构造函数
        {
            LeftMenulnfo = new List<LeftMenulnfo>();
            CreateMenu();
            _regionManager = regionManager;
            NavigeteCmm = new DelegateCommand<LeftMenulnfo>(Navigate);

            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoBack)
                    journal.GoBack();
            }); // 后退按钮

            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoForward)
                    journal.GoForward();
            }); // 前进按钮
        }

        private void Navigate(LeftMenulnfo menulnfo)
        {
            if (menulnfo == null || string.IsNullOrEmpty(menulnfo.ViewName))
            {
                return;
            }
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(menulnfo.ViewName, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }

        private void CreateMenu()
        {
            LeftMenulnfo.Add(new LeftMenulnfo() { Icon = "Home", MenuName = "首页", ViewName = "HomeUC" });
            LeftMenulnfo.Add(new LeftMenulnfo() { Icon = "NotebookOutline", MenuName = "待办事项", ViewName = "ToDoUC" });
            LeftMenulnfo.Add(new LeftMenulnfo() { Icon = "NotebookPlus", MenuName = "备忘录", ViewName = "MemoUC" });
            LeftMenulnfo.Add(new LeftMenulnfo() { Icon = "Cog", MenuName = "设置", ViewName = "SettingsUC" });
        }

        public void SetDefultNav(string LoginName)
        {
            NavigationParameters paras = new NavigationParameters();
            paras.Add("LoginName", LoginName);
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("HomeUC", back =>
            {
                journal = back.Context.NavigationService.Journal;
            }, paras);
        }
    }
}
