using DailyApp.WPF.DTOs;
using DailyApp.WPF.MsgEvents;
using DailyApp.WPF.Service;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.ViewModels.Dialogs
{
    /// <summary>
    /// 添加待办事项视图模型
    /// </summary>
    class AddWaitUCViewModel : IDialogHostAware
    {
        private const string DailogHostName = "RootDialog"; // 主机对话框唯一标识
        public DelegateCommand SaveCommand { get; set; } // 确定命令
        public DelegateCommand CancelCommand { get; set; } // 取消命令
        public ToDoInfoDTO ToDoInfoDTO { get; set; } = new ToDoInfoDTO(); // 待办事项信息
        private readonly IEventAggregator _aggregator;
        public AddWaitUCViewModel(IEventAggregator aggregator) // 构造函数
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
            _aggregator = aggregator;
        }

        public void OnDialogOpening(IDialogParameters parameters)
        {

        }

        private void Save()
        {
            if(string.IsNullOrEmpty(ToDoInfoDTO.Title) || string.IsNullOrEmpty(ToDoInfoDTO.Content))
            {
                _aggregator.GetEvent<MsgEvent>().Publish("待办事项信息不全");
                return;
            }
            if (DialogHost.IsDialogOpen(DailogHostName))
            {
                DialogParameters paras = new DialogParameters();
                paras.Add("AddToDoInfo", ToDoInfoDTO);
                DialogHost.Close(DailogHostName, new DialogResult(ButtonResult.OK, paras));
            }
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DailogHostName))
            {
                DialogHost.Close(DailogHostName,new DialogResult(ButtonResult.No));
            }
        }
    }
}
