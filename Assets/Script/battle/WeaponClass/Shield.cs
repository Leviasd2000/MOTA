using LDtkUnity;
using UnityEngine;

public abstract class Shield
{
    public string Name { get; protected set; }
    public int BreathCost { get; protected set; } // 盾使用的氣息消耗
    public abstract int Damage(Braveattr player, MONSTER monster); // 抽象方法：沒有內容，等子類去實作
}

public class Mirror : Shield       
{
    public Mirror(Braveattr player, MONSTER monster)
    {
        Name = "鏡膜";
        BreathCost = 40;
    }

    public override int Damage(Braveattr player, MONSTER monster)
    {
        int Matk = monster.Atk;
        int Mdef = monster.Def;
        string Msys = monster.System;
        int Batk = Braveattr.GetAttribute("Atk");
        int Bdef = Braveattr.GetAttribute("Def");
        int Level = Braveattr.GetAttribute("Level");

        player.DecreaseAttribute("Breath", 40);
        player.IncreaseAttribute("Fatigue", 3);

        return (int)Mathf.Max(0.5f * (Matk - Bdef), 1);
    }
}

public class Crystallite : Shield
{
    public Crystallite(Braveattr player, MONSTER monster)
    {
        Name = "結晶";
        BreathCost = 80;
    }

    public override int Damage(Braveattr player, MONSTER monster)
    {
        int Matk = monster.Atk;
        int Mdef = monster.Def;
        string Msys = monster.System;
        int Mbreath = monster.CurrentBreath;
        int Batk = Braveattr.GetAttribute("Atk");
        int Bdef = Braveattr.GetAttribute("Def");


        player.DecreaseAttribute("Breath", 80);
        player.IncreaseAttribute("Fatigue", 12);
        monster.Freeze = 1;

        return (int)Mathf.Max(0.66f * (Matk - Bdef), 1);
    }
}

public class Reflection : Shield
{
    public Reflection(Braveattr player, MONSTER monster)
    {
        Name = "反射";
        BreathCost = 40;
    }

    public override int Damage(Braveattr player, MONSTER monster)
    {
        int Matk = monster.Atk;
        int Mdef = monster.Def;
        int MHp = monster.CurrentHp;
        string Msys = monster.System;
        int Batk = Braveattr.GetAttribute("Atk");
        int Bdef = Braveattr.GetAttribute("Def");
        int damage = (int)Mathf.Min(0.8f * (Batk - Mdef), (monster.CurrentHp - 1));

        player.DecreaseAttribute("Breath", 40);
        player.IncreaseAttribute("Fatigue", 3);
        MHp -= Matk/10 + (int)0.3f * damage;

        return (int)Mathf.Max(0.8f * (Matk - Bdef), 1);

    }
}

public class Fairy : Shield
{
    public Fairy(Braveattr player, MONSTER monster)
    {
        Name = "精靈";
        BreathCost = 40;
    }

    public override int Damage(Braveattr player, MONSTER monster)
    {
        int Matk = monster.Atk;
        int Mdef = monster.Def;
        string Msys = monster.System;
        int Batk = Braveattr.GetAttribute("Atk");
        int Bdef = Braveattr.GetAttribute("Def");
        int Level = Braveattr.GetAttribute("Level");
        int Tempdef = Braveattr.Tempdef;

        Tempdef += (int)(Bdef / 100 + 0.1 * Level);
        player.DecreaseAttribute("Breath", 40);
        player.IncreaseAttribute("Fatigue", 3);
        player.IncreaseAttribute("Def", Tempdef);

        return (int)Mathf.Max(1f * (Matk - Bdef), 1);

    }
}

public class Sage : Shield
{
    public Sage(Braveattr player, MONSTER monster)
    {
        Name = "賢者";
        BreathCost = 80;
    }

    public override int Damage(Braveattr player, MONSTER monster)
    {
        int Matk = monster.Atk;
        int Mdef = monster.Def;
        string Msys = monster.System;
        int Batk = Braveattr.GetAttribute("Atk");
        int Bdef = Braveattr.GetAttribute("Def");


        player.DecreaseAttribute("Breath", 80);
        player.IncreaseAttribute("Fatigue", 2);
    　　// TODO: 公主血量轉換

        return (int)Mathf.Max(1f * (Matk - Bdef), 1);

    }
}
