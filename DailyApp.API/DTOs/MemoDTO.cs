namespace DailyApp.API.DTOs
{
    /// <summary>
    /// 备忘录DTO(接收添加备忘录的数据 返回查询/查询显示)
    /// </summary>
    public class MemoDTO
    {
        public int MemoID { get; set; }
        public string? Title { get; set; } // 标题
        public string? Content { get; set; } // 内容
    }
}
