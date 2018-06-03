using CodingHelmet.Optional;

namespace CodingHelmet.SampleApp.Domain.Interfaces
{
    interface IAccount
    {
        MoneyTransaction Deposit(decimal amount);
        Option<MoneyTransaction> TryWithdraw(decimal amount);
    }
}