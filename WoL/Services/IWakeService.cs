using System.Threading.Tasks;

namespace WoL.Services
{
    public interface IWakeService
    {
        Task WakeAsync(byte[] mac);
    }
}