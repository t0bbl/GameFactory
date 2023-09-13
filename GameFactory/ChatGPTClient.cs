using Newtonsoft.Json;
using RestSharp;

public class ChatGPTClient
{
    private readonly string _apiKey;
    private readonly RestClient _client;

    // Constructor that takes the API key as a parameter
    public ChatGPTClient(string apiKey)
    {
        _apiKey = apiKey;
        // Initialize the RestClient with the ChatGPT API endpoint
        _client = new RestClient("https://api.openai.com/v1/engines/text-davinci-003/completions");
    }

    // We'll add methods here to interact with the API.
    // Method to send a message to the ChatGPT API and return the response
    public string SendMessage(string message)
    {
        // Create a new POST request
        var request = new RestRequest("", Method.Post);
        // Set the Content-Type header
        request.AddHeader("Content-Type", "application/json");
        // Set the Authorization header with the API key
        request.AddHeader("Authorization", $"Bearer {_apiKey}");

        // Create the request body with the message and other parameters
        var requestBody = new
        {
            prompt = message,
            max_tokens = 100,
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
        return jsonResponse?.choices[0]?.text?.ToString()?.Trim() ?? string.Empty;
    }
}