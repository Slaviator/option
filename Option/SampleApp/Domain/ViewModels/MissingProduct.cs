using CodingHelmet.SampleApp.Presentation;

namespace CodingHelmet.SampleApp.Domain.ViewModels
{
    class MissingProduct : IPurchaseViewModel
    {
        private string ProductName { get; }

        public MissingProduct(string productName)
        {
            ProductName = productName;
        }

        public string Render()
        {
            return $"Product {ProductName} is not available.";
        }
    }
}