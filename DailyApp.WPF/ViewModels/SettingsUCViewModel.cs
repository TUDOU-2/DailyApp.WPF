using DailyApp.WPF.Extensions;
using DailyApp.WPF.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace DailyApp.WPF.ViewModels
{
    class SettingsUCViewModel : BindableBase
    {
        private void Navigate(LeftMenulnfo menu)
        {
            if (menu == null || string.IsNullOrWhiteSpace(menu.ViewName))
                return;

            _regionManager.Regions[PrismManager.SettingsViewRegionName].RequestNavigate(menu.ViewName);

        }

        public DelegateCommand<LeftMenulnfo> NavigateCommand { get; private set; }
        private List<LeftMenulnfo> _LeftMenuList;
        private readonly IRegionManager _regionManager;

        public List<LeftMenulnfo> LeftMenuList
        {
            get { return _LeftMenuList; }
            set { _LeftMenuList = value; RaisePropertyChanged(); }
        }

        public SettingsUCViewModel(IRegionManager regionManager) // 构造函数
        {
            NavigateCommand = new DelegateCommand<LeftMenulnfo>(Navigate);
            _regionManager = regionManager;
            CreateMenuList();
        }

        void CreateMenuList()
        {
            _LeftMenuList = new List<LeftMenulnfo>();
            _LeftMenuList.Add(new LeftMenulnfo() { Icon = "Palette", MenuName = "个性化", ViewName = "PersonalUC" });
            _LeftMenuList.Add(new LeftMenulnfo() { Icon = "Cog", MenuName = "系统设置", ViewName = "SysSetUC" });
            _LeftMenuList.Add(new LeftMenulnfo() { Icon = "Information", MenuName = "关于更多", ViewName = "AboutUsUC" });
        }
    }
}
