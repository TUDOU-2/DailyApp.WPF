namespace DailyApp.API.DTOs
{
    /// <summary>
    /// 待办事项DTO(接收添加待办事项的数据 返回查询/查询显示)
    /// </summary>
    public class ToDoDTO
    {
        public int WaitID { get; set;}
        public string? Title { get; set; } // 标题
        public string? Content { get; set; } // 内容
        public int Status { get; set; } = 0; // 状态；0：未完成；1：已完成
    }
}
