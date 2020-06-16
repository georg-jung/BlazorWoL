using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using WoL.Models;

namespace WoL.Data
{
    public class HostService : IHostService
    {
        private readonly ApplicationDbContext context;
        
        private static readonly Regex tsqlParseDuplicateValue = new Regex(@"The duplicate key value is \(([^)]+)\)");
        private static readonly Regex tsqlParseIndexName = new Regex(@"with unique index '([^']+)'\.");
        // this makes assumptions about the names of the indices which is kind of bad
        // on the other hand it sticks to ef's index naming conventions
        private static readonly Regex tsqlParseIdxField = new Regex(@"_([^_]+)$");

        private static readonly Regex sqliteParseParseIdxField = new Regex(@"UNIQUE constraint failed: ([^']+)");

        public HostService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task<List<Host>> GetAll()
        {
            return context.Hosts.ToListAsync();
        }

        public async Task Add(Host host)
        {
            try
            {
                await context.Hosts.AddAsync(host).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException dbue)
                // handle tsql
                // see https://docs.microsoft.com/en-us/sql/relational-databases/errors-events/database-engine-events-and-errors
                // i.e. "Cannot insert duplicate key row in object 'dbo.Host' with unique index 'IX_Host_Hostname'. The duplicate key value is (georg-nuc). The statement has been terminated."
                when (dbue.InnerException is SqlException sqlEx &&
                      (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                // otherwise this host would be added again when further using the same DbContext
                context.Entry(host).State = EntityState.Detached;

                var msg = sqlEx.Message;
                var duplVal = tsqlParseDuplicateValue.Match(msg).Groups[1].Value;
                var idxName = tsqlParseIndexName.Match(msg).Groups[1].Value;
                var fieldName = tsqlParseIdxField.Match(idxName).Groups[1].Value;
                throw new IHostService.DuplicateEntryException(fieldName, duplVal, nameof(host), dbue);
            }
            catch (DbUpdateException dbue)
                // handle sqlite
                // see https://sqlite.org/rescode.html
                // i.e. "SQLite Error 19: 'UNIQUE constraint failed: Host.MacAddress'."
                when (dbue.InnerException is SqliteException sqlEx &&
                      (sqlEx.SqliteErrorCode == 19 || sqlEx.SqliteExtendedErrorCode == 2067))
            {
                // otherwise this host would be added again when further using the same DbContext
                context.Entry(host).State = EntityState.Detached;

                var msg = sqlEx.Message;
                var fieldName = sqliteParseParseIdxField.Match(msg).Groups[1].Value;
                if (fieldName.StartsWith("host.", StringComparison.OrdinalIgnoreCase))
                    fieldName = fieldName.Substring(5);
                throw new IHostService.DuplicateEntryException(fieldName, nameof(host), dbue);
            }
            catch
            {
                // otherwise this host would be added again when further using the same DbContext
                context.Entry(host).State = EntityState.Detached;
                throw;
            }
        }

        public async Task Update(Host host)
        {
            context.Hosts.Update(host);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Delete(int id)
        {
            var host = await context.Hosts.FindAsync(id).ConfigureAwait(false);
            context.Hosts.Remove(host);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Host> Find(int id)
        {
            return await context.Hosts.FindAsync(id).ConfigureAwait(false);
        }
    }
}
