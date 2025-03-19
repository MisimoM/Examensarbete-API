using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Modules.Bookings.Common.Helpers;
using System.Text.Json.Nodes;

namespace Modules.Bookings.Features.Payment.Klarna.GetOrder;

public class GetOrderHandler(HttpClient httpClient, IConfiguration configuration, ILogger<GetOrderHandler> logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<GetOrderHandler> _logger = logger;

    public async Task<GetOrderResponse> Handle(GetOrderRequest request, CancellationToken cancellationToken)
    {
        var klarnaApiUrl = _configuration["Klarna:CheckoutUrl"];

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{klarnaApiUrl}/{request.Id}");
        requestMessage.Headers.Authorization = KlarnaAuthHelper.GetAuthHeader(_configuration);

        var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseJson = JsonNode.Parse(responseContent)!;

        _logger.LogInformation("Klarna API Response: {Response}", responseContent);

        string orderId = responseJson["order_id"]?.ToString()!;
        string status = responseJson["status"]?.ToString()!;
        string htmlSnippet = responseJson["html_snippet"]?.ToString()!;

        return new GetOrderResponse(orderId, status, htmlSnippet);
    }
}
