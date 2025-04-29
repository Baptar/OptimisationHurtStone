using UnityEngine;

public static class Constants
{
    // Number of duplicate to construct Deck from card pool
    public const int duplicateCount = 1;
    // Size of a deck
    public const int deckSize = 30;

    // Max Card cost
    public const int maxCardCost = 6;

    // Max Values (for generation)
    public const int maxCardValue = 20;

    // Default Deck Player Name
    public const string playerName = "Unknown";

    // Default deck names
    public const string benchmarkDeck = "benchmark_deck.json";
    public const string referenceDeck = "reference_deck.json";
    public const string benchmarkOptimize = "opti_Bench_deck.json";
    public const string referenceOptimize = "opti_Ref_deck.json";

    // Simulation Deck Pool Size
    public const int deckPoolSize = 5;

    public const int handNumberStart = 5;

    public const int playerPVStart = 30;

    public const int playerManaStart = 0;
}