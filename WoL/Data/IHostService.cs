using System.Collections.Generic;
using System.Threading.Tasks;
using WoL.Models;

namespace WoL.Data
{
    public interface IHostService
    {
        Task Add(Host host);
        Task Delete(int id);
        Task<Host> Find(int id);
        Task<List<Host>> GetAll();
        Task Update(Host host);
    }
}