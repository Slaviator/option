using System;
using System.Collections.Generic;
using System.Linq;
using CodingHelmet.Optional;
using CodingHelmet.SampleApp.Domain.Interfaces;

namespace CodingHelmet.SampleApp.Domain.Models
{
    class TransactionalAccount : IAccount
    {
        private RegisteredUser User { get; }
        private IList<MoneyTransaction> Transactions { get; } = new List<MoneyTransaction>();

        public TransactionalAccount(RegisteredUser user)
        {
            User = user;
        }

        public MoneyTransaction Deposit(decimal amount)
        {
            MoneyTransaction transaction = new MoneyTransaction(amount);
            Transactions.Add(transaction);

            Log(string.Format($"{UserName} deposited {amount:C} balance {Balance:C}"));

            return transaction;
        }

        public Option<MoneyTransaction> TryWithdraw(decimal amount)
        {
            if (Balance < amount)
                return None.Value;

            MoneyTransaction transaction = new MoneyTransaction(-amount);
            Transactions.Add(transaction);

            Log(string.Format($"{UserName} withdrew {amount:C} balance {Balance:C}"));

            return transaction;
        }

        public string UserName => User.UserName;

        private decimal Balance =>
            Transactions
                .Select(tran => tran.Amount)
                .DefaultIfEmpty(0.0M).Sum();

        private void Log(string message)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\tLOG ===> {0}", message);
            Console.ForegroundColor = color;
        }
    }
}