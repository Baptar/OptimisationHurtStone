using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Deck
{
    private string player = Constants.playerName;
    private List<Card> cards = new List<Card>();

    public int Count()
    {
        return cards.Count;
    }

    public bool IsEmpty()
    {
        return this.Count() <= 0;
    }

    public Card Pop()
    {
        if (this.IsEmpty()) return default;
        Card card = cards.First();
        cards.RemoveAt(0);
        return card;
    }

    public Card Seek()
    {
        if (this.IsEmpty()) return default;
        return cards.First();
    }

    public Deck Shuffle()
    {
        cards = cards.OrderBy((Card _) =>
            System.Guid.NewGuid()
        ).ToList();
        return this;
    }
}



public partial class Deck
{
    public Deck Copy()
    {
        return new Deck {
            cards = cards.ToList(),
            player = player
        };
    }

    public static Deck Generate(string player = Constants.playerName)
    {
        // Get Deck Construct to use to generate one
        List<Card> deckConstruct = DeckHelper.PoolToDeckConstruct(CardPool.Get());

        // Generate deck content (WARNING !! Not optimized. Use with caution)
        List<Card> deckResult = new List<Card>();
        for (int i = 0; i < Constants.deckSize; ++i) {
            int id = Random.Range(0, deckConstruct.Count);
            deckResult.Add(deckConstruct[id]);
            deckConstruct.RemoveAt(id);
        }

        // Return result deck
        return new Deck {
            cards = CardPool.Order(deckResult),
            player = player,
        };
    }
}



public partial class Deck
{
    [System.Serializable]
    public struct Json
    {
        public string player;
        public List<Card.Json> deck;
    }


    public Deck.Json ToSerializable()
    {
        return new Deck.Json {
            player = player,
            deck = cards.Select((Card c) =>
                c.ToSerializable()
            ).ToList()
        };
    }

    public static Deck FromSerializable(Deck.Json serializable)
    {
        return new Deck {
            player = serializable.player,
            cards = serializable.deck.Select((Card.Json c) => 
                Card.FromSerializable(c)
            ).ToList()
        };
    }


    public string ToJson()
    {
        return JsonUtility.ToJson(
            this.ToSerializable(),
            prettyPrint: true
        );
    }

    public static bool FromJson(string json, out Deck deck)
    {
        try {
            Deck.Json jsonDeck = JsonUtility.FromJson<Deck.Json>(json);
            deck = Deck.FromSerializable(jsonDeck);
            return true;
        } catch (System.Exception) {
            deck = default;
            return false;
        }
    }
}