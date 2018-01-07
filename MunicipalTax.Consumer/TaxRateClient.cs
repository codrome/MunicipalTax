using MunicpalTax.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MunicipalTax.Consumer
{
    public class TaxRateClient
    {
        private readonly string _baseAddress;
        private readonly HttpClient _client;

        public TaxRateClient(string baseAddress)
        {
            _baseAddress = baseAddress;
            _client = new HttpClient();
        }


        public async Task<double> GetTaxRateAsync(string municipality, DateTime date)
        {
            Console.WriteLine($">> Getting effective tax rate for municipality={municipality} and date={date}...");

            var response = await _client.GetAsync(_baseAddress + $"api/TaxRate?municipality={municipality}&date={date}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<double>();
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return await Task.FromResult(0);
        }

        public async Task<IEnumerable<TaxRate>> GetAllTaxRatesAsync()
        {
            Console.WriteLine(">> Getting all tax rates from service.");

            var response = await _client.GetAsync(_baseAddress + "api/TaxRates");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<TaxRate>>();
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return null;
        }

        public async Task<TaxRate> GetTaxRateByIdAsync(Guid id)
        {
            Console.WriteLine($">> Getting tax rate id={id} from service.");

            var requestString = _baseAddress + "api/TaxRate/" + id.ToString();
            HttpResponseMessage response = await _client.GetAsync(requestString);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<TaxRate>();
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return null;
        }

        public async Task<TaxRate> AddTaxRateAsync(TaxRate taxRate)
        {
            Console.WriteLine($">> Adding tax rate id={taxRate.Id} with service.");

            var response = await _client.PostAsXmlAsync(_baseAddress + "api/TaxRate", taxRate);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<TaxRate>();
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return null;
        }

        public async Task<TaxRate> UpdateTaxRateAsync(TaxRate taxRate)
        {
            Console.WriteLine($">> Updating tax rate id={taxRate.Id} with service.");

            var response = await _client.PutAsXmlAsync(_baseAddress + $"api/TaxRate/{taxRate.Id.ToString()}", taxRate);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<TaxRate>();
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return null;
        }

        public async void DeleteTaxRateAsync(Guid id)
        {
            Console.WriteLine($">> Deleting tax rate id={id} on service.");

            var response = await _client.DeleteAsync(_baseAddress + $"api/TaxRate/{id.ToString()}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}
