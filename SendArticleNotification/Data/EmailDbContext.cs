using Microsoft.EntityFrameworkCore;
using SendArticleNotification.Models;

namespace SendArticleNotification.Data
{
    public class EmailDbContext : DbContext
    {
        public DbSet<EmailToNotify> Emails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=notification_db;database=articles_notification;user=root;password=root",
                ServerVersion.AutoDetect("server=notification_db;database=articles_notification;user=root;password=root")
            );
        }
    }
}
