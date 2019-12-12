using System.Threading.Tasks;

namespace WoL.Services
{
    public interface IWakeService
    {
        Task Wake(byte[] mac);
    }
}