using System.Linq;
using CodingHelmet.Optional;
using CodingHelmet.SampleApp.Domain.Interfaces;
using CodingHelmet.SampleApp.Domain.Models;
using CodingHelmet.SampleApp.Domain.ViewModels;
using CodingHelmet.SampleApp.Infrastructure;
using CodingHelmet.SampleApp.Presentation;

namespace CodingHelmet.SampleApp.Domain
{
    class DomainServices
    {
        private UserRepository UserRepository { get; } = new UserRepository();
        private ProductRepository ProductRepository { get; } = new ProductRepository();
        private AccountRepository AccountRepository { get; } = new AccountRepository();

        public void RegisterUser(string userName)
        {
            RegisteredUser user = CreateUser(userName);
            RegisterUser(user);
        }

        public void RegisterUser(string userName, string referrerName)
        {
            RegisteredUser user = CreateUser(userName);
            RegisterUser(user);
            SetReferrer(user, referrerName);
        }

        private void SetReferrer(RegisteredUser user, string referrerName)
        {
            Option<RegisteredUser> referrerOption = UserRepository.TryFind(referrerName);

            if (referrerOption is Some<RegisteredUser> referrer)
            {
                user.SetReferrer(referrer.Content);
            }
        }

        private void RegisterUser(RegisteredUser user)
        {
            UserRepository.Add(user);

            TransactionalAccount account = new TransactionalAccount(user);
            AccountRepository.Add(account);
        }

        private RegisteredUser CreateUser(string userName) =>
            new RegisteredUser(userName);

        public bool VerifyCredentials(string userName) =>
            UserRepository.TryFind(userName).Map(_ => true).Reduce(() => false);

        public IPurchaseViewModel Purchase(string userName, string itemName) =>
            UserRepository
                .TryFind(userName)
                .Map(user => Purchase(user, FindAccount(user), itemName))
                .Reduce(FailedPurchase.Instance);

        private IAccount FindAccount(RegisteredUser user) =>
            AccountRepository.FindByUser(user);

        public IPurchaseViewModel AnonymousPurchase(string itemName) =>
            Purchase(new AnonymousBuyer(), new Cash(), itemName);

        private IPurchaseViewModel Purchase(IUser user, IAccount account, string itemName) =>
            ProductRepository
                .TryFind(itemName)
                .Map(user.Purchase)
                .Map(receipt => Charge(user, account, receipt))
                .Reduce(() => new MissingProduct(itemName));

        private IPurchaseViewModel Charge(IUser user, IAccount account, IReceipt receipt) =>
            account
                .TryWithdraw(receipt.Price)
                .Map(trans => (IPurchaseViewModel) receipt)
                .Reduce(() => new InsufficientFunds(user.DisplayName, receipt.Price));

        public void Deposit(string userName, decimal amount)
        {
            Option<IAccount> s = UserRepository
                .TryFind(userName)
                .Map(FindAccount);
            if (s is Some<IAccount> some)
            {
                some.Content.Deposit(amount);
            }
        }
    }
}