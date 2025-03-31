using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using System.Text.Json.Nodes;
using System.Text;
using Microsoft.Extensions.Configuration;
using Modules.Bookings.Data;
using Modules.Bookings.Common.Helpers;

namespace Modules.Bookings.Features.Payment.Klarna.OrderConfirmation;

public class OrderConfirmationHandler(ILogger<OrderConfirmationHandler> logger, IConfiguration configuration, HttpClient httpClient, BookingDbContext dbContext)
{
    private readonly ILogger<OrderConfirmationHandler> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _httpClient = httpClient;
    private readonly BookingDbContext _dbContext = dbContext;
    public async Task<OrderConfirmationResponse> Handle(HttpContext context, CancellationToken cancellationToken)
    {
        string klarnaOrderId = context.Request.Query["order_id"]!;

        if(string.IsNullOrEmpty(klarnaOrderId))
            throw new BadRequestException("Missing order_id in query parameters");

        _logger.LogInformation("Received Klarna Order Confirmation for Order ID: {OrderId}", klarnaOrderId);

        var klarnaOrdermanagementUrl = _configuration["Klarna:OrdermanagementUrl"];

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{klarnaOrdermanagementUrl}/{klarnaOrderId}");
        requestMessage.Headers.Authorization = KlarnaAuthHelper.GetAuthHeader(_configuration);

        var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseJson = JsonNode.Parse(responseContent)!;

        _logger.LogInformation("Klarna API Response: {Response}", responseContent);

        string bookingId = responseJson["order_lines"]?[0]?["reference"]?.ToString()!;
        if (string.IsNullOrEmpty(bookingId))
            throw new BadRequestException("Booking ID cannot be null or empty");

        var booking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == Guid.Parse(bookingId), cancellationToken);
        if (booking is null)
            throw new NotFoundException($"Booking with ID {bookingId} not found.");

        booking.ConfirmPayment();
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Payment confirmed for Booking ID: {BookingId}", bookingId);

        var acknowledgeRequest = new HttpRequestMessage(HttpMethod.Post, $"{klarnaOrdermanagementUrl}/{klarnaOrderId}/acknowledge");
        acknowledgeRequest.Headers.Authorization = KlarnaAuthHelper.GetAuthHeader(_configuration);
        acknowledgeRequest.Content = new StringContent("{}", Encoding.UTF8, "application/json");

        var acknowledgeResponse = await _httpClient.SendAsync(acknowledgeRequest, cancellationToken);
        acknowledgeResponse.EnsureSuccessStatusCode();
        _logger.LogInformation("Acknowledged order {OrderId} to Klarna", klarnaOrderId);

        return new OrderConfirmationResponse("Order confirmed and acknowledged.");
    }
}
