using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace dotnetCancelationTokenBff.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ILogger<TransactionController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando");

            var endpoint = "http://localhost:5058/api/Transactions";

            var httpClient = new HttpClient();

            var requestData = new
            {
                ticker = new
                {
                    code = "AAPL",
                    companyName = "Apple Inc"
                },
                amount = 1500.00,
                quantity = 100,
                date = "2023-11-01T00:00:00"
            };

            string json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            string? msg;
            try
            {
                var response = await httpClient.PostAsync(endpoint, content, cancellationToken);

                msg = response.IsSuccessStatusCode
                    ? "Transaction created successfully!"
                    : $"Failed to create transaction. Status code: {response.StatusCode}";
            }
            catch (TaskCanceledException)
            {
                msg = "Request was canceled due to a timeout or cancellation token.";
            }
            catch (HttpRequestException ex)
            {
                msg = $"Request failed: {ex.Message}";
            }

            return Ok(new { msg });

        }
    }
}
