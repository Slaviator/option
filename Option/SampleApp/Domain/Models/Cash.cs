using CodingHelmet.Optional;
using CodingHelmet.SampleApp.Domain.Interfaces;

namespace CodingHelmet.SampleApp.Domain.Models
{
    class Cash : IAccount
    {
        public MoneyTransaction Deposit(decimal amount) =>
            new MoneyTransaction(amount);

        public Option<MoneyTransaction> TryWithdraw(decimal amount) =>
            new MoneyTransaction(-amount);
    }
}