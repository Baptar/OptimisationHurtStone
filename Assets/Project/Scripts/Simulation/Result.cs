[System.Serializable]
public struct Result
{
    public int winnerId; // 0 = joueur, 1 = adversaire
    public int roundCount; // Nombre de round/tour (1 tour = 1 joueur a joué)
}