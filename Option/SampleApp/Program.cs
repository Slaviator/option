﻿using System;
using System.IO;
using System.Text;
using CodingHelmet.SampleApp.Application;
using CodingHelmet.SampleApp.Presentation;

namespace CodingHelmet.SampleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DemoRunner("Purchase demo", DemoPurchase);
            DemoRunner("Affiliate demo", DemoReferral);
            DemoRunner("Site maintenance demo", DemoMaintenance);
        }

        private static void DemoRunner(string title, Action demo)
        {
            PrintTitle(title);

            try
            {
                demo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Pause();
        }

        private static void PrintTitle(string title)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine();
            Console.WriteLine($"{title.ToUpper()}");
            Console.WriteLine($"{new string('-', title.Length)}");
            Console.WriteLine();
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press ENTER to continue... ");
            Console.ReadLine();
            Console.Clear();
        }

        private static void DemoPurchase()
        {
            ApplicationServices app = new ApplicationServices();

            string userName = "Jack";

            app.RegisterUser(userName);
            app.Deposit(150);

            Purchase(app, "book");
            Purchase(app, "book");
            Purchase(app, "parachute");
            Purchase(app, "book");
            Purchase(app, "book");

            AnonymousPurchase(app, "book");
            AnonymousPurchase(app, "parachute");
        }

        private static void DemoReferral()
        {
            ApplicationServices app = new ApplicationServices();

            string userName = "Jack";

            app.RegisterUser(userName);
            app.Deposit(200);
            Purchase(app, "book");

            app.RegisterUser("Joe", userName);

            app.Login(userName);
            Purchase(app, "book");

            app.RegisterUser("Jill", userName);

            app.Login(userName);
            Purchase(app, "book");
        }

        private static void DemoMaintenance()
        {
            ApplicationServices app = new ApplicationServices();

            string userName = "Jack";

            app.RegisterUser(userName);
            app.Deposit(150);
            Purchase(app, "book");

            using (File.Create("maintenance.lock"))
            {
            }

            Purchase(app, "book");

            File.Delete("maintenance.lock");
            Purchase(app, "book");
        }

        private static void Purchase(ApplicationServices app, string item)
        {
            IPurchaseViewModel receipt = app.Purchase(item);
            PrintReceipt(receipt);
        }

        private static void AnonymousPurchase(ApplicationServices app, string item)
        {
            IPurchaseViewModel receipt = app.AnonymousPurchase(item);
            PrintReceipt(receipt);
        }

        private static void PrintReceipt(IPurchaseViewModel receipt)
        {
            Console.WriteLine(receipt.Render());
        }
    }
}