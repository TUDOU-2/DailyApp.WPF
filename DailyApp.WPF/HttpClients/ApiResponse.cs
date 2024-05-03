using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.WPF.HttpClients
{
    /// <summary>
    /// 接收模型
    /// </summary>
    public class ApiResponse
    {
        public int ResultCode { get; set; } // 结果编码
        public string Msg { get; set; } // 结果信息
        public object ResultData { get; set; } // 结果数据
    }
}
