using CodingHelmet.SampleApp.Presentation;

namespace CodingHelmet.SampleApp.Domain.ViewModels
{
    class FailedPurchase : IPurchaseViewModel
    {
        private static FailedPurchase _instance;

        public static FailedPurchase Instance => _instance ?? (_instance = new FailedPurchase());

        private FailedPurchase()
        {
        }

        public string Render()
        {
            return "Purchase failed.";
        }
    }
}