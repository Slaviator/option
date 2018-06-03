namespace CodingHelmet.SampleApp.Domain.Interfaces
{
    class MoneyTransaction
    {
        public decimal Amount { get; }

        public MoneyTransaction(decimal amount)
        {
            Amount = amount;
        }
    }
}