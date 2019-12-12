using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WoL.Models;

namespace WoL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Host> Hosts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //This will singularize all table names
            foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
            {
                // this works different since ef core 3
                // https://docs.microsoft.com/de-de/ef/core/what-is-new/ef-core-3.0/breaking-changes#provider-specific-metadata-api-changes
                entityType.SetTableName(entityType.ClrType.Name);
            };

            // disable cascading deletes globally
            /*var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;*/

            builder.Entity<Host>().HasIndex(e => e.Hostname).IsUnique();
            builder.Entity<Host>().HasIndex(e => e.MacAddress).IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
