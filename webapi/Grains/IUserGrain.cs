using System.Threading.Tasks;
using Orleans;
using WebApi.Models;

namespace WebApi.Grains
{
    public interface IUserGrain:IGrainWithIntegerKey
    {
        Task<UserInfo> GetInfo();
		Task<UserInfo> UpdateInfo(UserInfo info);
		Task<uint> GetBalance();
	}
}