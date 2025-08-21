using Newtonsoft.Json;
using Nop.Services.Localization;
using Nop.Services.Logging;
using RestSharp;

namespace Nop.Plugin.Widgets.AskVendor.Service
{
    public class GptService
    {
        private readonly ILogger _logger;
        private readonly ILocalizationService _localizationService;
         public GptService(ILogger logger, ILocalizationService localizationService)
        {
            _logger = logger;
            _localizationService = localizationService;
        }
        public async Task<string> Answer(string message, string apiKey, string client)
        {
            try
            {
                RestClient _client = new RestClient(client);
                // Create a new POST request
                var request = new RestRequest(client, Method.POST);
                // Set the Content-Type header
                request.AddHeader("Content-Type", "application/json");
                // Set the Authorization header with the API key
                request.AddHeader("Authorization", $"Bearer {apiKey}");

                // Create the request body with the message and other parameters
                var requestBody = new
                {
                    prompt = message,
                    max_tokens = 150,
                    n = 1,
                    stop = (string?)null,
                    temperature = 0.7,
                };

                // Add the JSON body to the request
                request.AddJsonBody(JsonConvert.SerializeObject(requestBody));
                // Execute the request and receive the response
                var response = _client.Execute(request);
                // Deserialize the response JSON content
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content ?? string.Empty);
                // Extract and return the chatbot's response text
                string result = jsonResponse?.choices[0]?.text?.ToString()?.Trim() ?? string.Empty;
                return result;
            }catch(Exception ex){
                _logger.InsertLog(Core.Domain.Logging.LogLevel.Information, ex.Message, ex.StackTrace);
                var errorMessage = await _localizationService.GetResourceAsync("Plugin.Widgets.AskVendor.Api.Error");
                return errorMessage;
            }
        }         
    }
}

