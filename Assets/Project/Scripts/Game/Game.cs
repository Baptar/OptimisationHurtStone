using UnityEngine;

public class Game
{
    [HideInInspector]
    public Player[] players = new Player[2];
    [HideInInspector]
    public Board board;

    [HideInInspector]
    public int roundCount { get; private set; } = 0;
    private int startOrder = 0;

    public Result result { get; private set; } = default;
    [HideInInspector]
    public bool finished { get; private set; } = false;
    [HideInInspector]
    public int winId = -1;
    

    public void Start(bool autoPlay = true)
    {
        // Create everything
        players[0] = new Player(); // Benchmark Player
        players[1] = new Player(); // Ennemy Reference Player
        board = new Board();

        // Init Everything
        if (!autoPlay) return;
        Init(null, null);
        PlayGame();
    }

    public void Init(Deck benchDeck = null, Deck refDeck = null)
    {
        startOrder = Random.Range(0, 2);
        board.Init(count: 2);
        if (benchDeck == null) players[0].Init(0, Constants.benchmarkDeck); // Load bench deck
        else players[0].Init(0, benchDeck); // Create by copying bench deck
        if (refDeck == null) players[1].Init(1, Constants.referenceDeck); // Load ref deck
        else players[1].Init(1, refDeck); // Create by copying ref deck
    }

    public void PlayGame()
    {
        Player player;
        while (!finished) {

            // Update Round & make player play
            ++roundCount;
            int turnId = (roundCount + startOrder) % 2;
            player = players[1 - turnId];
            player.Play(this);

            // Check if game finished
            if (winId >= 0) {
                finished = true;
                Simulator.AddResult(result = new Result {
                    winnerId = winId,
                    roundCount = roundCount,
                    startId = startOrder,
                });
            }

        }
    }

}