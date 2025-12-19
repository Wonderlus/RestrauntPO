using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL
{
    /// <summary>
    /// Фабрика для создания контекста БД во время разработки (для миграций)
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RestrauntContext>
    {
        public RestrauntContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RestrauntContext>();
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=RestrauntPO;Username=postgres;Password=zxc123"
            );

            return new RestrauntContext();
        }
    }
}
