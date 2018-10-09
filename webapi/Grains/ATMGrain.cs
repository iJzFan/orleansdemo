using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.Concurrency;

namespace WebApi.Grains
{
    [StatelessWorker]
    public class ATMGrain : Grain, IATMGrain
    {
        Task IATMGrain.Transfer(long fromAccount, long toAccount, uint amountToTransfer)
        {
            return Task.WhenAll(
                this.GrainFactory.GetGrain<IAccountGrain>(fromAccount).Withdraw(amountToTransfer),
                this.GrainFactory.GetGrain<IAccountGrain>(toAccount).Deposit(amountToTransfer));
        }
    }
}
