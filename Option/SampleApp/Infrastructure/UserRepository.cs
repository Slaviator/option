using System.Collections.Generic;
using CodingHelmet.Optional;
using CodingHelmet.Optional.Extensions;
using CodingHelmet.SampleApp.Domain.Models;

namespace CodingHelmet.SampleApp.Infrastructure
{
    class UserRepository
    {
        private Dictionary<string, RegisteredUser> UserNameToUser { get; } = new Dictionary<string, RegisteredUser>();

        public void Add(RegisteredUser user)
        {
            UserNameToUser.Add(user.UserName, user);
        }

        public Option<RegisteredUser> TryFind(string userName) =>
            UserNameToUser.TryGetValue(userName);
    }
}