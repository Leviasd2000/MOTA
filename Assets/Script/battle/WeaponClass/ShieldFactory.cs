using UnityEngine;

public static class ShieldFactory
{
    public static Shield Create(string name, Braveattr player, MONSTER monster)
    {
        switch (name)
        {
            case "鏡膜":
                return new Mirror(player, monster);
            case "結晶":
                return new Crystallite(player, monster);
            case "反射":
                return new Reflection(player, monster);
            case "精靈":
                return new Fairy(player, monster);
            case "賢者":
                return new Sage(player, monster);
            default:
                Debug.LogWarning($"Unknown weapon: {name}");
                return null;
        }
    }
}
