using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Text;
using Modules.Bookings.Common.Helpers;

namespace Modules.Bookings.Features.Payment.Klarna.CreateOrder;

public class CreateOrderHandler(HttpClient httpClient, IConfiguration configuration, ILogger<CreateOrderHandler> logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<CreateOrderHandler> _logger = logger;

    private readonly string frontendUrl = "";
    private readonly string backendUrl = "";

    public async Task<CreateOrderResponse> Handle(CreateOrderRequest request)
    {
        var klarnaApiUrl = _configuration["Klarna:CheckoutUrl"];

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
                terms = $"{frontendUrl}/terms",
                checkout = $"{frontendUrl}/checkout",
                confirmation = $"{frontendUrl}/confirmation?order_id={{checkout.order.id}}",
                push = $"{backendUrl}/order/confirmation"
            }
        };

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, klarnaApiUrl)
        {
            Content = new StringContent(JsonConvert.SerializeObject(klarnaPayload), Encoding.UTF8, "application/json")
        };
        requestMessage.Headers.Authorization = KlarnaAuthHelper.GetAuthHeader(_configuration);

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
