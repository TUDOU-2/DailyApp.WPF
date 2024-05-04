using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyApp.API.DataModel
{
    /// <summary>
    /// 待办事项数据模型
    /// </summary>
    [Table("ToDoInfo")]
    public class ToDoInfo
    {
        [Key]
        public int waitID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        /// <summary>
        /// 状态；0：未完成；1：已完成
        /// </summary>
        public int status { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
