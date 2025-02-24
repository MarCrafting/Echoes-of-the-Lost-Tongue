using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HiraganaSpellingDisplay : MonoBehaviour
{
    //Text boxes, Buttons, etc. here:
    public TextMeshProUGUI spellingText;
    public Button playAudioButton;
    public AudioSource audioSource;

    private List<HiraganaCard> deckCards = new List<HiraganaCard>();
    private int currentCardIndex = 0;
    private string lastPlayedAudioPath = "";

    private async void Start()
    {
        // Initialize deck and load cards
        await HiraganaDeckInitializer.InitializeDeckAsync("Japanese Phonetic Writing System - Free Refold Deck::Refold Hiragana");
        deckCards = HiraganaDeckInitializer.GetDeckCards();

        if (deckCards.Count > 0)
        {
            DisplayCurrentCard();
        }

        // Assign button click event to play audio
        playAudioButton.onClick.AddListener(PlaySpellingAudio);
    }

    private void DisplayCurrentCard()
    {
        if (deckCards.Count == 0) return;

        var card = deckCards[currentCardIndex];
        spellingText.text = card.Spellings;
    }

    public void PlaySpellingAudio()
    {
        if (deckCards.Count == 0) return;

        var card = deckCards[currentCardIndex];
        if (string.IsNullOrEmpty(card.WordAudio) || card.WordAudio == "N/A") return;

        // Force Unity to use the correct media path
        string filePath = Path.Combine(HiraganaDeckInitializer.MediaFolderPath, card.WordAudio);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"Audio file not found: {filePath}");
            return;
        }

        lastPlayedAudioPath = filePath;

        // Play audio using UnityWebRequest
        StartCoroutine(PlayAudioClip(filePath));
    }

    private IEnumerator PlayAudioClip(string filePath)
    {
        // Ensure the file path is correctly formatted for Unity
        string url = "file:///" + filePath.Replace("\\", "/");

        using (var www = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError ||
                www.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Audio file failed to load: {www.error}");
            }
            else
            {
                var clip = UnityEngine.Networking.DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }
}
