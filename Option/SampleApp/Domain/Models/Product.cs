using System.Collections.Generic;
using CodingHelmet.SampleApp.Domain.Interfaces;

namespace CodingHelmet.SampleApp.Domain.Models
{
    class Product : IProduct
    {
        public decimal Price { get; private set; }
        public string Name { get; }

        public Product(string name)
        {
            Name = name;
            Price = (decimal) (name.Length * 10);
        }

        public IProduct ApplyDiscounts(IEnumerable<IDiscount> discounts)
        {
            decimal price = Price;

            foreach (IDiscount discount in discounts)
                price = discount.Apply(price);

            return new Product(Name)
            {
                Price = price
            };
        }
    }
}