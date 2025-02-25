using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HiraganaDisplay : MonoBehaviour
{
    public TextMeshProUGUI spellingText; // Displays the Hiragana spelling
    public TextMeshProUGUI exampleSentenceText; // Displays the Example Sentence

    public Button playSpellingAudioButton; // Button to play the spelling audio
    public Button playSentenceAudioButton; // Button to play the example sentence audio

    public AudioSource audioSource;

    private List<HiraganaCard> deckCards;
    private int currentCardIndex = 0;
    private string lastPlayedAudioPath = "";

    private async void Start()
    {
        Debug.Log("Initializing Hiragana deck...");

        await HiraganaDeckInitializer.InitializeDeckAsync("Japanese Phonetic Writing System - Free Refold Deck::Refold Hiragana");

        deckCards = HiraganaDeckInitializer.GetDeckCards();

        if (deckCards == null || deckCards.Count == 0)
        {
            Debug.LogError("Deck is empty or not initialized. Check Anki response.");
            return;
        }

        Debug.Log($"Deck successfully loaded! {deckCards.Count} cards found.");
        DisplayCurrentCard();

        //Assign Buttons
        playSpellingAudioButton.onClick.AddListener(PlaySpellingAudio);
        playSentenceAudioButton.onClick.AddListener(PlaySentenceAudio);
    }

    private void DisplayCurrentCard()
    {
        if (deckCards.Count == 0) return;

        var card = deckCards[currentCardIndex];

        // Display Spelling
        spellingText.text = card.Spellings;

        // Display Example Sentence
        exampleSentenceText.text = card.ExampleSentence;
    }

    public void PlaySpellingAudio()
    {
        Debug.Log("Playing Spelling Audio...");
        PlayAudio(deckCards[currentCardIndex].WordAudio);
    }

    public void PlaySentenceAudio()
    {
        var card = deckCards[currentCardIndex];

        if (!string.IsNullOrEmpty(card.SentenceAudio) && card.SentenceAudio != "N/A")
        {
            PlayAudio(card.SentenceAudio);  //Example sentence audio
        }
        else
        {
            Debug.LogError("No example sentence audio available for this card.");
        }
    }

    private void PlayAudio(string audioFile)
    {
        if (string.IsNullOrEmpty(audioFile) || audioFile == "N/A")
        {
            Debug.LogWarning("No valid audio file to play.");
            return;
        }

        string filePath = Path.Combine(HiraganaDeckInitializer.MediaFolderPath, audioFile);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"Audio file not found: {filePath}");
            return;
        }

        lastPlayedAudioPath = filePath;
        StartCoroutine(PlayAudioClip(filePath));
    }

    private IEnumerator PlayAudioClip(string filePath)
    {
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
