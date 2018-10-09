using System;
using System.Threading.Tasks;
using Orleans;

namespace WebApi.Grains
{
    public interface IATMGrain : IGrainWithIntegerKey
    {
        [Transaction(TransactionOption.Create)]
        Task Transfer(long fromAccount, long toAccount, uint amountToTransfer);
    }
}
