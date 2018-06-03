using CodingHelmet.Optional;
using CodingHelmet.SampleApp.Domain.Models;

namespace CodingHelmet.SampleApp.Infrastructure
{
    class ProductRepository
    {
        public Option<Product> TryFind(string itemName)
        {
            if (itemName.Length >= 9)
                return None.Value;
            return new Product(itemName);
        }
    }
}