using System.Threading.Tasks;
using Orleans;
using WebApi.Models;

namespace WebApi.Grains
{
    public interface IUserGrain:IGrainWithIntegerKey
    {
        ValueTask<UserInfo> GetInfo();
		Task<UserInfo> UpdateInfo(UserInfo info);
		Task<uint> GetBalance();
	}
}