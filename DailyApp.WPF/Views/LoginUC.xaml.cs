﻿using DailyApp.WPF.MsgEvents;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DailyApp.WPF.Views
{
    /// <summary>
    /// LoginUC.xaml 的交互逻辑
    /// </summary>
    public partial class LoginUC : UserControl
    {
        private readonly IEventAggregator _Aggregator;
        public LoginUC(IEventAggregator Aggregator)
        {
            InitializeComponent();
            _Aggregator = Aggregator;

            _Aggregator.GetEvent<MsgEvent>().Subscribe(Sub); // 订阅
        }

        private void Sub(string obj)
        {
            RegLoginBar.MessageQueue.Enqueue(obj);

        }
    }
}
