using System.Net.Http.Json;

namespace Howest.lab2.ex04_sensor_monitoring.Services;

public interface ISmsService
{
    Task SendSmsAsync(string phoneNumber, string message);
}

public class SmsService : ISmsService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SmsService> _logger;

    public SmsService(HttpClient httpClient, ILogger<SmsService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            var payload = new { PhoneNumber = phoneNumber, Message = message };
            var response = await _httpClient.PostAsJsonAsync("api/SMS/send", payload);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("SMS sent successfully to {PhoneNumber}", phoneNumber);
            }
            else
            {
                _logger.LogWarning("Failed to send SMS to {PhoneNumber}. Status: {Status}", phoneNumber, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending SMS to {PhoneNumber}", phoneNumber);
        }
    }
}
