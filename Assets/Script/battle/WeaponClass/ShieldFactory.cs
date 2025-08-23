using UnityEngine;

public static class ShieldFactory
{
    public static Shield Create(string name, Braveattr player, MONSTER monster)
    {
        switch (name)
        {
            case "�轤":
                return new Mirror(player, monster);
            case "����":
                return new Crystallite(player, monster);
            case "�Ϯg":
                return new Reflection(player, monster);
            case "���F":
                return new Fairy(player, monster);
            case "���":
                return new Sage(player, monster);
            default:
                Debug.LogWarning($"Unknown weapon: {name}");
                return null;
        }
    }
}
