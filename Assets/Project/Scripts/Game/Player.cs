using System.Collections.Generic;
using System.Linq;

public class Player
{
    private int id;
    private int pv;
    private int manaMax;
    private List<Card> hand;
    private Deck deck;

    private int currentMana;
    public int OtherPlayerId => 1 - id;

    public void Init(int id, string deckFile = "")
    {
        this.id = id;
        InitPlayer();
        InitDeck(deckFile);
        InitHand();
    }

    public void Init(int id, Deck deck)
    {
        this.id = id;
        InitPlayer();
        InitDeck(deck);
        InitHand();
    }

    private void InitPlayer()
    {
        // init value
        pv = Constants.playerPVStart;
        manaMax = Constants.playerManaStart;
        currentMana = Constants.playerManaStart;
    }

    private void InitDeck(string deckFile)
    {
        if (string.IsNullOrEmpty(deckFile)
            || !Load.Deck(out deck, filename: deckFile)
        ) deck = DeckPool.Get();
        deck.Shuffle();
    }

    private void InitDeck(Deck deck)
    {
        this.deck = deck.Copy().Shuffle();
    }

    private void InitHand()
    {
        hand = new List<Card>();
        for (int i = 0; i < Constants.handNumberStart; i++) {
            AddToHand(deck.Pop());
        }
    }


    private void AddToHand(Card card)
    {
        // Try insert in cost order
        for (int i = 0; i < hand.Count; ++i) { 
            if (card.cost <= hand[i].cost) {
                hand.Insert(i, card);
                return;
            }
        }
        // No higher cost, adding at the end
        hand.Add(card);
    }


    public void Play(Game game)
    {
        // Update mana
        ++manaMax;
        currentMana = manaMax;

        // Pioche depuis le deck
        if (deck.IsEmpty()) {
            game.winId = OtherPlayerId;
            return;
        }
        AddToHand(deck.Pop());

        // Joue les cartes dans l'ordre
        for (int i = hand.Count() - 1; i >= 0; --i) {
            Card card = hand[i];
            if (card.cost > currentMana) continue;
            currentMana -= card.cost;
            hand.RemoveAt(i);
            Play(game, card);
        }

        // Make creatures attack
        Board.Player board = game.board.players[id];
        Player target = game.players[OtherPlayerId];
        foreach (Card card in board.board) {
            target.pv -= card.atk;
            if (target.pv <= 0) {
                game.winId = id;
                break;
            }
        }
    }

    private void Play(Game game, Card card)
    {
        // Simply add card to board
        Board.Player board = game.board.players[id];
        board.board.Add(card);
    }
}