using Microsoft.EntityFrameworkCore;

namespace WoL.Data
{
    public class SqlServerDbContext : ApplicationDbContext
    {
        public SqlServerDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
