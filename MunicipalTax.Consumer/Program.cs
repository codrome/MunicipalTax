using MunicipalTax.Service;
using MunicpalTax.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalTax.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Do you want to run tests (yes/no)?");
            string runTests = Console.ReadLine();
            if (string.IsNullOrEmpty(runTests) || string.Equals(runTests, "yes", StringComparison.OrdinalIgnoreCase))
            {
                TestTaxRateClient().Wait();
                Console.WriteLine("Hit <Enter> to continue.");
                Console.ReadLine();
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Enter municipality name (empty to finish):");
                string municipality = Console.ReadLine();
                if (string.IsNullOrEmpty(municipality))
                    return;

                Console.WriteLine("Enter date (empty to finish):");
                string dateString = Console.ReadLine();
                if (string.IsNullOrEmpty(dateString))
                    return;

                var date = DateTime.Parse(dateString);

                Console.WriteLine(GetTaxRateAsync(municipality, date));
            }
        }

        private static async Task TestTaxRateClient()
        {
            var client = new TaxRateClient("http://localhost:9000/");

            //test get all rates
            var taxRates = await client.GetAllTaxRatesAsync();
            foreach (var taxRate in taxRates)
            {
                DumpTaxRate(taxRate);
            }

            //test get taxrate by id
            if (taxRates.Any())
            {
                DumpTaxRate(await client.GetTaxRateByIdAsync(taxRates.First().Id));
            }

            //test get affective taxrate
            Console.WriteLine(await client.GetTaxRateAsync("Vilnius", new DateTime(2017, 1, 1))); // yearly task
            Console.WriteLine(await client.GetTaxRateAsync("Kaunas", new DateTime(2018, 1, 1))); // yearly task
            Console.WriteLine(await client.GetTaxRateAsync("Kaunas", new DateTime(2019, 10, 1))); // not found
            Console.WriteLine(await client.GetTaxRateAsync("Vilnius", new DateTime(2017, 3, 15))); // monthly tax
            Console.WriteLine(await client.GetTaxRateAsync("Vilnius", new DateTime(2017, 1, 25))); // weekly tax
            Console.WriteLine(await client.GetTaxRateAsync("Vilnius", new DateTime(2017, 4, 1))); // daily tax

            //test add taxrate
            TaxRate newTaxRate = new TaxRate()
            {
                Id = Guid.NewGuid(),
                Name = "2017-D62",
                MunicipalityName = "Vilnius",
                StartDate = new DateTime(2017, 3, 4),
                EndDate = new DateTime(2017, 3, 4),
                RateType = (int)RateType.Daily,
                Rate = 0.33
            };

            var taxRateInserted = await client.AddTaxRateAsync(newTaxRate);
            if (taxRateInserted == null)
                return;

            DumpTaxRate(taxRateInserted);

            //test update taxrate
            newTaxRate.Name = "2017-D63";
            newTaxRate.MunicipalityName = "Kaunas";
            newTaxRate.StartDate = new DateTime(2017, 3, 5);
            newTaxRate.EndDate = new DateTime(2017, 3, 5);
            newTaxRate.RateType = (int)RateType.Daily;
            newTaxRate.Rate = 0.35;

            var taxRateUpdated = await client.UpdateTaxRateAsync(newTaxRate);
            if (taxRateUpdated == null)
                return;

            DumpTaxRate(taxRateUpdated);

            //test delete taxrate
            client.DeleteTaxRateAsync(newTaxRate.Id);
            await client.GetTaxRateByIdAsync(newTaxRate.Id); // not found
        }

        private static double GetTaxRateAsync(string municipality, DateTime date)
        {
            var client = new TaxRateClient("http://localhost:9000/");

            return client.GetTaxRateAsync(municipality, date).Result;
        }

        private static void DumpTaxRate(TaxRate taxRate)
        {
            if (taxRate == null)
                return;

            Console.WriteLine("------");

            Console.WriteLine($"  Id: {taxRate.Id}");
            Console.WriteLine($"  Name: {taxRate.Name}");
            Console.WriteLine($"  MunicipalityName: {taxRate.MunicipalityName}");
            Console.WriteLine($"  RateType: {taxRate.RateType}");
            Console.WriteLine($"  StartDate: {taxRate.StartDate}");
            Console.WriteLine($"  EndDate: {taxRate.EndDate}");
            Console.WriteLine($"  Rate: {taxRate.Rate}");
        }
    }
}
