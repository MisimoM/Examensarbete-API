using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using System.Text;

namespace Modules.Bookings.Features.Payment.Klarna.CreateOrder;

public class CreateOrderHandler(HttpClient httpClient, IConfiguration configuration, ILogger<CreateOrderHandler> logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<CreateOrderHandler> _logger = logger;

    public async Task<CreateOrderResponse> Handle(CreateOrderRequest request)
    {
        var klarnaApiUrl = _configuration["Klarna:CheckoutUrl"];
        var username = _configuration["Klarna:Username"];
        var password = _configuration["Klarna:Password"];


        var klarnaPayload = new
        {
            purchase_country = "SE",
            purchase_currency = "SEK",
            locale = "sv-SE",
            order_amount = request.PricePerNight * request.NumberOfNights * 100,
            order_tax_amount = 0,
            order_lines = new[]
            {
                new
                {
                    reference = request.BookingId,
                    name = request.BookingId,
                    quantity = request.NumberOfNights,
                    quantity_unit = "Nights",
                    unit_price = request.PricePerNight * 100,
                    tax_rate = 0,
                    total_amount = (request.PricePerNight * request.NumberOfNights) * 100,
                    total_tax_amount = 0
                }
            },
            merchant_urls = new
            {
                terms = "frontendUrl/terms",
                checkout = "frontendUrl/checkout",
                confirmation = "frontendUrl/confirmation?order_id={checkout.order.id}",
                push = "backendUrl/callback"
            }
        };

        var klarnaAuth = Encoding.ASCII.GetBytes($"{username}:{password}");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, klarnaApiUrl)
        {
            Content = new StringContent(JsonConvert.SerializeObject(klarnaPayload), Encoding.UTF8, "application/json")
        };
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(klarnaAuth));

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseJson = JsonNode.Parse(responseContent)!;

        _logger.LogInformation("Klarna API Response: {Response}", responseContent);


        string orderId = responseJson["order_id"]?.ToString()!;
        string status = responseJson["status"]?.ToString()!;
        string htmlSnippet = responseJson["html_snippet"]?.ToString()!;

        return new CreateOrderResponse(orderId, status, htmlSnippet);
    }
}
