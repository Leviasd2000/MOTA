using UnityEngine;

public static class SwordFactory
{
    public static Sword Create(string name, Braveattr player, MONSTER monster)
    {
        switch (name)
        {
            case "�Z��":
                return new Mundane(player, monster);
            case "�`��":
                return new Crimson(player, monster);
            case "���F":
                return new Spirit(player, monster);
            case "�y��":
                return new Streamstone(player, monster);
            case "�Ӫ�":
                return new Sovereign(player, monster);
            default:
                Debug.LogWarning($"Unknown weapon: {name}");
                return null;
        }
    }
}
