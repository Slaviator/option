using CodingHelmet.SampleApp.Domain.Interfaces;

namespace CodingHelmet.SampleApp.Domain.ViewModels
{
    class Receipt : IReceipt
    {
        public string Buyer { get; }
        public string ItemName { get; }
        public decimal Price { get; }

        public Receipt(string buyer, string itemName, decimal price)
        {
            Buyer = buyer;
            ItemName = itemName;
            Price = price;
        }

        public string Render()
        {
            return $"{Buyer} -> {ItemName} {Price:C}";
        }
    }
}