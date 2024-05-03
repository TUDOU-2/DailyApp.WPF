namespace DailyApp.API.ApiRepinses
{
    // 响应模型
    public class ApiResponse
    {
        public int ResultCode { get; set; } // 结果编码
        public string Msg { get; set; } // 结果信息
        public object ResultData { get; set; } // 结果数据
    }
}
