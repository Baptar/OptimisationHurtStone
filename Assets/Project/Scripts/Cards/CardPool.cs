using System.Collections.Generic;
using System.Linq;

public static class CardPool
{
    static readonly List<Card> cache = new List<Card>();
    static bool Cached => cache.Count > 0;



    public static List<Card> Get()
    {
        if (!Cached) Generate();
        return cache.ToList();
    }

    public static List<Card> Order(List<Card> cards)
    {
        return cards.OrderBy((Card c) => c.cost).ToList();
    }

    public static bool IsValid(Card card)
    {
        return card.cost <= Constants.maxCardCost;
    }



    private static void Generate()
    {
        // Setup cards temp cache
        List<Card> cards = new List<Card>();
        cache.Clear();

        // Loop over possibilities
        for (int atk = 0; atk < Constants.maxCardValue; ++atk) { 
            for (int def = 1; def < Constants.maxCardValue; ++def) {

                // Generate card
                Card card = new Card {
                    name = $"Card(atk={atk}, def={def})",
                    atk = atk, def = def
                }.CalculateCost();

                // Regiter valid card
                if (IsValid(card)) {
                    cards.Add(card);
                }
            }
        }

        // Cache registered cards by cost
        cache.AddRange(CardPool.Order(cards));
    }
}
