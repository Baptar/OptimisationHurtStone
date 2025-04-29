using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SimSwitchCards : MonoBehaviour
{
    [Header("Helper")]
    [SerializeField]
    private bool stopSimulation = false;
    [SerializeField]
    private bool startSimulation = true;

    [Header("Optimization Settings")]
    [SerializeField]
    private float simulationDelay = 0.1f;
    [SerializeField]
    private string benchmarkDeck = Constants.benchmarkDeck;
    [SerializeField]
    private string referenceDeck = Constants.referenceDeck;

    [Space]
    [Header("Save Settings")]
    [SerializeField]
    private string benchmarkSave = Constants.benchmarkOptimize;

    [Space]
    [Header("Simulation Settings")]
    [SerializeField]
    private int simulationCount = 500;

    private Coroutine routine = null;
    private int runCount = 0;

    private float previousWinrate = -1;
    private Card previousCard;
    private List<Card> cardsPool;
    private int switchId = -1;
    private int poolId = -1;
    


    void Update()
    {
        if (!startSimulation) return;
        startSimulation = false;
        if (routine != null) return;
        StartSimulation();
    }

    private void StartSimulation()
    {
        stopSimulation = false;
        cardsPool = CardPool.Get();
        Load.Deck(out Simulator.benchDeck, benchmarkDeck);
        Load.Deck(out Simulator.refDeck, referenceDeck);
        routine = StartCoroutine(SimulationRoutine());
    }

    IEnumerator SimulationRoutine()
    {
        // Run Simulations while asked for
        while (!stopSimulation) {
            NewSimulation();
            if (simulationDelay > 0) yield return new WaitForSeconds(simulationDelay);
            yield return new WaitForFixedUpdate();
        }
        SaveResults();
        stopSimulation = false;
        routine = null;
    }



    void OnDisable() => StopSimulation();
    private void StopSimulation()
    {
        if (routine == null) return;
        StopCoroutine(routine);
        routine = null;
        SaveResults();
    }

    private void SaveResults()
    {
        Save.Deck(Simulator.benchDeck, benchmarkSave);
        Debug.Log($"Runned Simulator '{runCount}' times");
    }



    private void NewSimulation()
    {
        if (Simulator.Running) return;
        ++runCount;
        Simulate();
    }

    private void Simulate()
    {
        Simulator.OnEnded -= Simulator_OnEnded;
        Simulator.OnEnded += Simulator_OnEnded;
        Simulator.Run(simulationCount);
    }

    private void Simulator_OnEnded()
    {
        Simulator.OnEnded -= Simulator_OnEnded;
        float winrate = SimulatorOutput.GetWinRate(winnerId: 0);

        // Re-Switch cards depending on winrate results
        if (previousWinrate >= 0 && winrate < previousWinrate)
            Simulator.benchDeck.cards[switchId] = previousCard;
        previousWinrate = winrate;

        // Update bench switch id
        if (++poolId >= cardsPool.Count) {
            poolId = 0;
            if (++switchId >= Simulator.benchDeck.cards.Count) {
                StopSimulation();
                return;
            }
        }

        // Switch card for another optimization test
        previousCard = Simulator.benchDeck.cards[switchId];
        Simulator.benchDeck.cards[switchId] = cardsPool[poolId];
    }
}