using System;
using System.Collections.Generic;
using System.IO;
using CodingHelmet.SampleApp.Application.ViewModels;
using CodingHelmet.SampleApp.Domain;
using CodingHelmet.SampleApp.Presentation;

namespace CodingHelmet.SampleApp.Application
{
    class ApplicationServices
    {
        private Dictionary<string, string> IisSession { get; } = new Dictionary<string, string>();
        private DomainServices Domain { get; } = new DomainServices();

        public void RegisterUser(string userName) =>
            RegisterUser(userName, () => Domain.RegisterUser(userName));

        public void RegisterUser(string userName, string referrerName) =>
            RegisterUser(userName, () => Domain.RegisterUser(userName, referrerName));

        private void RegisterUser(string userName, Action domainRegister)
        {
            if (IsDownForMaintenance())
                return;

            domainRegister();

            IisSession["logged-in-user"] = userName;
        }

        public void Login(string userName)
        {
            if (IsDownForMaintenance())
                return;

            if (Domain.VerifyCredentials(userName))
                IisSession["logged-in-user"] = userName;
            else
                IisSession["logged-in-user"] = null;
        }

        public IPurchaseViewModel Purchase(string itemName)
        {
            return IsDownForMaintenance() ? new Downtime() : Domain.Purchase(IisSession["logged-in-user"], itemName);
        }

        public IPurchaseViewModel AnonymousPurchase(string itemName)
        {
            return IsDownForMaintenance() ? new Downtime() : Domain.AnonymousPurchase(itemName);
        }

        public void Deposit(decimal amount)
        {
            if (IsDownForMaintenance())
                return;
            Domain.Deposit(IisSession["logged-in-user"], amount);
        }

        private bool IsDownForMaintenance() => File.Exists("maintenance.lock");
    }
}