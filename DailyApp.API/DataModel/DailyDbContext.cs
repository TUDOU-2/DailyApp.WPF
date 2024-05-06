using Microsoft.EntityFrameworkCore;

namespace DailyApp.API.DataModel
{
    public class DailyDbContext : DbContext
    {
        public DailyDbContext(DbContextOptions<DailyDbContext> options) : base(options)
        {
            
        }

        // 数据迁移时会自动创建表
        public virtual DbSet<AccountInfo> AccountInfo { get; set; }
        public virtual DbSet<ToDoInfo> ToDoInfo { get; set; }
        public virtual DbSet<MemoInfo> MemoInfo { get; set; }
    }
}
