using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Modules.Bookings.Data;
using Shared.Exceptions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace Modules.Bookings.Features.Payment.Klarna.GetOrder;

public class GetOrderHandler(HttpClient httpClient, BookingDbContext dbContext, IConfiguration configuration, ILogger<GetOrderHandler> logger)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly BookingDbContext _dbContext = dbContext;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<GetOrderHandler> _logger = logger;

    public async Task<GetOrderResponse> Handle(GetOrderRequest request, CancellationToken cancellationToken)
    {
        var klarnaApiUrl = _configuration["Klarna:CheckoutUrl"];
        var username = _configuration["Klarna:Username"];
        var password = _configuration["Klarna:Password"];

        var klarnaAuth = Encoding.ASCII.GetBytes($"{username}:{password}");

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{klarnaApiUrl}/{request.Id}");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(klarnaAuth));

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseJson = JsonNode.Parse(responseContent)!;

        _logger.LogInformation("Klarna API Response: {Response}", responseContent);

        string orderId = responseJson["order_id"]?.ToString()!;
        string status = responseJson["status"]?.ToString()!;
        string bookingId = responseJson["order_lines"]?[0]?["reference"]?.ToString()!;
        string htmlSnippet = responseJson["html_snippet"]?.ToString()!;

        if (string.IsNullOrEmpty(bookingId))
            throw new BadRequestException("Booking ID cannot be null or empty");

        var booking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == Guid.Parse(bookingId), cancellationToken);
        if (booking is null)
            throw new NotFoundException($"Booking with ID {bookingId} not found.");

        if (status is "checkout_complete")
        {
            booking.ConfirmPayment();
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return new GetOrderResponse(orderId, status, htmlSnippet);
    }
}
