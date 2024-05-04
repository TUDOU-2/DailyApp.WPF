namespace DailyApp.API.DTOs
{
    public class StatWaitDTO
    {
        public int TotalCount { get; set; } // 总数
        public int FinishedCount { get; set; } // 已完成数

        // 完成百分比
        public string FinishPercent
        {
            get
            {
                if (TotalCount == 0)
                {
                    return "0.00%";
                }

                return ((double)FinishedCount * 100 / TotalCount).ToString("f2") + "%";
            }
        }
    }
}
