using DTO;
using Microsoft.Extensions.Configuration;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class PayPalService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _baseUrl;
        private readonly IProductsReposetory _productsReposetory;

        public PayPalService(HttpClient httpClient, IConfiguration config, IProductsReposetory productsReposetory)
        {
            _httpClient = httpClient;
            _clientId = config["PayPal:ClientId"];
            _clientSecret = config["PayPal:ClientSecret"];
            _baseUrl = config["PayPal:BaseUrl"];
            _productsReposetory = productsReposetory;
        }

        private async Task<string> GetAccessToken()
        {
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
            var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            var response = await _httpClient.PostAsync($"{_baseUrl}/v1/oauth2/token", content);
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("access_token").GetString();
        }

        // Validates the client's claimed amount against DB prices, then creates the PayPal order.
        public async Task<Resulte<string>> CreateVerifiedOrder(decimal clientAmount, string currency, List<AddToCartDTO> products)
        {
            double dbSum = 0;
            foreach (var item in products)
            {
                var product = await _productsReposetory.GetByIDProductsReposetory(item.ProductsID);
                if (product == null)
                    return Resulte<string>.Failure($"Product {item.ProductsID} not found");
                dbSum += product.Price;
            }

            // Reject if client amount differs by more than 1 cent (float rounding tolerance)
            if (Math.Abs(dbSum - (double)clientAmount) > 0.01)
                return Resulte<string>.Failure($"Amount mismatch: expected {dbSum:F2}, received {clientAmount:F2}");

            var paypalOrderId = await CreateOrder((decimal)dbSum, currency);
            return Resulte<string>.Success(paypalOrderId);
        }

        private async Task<string> CreateOrder(decimal amount, string currency)
        {
            var token = await GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var orderRequest = new
            {
                intent = "CAPTURE",
                purchase_units = new[] { new { amount = new { currency_code = currency, value = amount.ToString("F2") } } }
            };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/v2/checkout/orders", orderRequest);
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("id").GetString(); // מחזיר רק את ה-ID
        }

        public async Task<(bool Success, string? ErrorMessage)> CaptureOrder(string orderId)
        {
            var token = await GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // PayPal requires Content-Type: application/json even with an empty body
            var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/v2/checkout/orders/{orderId}/capture", emptyContent);

            if (response.IsSuccessStatusCode)
                return (true, null);

            var errorBody = await response.Content.ReadAsStringAsync();
            return (false, errorBody);
        }
    }
}
