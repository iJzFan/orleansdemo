using System;
using System.Threading.Tasks;
using Orleans;
using Orleans.CodeGeneration;
using Orleans.Transactions.Abstractions;
using WebApi.Models;

[assembly: GenerateSerializer(typeof(Balance))]

namespace WebApi.Grains
{
    public class AccountGrain : Grain, IAccountGrain
    {
        private readonly ITransactionalState<Balance> _balance;

        public AccountGrain(
            [TransactionalState("balance")] ITransactionalState<Balance> balance)
        {
            _balance = balance ?? throw new ArgumentNullException(nameof(balance));
        }

        async Task IAccountGrain.Deposit(uint amount)
        {
            await _balance.PerformUpdate(x => x.Value += amount);
        }

        async Task IAccountGrain.Withdraw(uint amount)
        {
            await _balance.PerformUpdate(x =>
            {
                if (x.Value < amount)
                {
                    throw new InvalidOperationException( "The transferred amount was greater than the balance.");
                }
                return x.Value -= amount;
            });
        }

        Task<uint> IAccountGrain.GetBalance()
        {
            return _balance.PerformRead(x => x.Value);
        }
    }
}