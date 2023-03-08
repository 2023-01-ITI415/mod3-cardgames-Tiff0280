using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class stores information about each decorator or pip from JSON_Deck
[System.Serializable]                                                       
public class JsonPip
{                                                       
    public string type = "pip";  // "pip", "letter", or "suit"
    public Vector3 loc;           // Location of the Sprite on the Card
    public bool flip = false;  // True to flip the Sprite vertically
    public float scale = 1;     // The scale of the Sprite
}

// This class stores information for each rank of card
[System.Serializable]
public class JsonCard
{                                                      
    public int rank;          // The rank (1-13) of this card
    public string face;          // Sprite to use for each face card
    public List<JsonPip> pips = new List<JsonPip>(); // The pips on this card
}

// This class contains information about the entire deck
[System.Serializable]
public class JsonDeck
{                                                     
    public List<JsonPip> decorators = new List<JsonPip>();
    public List<JsonCard> cards = new List<JsonCard>();
}

public class JsonParseDeck : MonoBehaviour
{
    [Header("Inscribed")]
    public TextAsset jsonDeckFile;  // Reference to the JSON_Deck text file 

    [Header("Dynamic")]
    public JsonDeck deck;

    void Awake()
    {
        deck = JsonUtility.FromJson<JsonDeck>(jsonDeckFile.text);      
    }
}
