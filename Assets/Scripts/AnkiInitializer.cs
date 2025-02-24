using UnityEngine;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json; // Use Newtonsoft for JSON parsing

public class AnkiInitializer : MonoBehaviour
{
    // Run once when the game starts
    async void Awake()
    {
        Debug.Log("Initializing AnkiConnect API...");

        // Request API permission
        await AnkiAPIHelper.RequestPermission();

        // Fetch available decks
        await AnkiAPIHelper.FetchDeckNames();

        Debug.Log("AnkiConnect API initialized successfully!");

        //ensure's the API remains throughout the scenes
        DontDestroyOnLoad(gameObject);
    }
}
