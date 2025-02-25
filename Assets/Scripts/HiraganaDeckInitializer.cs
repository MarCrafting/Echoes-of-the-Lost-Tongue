using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public static class HiraganaDeckInitializer
{
    private static List<HiraganaCard> DeckCards = new();

    //Centralized media path for audio playback.
    public static string MediaFolderPath = @"C:\Users\AVI\AppData\LocalLow\DefaultCompany\Echoes of the Lost Tongue\AnkiMedia";

    public static async Task InitializeDeckAsync(string deckName)
    {
        try
        {
            Debug.Log($"Fetching cards from deck: {deckName}");

            var query = "deck:*Hiragana";
            var cardIdsJson = await AnkiAPIHelper.InvokeAsync("findCards", new { query });

            var cardIdList = JsonConvert.DeserializeObject<List<long>>(cardIdsJson.ToString());

            if (cardIdList == null || cardIdList.Count == 0)
            {
                Debug.LogError("No valid card IDs returned.");
                return;
            }

            var notesInfoJson = await AnkiAPIHelper.InvokeAsync("cardsInfo", new { cards = cardIdList });
            var notesInfo = JsonConvert.DeserializeObject<List<dynamic>>(notesInfoJson.ToString());

            DeckCards.Clear();

            foreach (var note in notesInfo)
            {
                var fields = note.fields;

                DeckCards.Add(new HiraganaCard
                {
                    Spellings = fields.ContainsKey("Spellings") ? fields.Spellings.value ?? "N/A" : "N/A",
                    ExampleSentence = fields.ContainsKey("Example Sentence") ? fields["Example Sentence"].value ?? "N/A" : "N/A",
                    SentenceTranslation = fields.ContainsKey("Sentence Translation") ? fields["Sentence Translation"].value ?? "N/A" : "N/A",

                    //Check if "word_audio" field exists before accessing it
                    WordAudio = fields.ContainsKey("word_audio") && fields.word_audio != null && fields.word_audio.value != null
                        ? fields.word_audio.value.ToString().Replace("[sound:", "").Replace("]", "").Trim()
                        : "N/A"
                });
            }

            Debug.Log($"Loaded {DeckCards.Count} cards from '{deckName}'");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error initializing deck: {e.Message}");
        }
    }

    public static List<HiraganaCard> GetDeckCards()
    {
        return DeckCards;
    }
}

// Represents a single Hiragana flashcard.
public class HiraganaCard
{
    public string Spellings { get; set; }
    public string ExampleSentence { get; set; }
    public string SentenceTranslation { get; set; }
    public string WordAudio { get; set; }
}