using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HiraganaDisplay : MonoBehaviour
{
    public TextMeshProUGUI spellingsText; // Displays the Hiragana Spelling
    public TextMeshProUGUI spellingsTextBack; // Displays the Hiragana Spelling on the back side
    public TextMeshProUGUI pronunciationText; // Displays the Hiragana Pronunciation
    public TextMeshProUGUI pronunciationTextBack; // Displays the Hiragana Pronunciation on the back side
    public TextMeshProUGUI wordText; // Displays the Hiragana Example Word
    public TextMeshProUGUI exampleSentenceText; // Displays the Example Sentence
    
    public Button playSpellingsAudioButton; // Button to play the spellings audio
    public Button playWordAudioButton; // Button to play the word audio
    public Button playSentenceAudioButton; // Button to play the example sentence audio

    public Button playSpellingsAudioButton;
    public Button playWordAudioButton;
    public Button playSentenceAudioButton;

    public AudioSource audioSource;

    private List<HiraganaCard> deckCards;
    public int currentCardIndex = 0;
    private string lastPlayedAudioPath = "";
    private bool isInitialized = false;

    // Called when the object is first created
    void Start()
    {
        // Assign button listeners once
        playSpellingsAudioButton.onClick.AddListener(PlaySpellingsAudio);
        playWordAudioButton.onClick.AddListener(PlayWordAudio);
        playSentenceAudioButton.onClick.AddListener(PlaySentenceAudio);
    }

    // Public method to trigger or re-trigger the lesson display
    public async void DisplayHiragana()
    {
        if (!isInitialized)
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
            isInitialized = true;
        }

        DisplayCurrentCard();
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return; // Only process input when active

        if (Input.GetKeyDown(KeyCode.Space)) NextSection();
        if (Input.GetKeyDown(KeyCode.Backspace)) PreviousSection();
        if (Input.GetKeyDown(KeyCode.R)) ReplayAudio();
        if (Input.GetKeyDown(KeyCode.T)) ToggleTranslation();
    }

    private void DisplayCurrentCard()
    {
        if (deckCards == null || deckCards.Count == 0) return;

        var card = deckCards[currentCardIndex];

        spellingsText.text = card.Spellings;
        spellingsTextBack.text = spellingsText.text;
        pronunciationText.text = card.Pronunciation;
        pronunciationTextBack.text = pronunciationText.text;
        wordText.text = card.ExampleWord;
        exampleSentenceText.text = card.ExampleSentence;
    }

    public void PlayWordAudio()
    {
        PlayAudio(deckCards[currentCardIndex].WordAudio);
    }

    public void PlaySpellingsAudio()
    {
        var card = deckCards[currentCardIndex];
        if (!string.IsNullOrEmpty(card.SpellingsAudio) && card.SpellingsAudio != "N/A")
        {
            PlayAudio(card.SpellingsAudio);
        }
        else
        {
            Debug.LogError("No spellings audio available for this card.");
        }
    }

    public void PlaySentenceAudio()
    {
        var card = deckCards[currentCardIndex];
        if (!string.IsNullOrEmpty(card.SentenceAudio) && card.SentenceAudio != "N/A")
        {
            PlayAudio(card.SentenceAudio);
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

    public void ReplayAudio()
    {
        if (!string.IsNullOrEmpty(lastPlayedAudioPath))
        {
            PlayAudio(lastPlayedAudioPath);
        }
    }

    public void ToggleTranslation()
    {
        if (deckCards == null || deckCards.Count == 0) return;

        var card = deckCards[currentCardIndex];
        // Check if the spellings text is currently in Japanese or English
        bool isCurrentlyJapanese = wordText.text == card.ExampleWord;

        wordText.text = isCurrentlyJapanese ? card.WordTranslation : card.ExampleWord;
        exampleSentenceText.text = isCurrentlyJapanese ? card.SentenceTranslation : card.ExampleSentence;
    }

    public void NextSection()
    {
        currentCardIndex = (currentCardIndex + 1) % deckCards.Count;
        DisplayCurrentCard();
    }

    public void PreviousSection()
    {
        currentCardIndex = Mathf.Max(0, currentCardIndex - 1);
        DisplayCurrentCard();
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