using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class LogicScript : MonoBehaviour
{
    public AnkiRequest request;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(GetRequest("http://localhost:8765"));
        
        createDeck("TestDeck");
        // deleteDecks(new string[] {"TestDeck"});

        StartCoroutine(PostRequest("http://localhost:8765", JsonConvert.SerializeObject(request)));
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    IEnumerator PostRequest(string uri, string json)
    {
        UnityWebRequest uwr = new UnityWebRequest(uri, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    public void createDeck(string name)
    {
        request.action = "createDeck";
        request.@params = new CreateDeck {
            deck = name
        };
    }

    public void deleteDecks(string[] names)
    {
        request.action = "deleteDecks";
        request.@params = new DeleteDecks {
            decks = names, cardsToo = true
        };
    }
}

// Base class for different types of parameters
[System.Serializable]
public class AnkiRequest
{
    public string action;
    public Params @params;  // Use the base class/interface type
    public int version = 6;
}

// Base class for parameters
[System.Serializable]
public class Params
{
    // This is just a base class. It can be empty or contain common functionality if needed.
}

// Derived class for CreateDeck
[System.Serializable]
public class CreateDeck : Params
{
    public string deck;
}

// Derived class for DeleteDecks
[System.Serializable]
public class DeleteDecks : Params
{
    public string[] decks;
    public bool cardsToo;
}