using System.Threading.Tasks;
using Orleans;

namespace Grains
{
    public interface IUserGrain:IGrainWithIntegerKey
    {
        Task<UserInfo> GetInfo();
    }
}