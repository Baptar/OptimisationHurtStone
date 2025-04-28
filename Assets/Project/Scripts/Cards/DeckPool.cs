using System.Collections.Generic;
using UnityEngine;

public static class DeckPool
{
    static readonly Queue<Deck> pool = new Queue<Deck>();

    [RuntimeInitializeOnLoadMethod]
    static void Initialize() => Allocate();
    public static void Allocate()
    {
        for (int i = 0; i < Constants.deckPoolSize; ++i) {
            pool.Enqueue(Deck.Generate());
        }
    }

    public static Deck Get()
    {
        if (pool.Count <= 0) return Deck.Generate();
        return pool.Dequeue();
    }
}
