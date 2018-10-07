using System.Threading.Tasks;
using Orleans;

namespace Grains
{
    public class UserGrain:Grain,IUserGrain
    {
        public Task<UserInfo> GetInfo()
        {
            return Task.FromResult(new UserInfo{Name = "xiaoming",Age = 18});
        }
    }
}