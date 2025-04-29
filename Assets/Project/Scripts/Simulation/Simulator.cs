using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public static class Simulator
{
    [RuntimeInitializeOnLoadMethod]
    static void Initialize() => Reset();
    private static readonly List<Result> results = new List<Result>();
    private static bool running = false;

    public static bool Running => running;
    public static event System.Action OnStart;
    public static event System.Action OnEnded;

    public static Deck benchDeck = null;
    public static Deck refDeck = null;

    public static void Reset()
    {
        End();
        results.Clear();
    }

    public static void Run(int count)
    {
        if (running) return;
        running = true;
        OnStart?.Invoke();

        // Run full simulation
        Game game;
        for (int i = 0; i < count; ++i) {
            game = new Game();
            game.Start(autoPlay: false);
            game.Init(benchDeck, refDeck);
            game.PlayGame();
        }
        
        // End/Get Results
        End();
    }

    public static void End()
    {
        if (!running) return;
        running = false;
        // Stop les simulations
        OnEnded?.Invoke();
    }



    public static void AddResult(Result result)
    {
        results.Add(result);
    }

    public static ReadOnlyCollection<Result> GetResults()
    {
        return results.AsReadOnly();
    }
}