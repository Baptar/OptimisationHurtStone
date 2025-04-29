using System.Collections.Generic;

public static class DeckHelper
{
    public static List<Card> PoolToDeckConstruct(List<Card> pool)
    {
#pragma warning disable CS0162 // Code inaccessible détecté
        if (Constants.duplicateCount <= 0) return pool;
#pragma warning restore CS0162 // Code inaccessible détecté
        List<Card> output = new List<Card>(pool);
        for (int i = 0; i < Constants.duplicateCount; ++i) output.AddRange(pool);
        return CardPool.Order(output);
    }
}
