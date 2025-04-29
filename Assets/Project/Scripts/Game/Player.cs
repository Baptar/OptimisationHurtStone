using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int pv;
    private int mana;
    private List<Card> main;
    private Deck deck;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitPlayer();
        InitDeck();
        InitMain();
    }

    private void InitPlayer()
    {
        // init value
        pv = Constants.playerPVStart;
        mana = 0;
    }

    private void InitDeck()
    {
        deck = Deck.Generate();
    }

    private void InitMain()
    {
        main = new List<Card>();
        for (int i = 0; i < Constants.mainNumberStart; i++)
        {
            main.Add(deck.Pop());
        }
    }
}
