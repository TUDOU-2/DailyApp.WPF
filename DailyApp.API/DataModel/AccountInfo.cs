using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyApp.API.DataModel
{
    /// <summary>
    /// 账号数据模型
    /// </summary>
    [Table("AccountInfo")] // 表名
    public class AccountInfo
    {
        [Key] // 主键 自增
        public int AccountId { get; set; }
        public string  Name { get; set; }
        public string Account { get; set; }
        public string Pwd { get; set; }
    }
}
