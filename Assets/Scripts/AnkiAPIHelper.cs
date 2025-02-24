using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public static class AnkiAPIHelper
{
    // Base URL for AnkiConnect
    private const string Url = "http://localhost:8765";

    // API key for AnkiConnect
    private const string ApiKey = "MySecureApiKey";

    // Reuse a single HttpClient instance for performance
    private static readonly HttpClient Client = new HttpClient();

    // Invoke a request to AnkiConnect
    public static async Task<dynamic> InvokeAsync(string action, object parameters)
    {
        var payload = new
        {
            action,                // Action to perform (e.g., "deckNames", "findCards")
            @params = parameters,  // Parameters for the action
            version = 6,           // AnkiConnect API version
            key = ApiKey           // API key for authentication
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        try
        {
            // Send the HTTP request to AnkiConnect
            var response = await Client.PostAsync(Url, content);
            response.EnsureSuccessStatusCode();

            // Parse the JSON response
            var resultJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(resultJson);

            // Check for errors in the response
            if (result.error != null && result.error.ToString() != "null")
            {
                throw new Exception($"API Error: {result.error}");
            }

            // Return the result from the response
            return result.result;
        }
        catch (HttpRequestException e)
        {
            throw new Exception($"HTTP Request failed: {e.Message}");
        }
        catch (Exception e)
        {
            throw new Exception($"API Error: {e.Message}");
        }
    }

    // Request permission from AnkiConnect to allow API communication
    public static async Task RequestPermission()
    {
        try
        {
            var result = await InvokeAsync("requestPermission", new { });
            Debug.Log("API Permission Granted.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error requesting permission: {e.Message}");
        }
    }

    // Fetch and display the names of all available Anki decks
    public static async Task FetchDeckNames()
    {
        try
        {
            var result = await InvokeAsync("deckNames", new { });

            // Deserialize the deck names
            var decks = JsonConvert.DeserializeObject<string[]>(result.ToString()) ?? Array.Empty<string>();

            Debug.Log("\nAvailable decks:");
            foreach (var deck in decks)
            {
                Debug.Log($"- {deck}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error fetching deck names: {e.Message}");
        }
    }
}
