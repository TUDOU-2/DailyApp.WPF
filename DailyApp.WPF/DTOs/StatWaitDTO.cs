using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.DTOs
{
    class StatWaitDTO
    {
        /// <summary>
        /// 接收API待办事项统计的数据模型
        /// </summary>
        public int TotalCount { get; set; } // 总数
        public int FinishedCount { get; set; } // 已完成数
        public string FinishPercent { get; set; } // 完成百分比
    }
}
