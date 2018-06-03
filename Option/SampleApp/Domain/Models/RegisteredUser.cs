using System.Collections.Generic;
using CodingHelmet.SampleApp.Domain.Interfaces;
using CodingHelmet.SampleApp.Domain.ViewModels;

namespace CodingHelmet.SampleApp.Domain.Models
{
    class RegisteredUser : IUser
    {
        public string UserName { get; }
        public string DisplayName => UserName;
        private decimal _totalPurchases;
        private bool _hasReceivedLoyaltyDiscount;
        private IList<RelativeDiscount> Discounts { get; } = new List<RelativeDiscount>();

        public RegisteredUser(string userName)
        {
            UserName = userName;
        }

        public IReceipt Purchase(IProduct item)
        {
            IProduct discountedItem = item.ApplyDiscounts(Discounts);
            RegisterPurchase(discountedItem.Price);
            return new Receipt(UserName, discountedItem.Name, discountedItem.Price);
        }

        public void SetReferrer(RegisteredUser referrer)
        {
            referrer?.ReferralAdded();
        }

        private void RegisterPurchase(decimal price)
        {
            _totalPurchases += price;
            if (!_hasReceivedLoyaltyDiscount && _totalPurchases > 1000.0M)
            {
                Discounts.Add(new RelativeDiscount(0.05M));
                _hasReceivedLoyaltyDiscount = true;
            }
        }

        private void ReferralAdded()
        {
            Discounts.Add(new RelativeDiscount(.02M));
        }
    }
}