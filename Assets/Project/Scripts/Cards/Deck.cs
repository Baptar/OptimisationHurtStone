using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Deck
{
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

    public void Shuffle()
    {
        cards = cards.OrderBy((Card _) =>
            System.Guid.NewGuid()
        ).ToList();
    }
}



public partial class Deck
{
    public static Deck Generate()
    {
        List<Card> deckConstruct = DeckHelper.PoolToDeckConstruct(CardPool.Get());

        throw new System.NotImplementedException();
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
            deck = cards.Select((Card c) =>
                c.ToSerializable()
            ).ToList()
        };
    }

    public static Deck FromSerializable(Deck.Json serializable)
    {
        return new Deck {
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