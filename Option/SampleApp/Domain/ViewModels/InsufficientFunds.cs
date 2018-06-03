using CodingHelmet.SampleApp.Presentation;

namespace CodingHelmet.SampleApp.Domain.ViewModels
{
    class InsufficientFunds : IPurchaseViewModel
    {
        private string UserName { get; }
        private decimal Price { get; }

        public InsufficientFunds(string userName, decimal price)
        {
            UserName = userName;
            Price = price;
        }

        public string Render()
        {
            return $"Dear {UserName}, you don't have {Price:C} available in your account.";
        }
    }
}