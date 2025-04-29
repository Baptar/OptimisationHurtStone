using System.Collections.Generic;

public class Board
{
    public class Player
    {
        public List<Card> board = new List<Card>();
    }

    public List<Board.Player> players;

    public void Init(int count)
    {
        players = new List<Board.Player>();
        for (int i = 0; i < count; ++i) players.Add(new Board.Player());
    }
}