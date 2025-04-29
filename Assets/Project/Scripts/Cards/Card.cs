using UnityEngine;

public partial struct Card
{
    public string name;
    public int cost;
    public int atk;
    public int def;
}



public partial struct Card
{
    public readonly bool IsValid()
    {
        return !string.IsNullOrEmpty(name) && def > 0;
    }

    public Card CalculateCost()
    {
        cost = Mathf.FloorToInt((atk + def) / 2);
        return this;
    }

    public override readonly bool Equals(object obj)
    {
        if (obj is Card card) Equals(card);
        return base.Equals(obj);
    }

    public override readonly int GetHashCode()
    {
        return System.Tuple.Create(name, cost, atk, def).GetHashCode();
    }

    public readonly bool Equals(Card card)
    {
        return name == card.name
            && cost == card.cost
            && atk == card.atk
            && def == card.def;
    }
}



public partial struct Card
{
    [System.Serializable]
    public struct Json
    {
        public string name;
        public int attack;
        public int defense;
    }


    public readonly Card.Json ToSerializable()
    {
        return new Card.Json {
            name = name,
            attack = atk,
            defense = def
        };
    }

    public static Card FromSerializable(Card.Json serializable)
    {
        return new Card {
            name = serializable.name,
            atk = serializable.attack,
            def = serializable.defense,
        }.CalculateCost();
    }


    public readonly string ToJson()
    {
        return JsonUtility.ToJson(
            this.ToSerializable(),
            prettyPrint: true
        );
    }

    public static bool FromJson(string json, out Card card)
    {
        try {
            Json jsonCard = JsonUtility.FromJson<Card.Json>(json);
            card = Card.FromSerializable(jsonCard);
            return true;
        } catch (System.Exception) {
            card = default;
            return false;
        }
    }
}