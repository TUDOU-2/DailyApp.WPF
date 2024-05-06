using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DailyApp.API.DataModel
{
    /// <summary>
    /// 备忘录数据模型
    /// </summary>
    [Table("MemoInfo")]
    public class MemoInfo
    {
        [Key]
        public int memoID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
