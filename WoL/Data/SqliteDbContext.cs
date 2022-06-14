using Microsoft.EntityFrameworkCore;

namespace WoL.Data
{
    public class SqliteDbContext : ApplicationDbContext
    {
        public SqliteDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
