using UnityEngine;

public static class SwordFactory
{
    public static Sword Create(string name, Braveattr player, MONSTER monster)
    {
        switch (name)
        {
            case "凡骨":
                return new Mundane(player, monster);
            case "深紅":
                return new Crimson(player, monster);
            case "天靈":
                return new Spirit(player, monster);
            case "流石":
                return new Streamstone(player, monster);
            case "皇者":
                return new Sovereign(player, monster);
            default:
                Debug.LogWarning($"Unknown weapon: {name}");
                return null;
        }
    }
}
