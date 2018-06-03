using CodingHelmet.SampleApp.Domain.Interfaces;
using CodingHelmet.SampleApp.Domain.ViewModels;

namespace CodingHelmet.SampleApp.Domain.Models
{
    class AnonymousBuyer : IUser
    {
        public string DisplayName { get; } = "anonymous buyer";

        public IReceipt Purchase(IProduct item)
        {
            return new Receipt(DisplayName, item.Name, item.Price);
        }
    }
}