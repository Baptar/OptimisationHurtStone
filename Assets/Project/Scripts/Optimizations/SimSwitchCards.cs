using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

// Cout moyen des cartes avec l'evolution du deck
// Ditribution du cout des cartes (toutes les 5-10 iterations)
// Duree des parties
// Atk moyenne & Defense moyenne des cartes

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
    private List<Card> deckConstruct;
    private Queue<Card> storeConstruct;
    private int switchId = 0;


    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////


    void Update()
    {
        if (!startSimulation) return;
        startSimulation = false;
        if (routine != null) return;
        StartSimulation();
    }

    private void StartSimulation()
    {
        // Load optimization decks
        Load.Deck(out Simulator.benchDeck, benchmarkDeck);
        Load.Deck(out Simulator.refDeck, referenceDeck);

        // Setup deck construct
        deckConstruct = DeckHelper.PoolToDeckConstruct(CardPool.Get());
        storeConstruct = new Queue<Card>(deckConstruct.Count);
        LinkConstructToDeck();

        // Start properly simulation
        stopSimulation = false;
        routine = StartCoroutine(SimulationRoutine());
    }

    IEnumerator SimulationRoutine()
    {
        // Run Simulations while asked for
        while (!stopSimulation) {
            NewSimulation(); // Warning !! Is blocking thread
            if (simulationDelay > 0) yield return new WaitForSeconds(simulationDelay);
            yield return new WaitForFixedUpdate();
        }
        SaveResults();
        stopSimulation = false;
        routine = null;
    }


    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////


    void OnDisable() => StopSimulation(false);
    private void StopSimulation(bool save)
    {
        if (routine == null) return;
        StopCoroutine(routine);
        routine = null;
        if (save) SaveResults();
    }

    private void SaveResults()
    {
        Save.Deck(Simulator.benchDeck, benchmarkSave);
        Debug.Log($"Runned Simulator '{runCount}' times");
    }


    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////


    private void NewSimulation()
    {
        if (Simulator.Running) return;
        ++runCount;
        Simulate();
    }

    private void Simulate()
    {
        Simulator.OnEnded -= Simulator_OnEnded;
        Simulator.Reset();
        Simulator.OnEnded += Simulator_OnEnded;
        Simulator.Run(simulationCount);
    }

    private void Simulator_OnEnded()
    {
        Simulator.OnEnded -= Simulator_OnEnded;
        float winrate = SimulatorOutput.GetWinRate(winnerId: 0);

        // Re-Switch cards depending on winrate results
        if (previousWinrate >= 0f && winrate < previousWinrate)
            Simulator.benchDeck.cards[switchId] = previousCard;
        previousWinrate = winrate;

        // Update Deck Construct & Deck switch id
        if (storeConstruct.Count <= 0) {
            if (++switchId >= Simulator.benchDeck.cards.Count) {
                StopSimulation(true);
                return;
            } else LinkConstructToDeck();
        }

        // Deck Construct does not has card to offer a better deck
        if (storeConstruct.Count <= 0) {
            StopSimulation(true);
            return;
        }

        // Switch card for another optimization test
        previousCard = Simulator.benchDeck.cards[switchId];
        Simulator.benchDeck.cards[switchId] = storeConstruct.Dequeue();
    }


    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////
    

    private void LinkConstructToDeck()
    {
        storeConstruct.Clear();
        List<Card> cards = Simulator.benchDeck.cards.ToList();
        foreach (Card card in deckConstruct) {
            int index = cards.FindIndex(card.Equals);
            if (index < 0) storeConstruct.Enqueue(card);
            else cards.RemoveAt(index);
        }
    }
}