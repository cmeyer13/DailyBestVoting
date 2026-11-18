using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var emails = configuration.GetSection("Emails").Get<List<string>>();
        var accessToken = configuration["AccessToken"];
        var apiKey = configuration["ApiKey"];
        var fingerprint = configuration["Fingerprint"];

        var logger = new StreamWriter("log.txt", true);
        logger.AutoFlush = true;

        var httpClient = new HttpClient();
        var headers = new Dictionary<string, string>
        {
            {"Content-Type", "application/json; charset=UTF-8"},
            {"Authorization", $"Bearer {accessToken}"},
            {"X-Api-Key", apiKey},
            {"X-Fingerprint", fingerprint},
            {"X-Organization-Id", "1"},
            {"X-Organization-Promotion-Id", "1176068"},
            {"X-Promotion-Id", "1007522"}
        };

        var timer = new System.Timers.Timer(24 * 60 * 60 * 100, Vote);
        timer.Elapsed += async (sender, e) => await Vote(emails, httpClient, headers, logger);
        timer.Start();

        Console.WriteLine("Voting application started. Press Enter to exit.");
        Console.ReadLine();
    }

    private static async Task Vote(List<string> emails, HttpClient httpClient, Dictionary<string, string> headers, StreamWriter logger)
    {
        var jsonBody = new
        {
            votes = new[]
            {
                new
                {
                    matchup_entry_id = 560806662,
                    matchup_id = 6099977,
                    organization_promotion_id = 1176068,
                    promotion_id = 1007522
                }
            }
        };

        var random = new Random();

        foreach (var email in emails)
        {
            try
            {
                // Add a small delay between requests to avoid rate limiting
                await Task.Delay(random.Next(500, 3000));

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "https://embed-1176068.secondstreetapp.com/api/votes/950009314"))
                {
                    foreach (var header in headers)
                    {
                        requestMessage.Headers.Add(header.Key, header.Value);
                    }

                    var response = await httpClient.SendAsync(requestMessage);

                    if (response.IsSuccessStatusCode)
                    {
                        logger.WriteLine($"Vote deleted successfully using email: {email}, Status Code: {response.StatusCode}");
                    }
                    else
                    {
                        logger.WriteLine($"Vote deletion failed using email: {email}, Status Code: {response.StatusCode}, Reason: {await response.Content.ReadAsStringAsync()}");
                    }
                }

                // Add a small delay between requests to avoid rate limiting
                await Task.Delay(random.Next(500, 3000));

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://embed-1176068.secondstreetapp.com/api/votes"))
                {
                    foreach (var header in headers)
                    {
                        requestMessage.Headers.Add(header.Key, header.Value);
                    }

                    var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(jsonBody), System.Text.Encoding.UTF8, "application/json");
                    requestMessage.Content = jsonContent;

                    var response = await httpClient.SendAsync(requestMessage);

                    if (response.IsSuccessStatusCode)
                    {
                        logger.WriteLine($"Vote successful using email: {email}, Status Code: {response.StatusCode}");
                    }
                    else
                    {
                        logger.WriteLine($"Vote failed using email: {email}, Status Code: {response.StatusCode}, Reason: {await response.Content.ReadAsStringAsync()}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteLine($"Error voting using email: {email}, Message: {ex.Message}, Stack Trace: {ex.StackTrace}");
            }
        }
    }
}
