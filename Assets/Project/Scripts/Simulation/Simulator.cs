using System.Collections.Generic;
using System.Collections.ObjectModel;

public static class Simulator
{
    private static readonly List<Result> results = new List<Result>();
    private static bool running = false;

    public static event System.Action OnStart;
    public static event System.Action OnEnded;


    public static void Run()
    {
        if (running) return;
        running = true;
        // TODO : Start les simulations
        OnStart?.Invoke();
    }

    public static void Finish()
    {
        if (!running) return;
        running = false;
        // TODO : Stop les simulations
        OnEnded?.Invoke();
    }

    public static void Reset()
    {
        Finish();
        results.Clear();
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