using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WoL.Models;

namespace WoL.Data
{
    public class HostService : IHostService
    {
        private readonly ApplicationDbContext context;

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
            await context.Hosts.AddAsync(host).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
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
            return await context.Hosts.FindAsync(id);
        }
    }
}
