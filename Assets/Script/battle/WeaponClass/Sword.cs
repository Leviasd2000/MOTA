using UnityEngine;

public abstract class Sword
{
    public string Name { get; protected set; }  
    public int BreathCost { get; protected set; } // 劍使用的氣息消耗

    public abstract int Damage(Braveattr player, MONSTER monster); // 抽象方法：沒有內容，等子類去實作
}

public class Mundane  : Sword
{
    public Mundane(Braveattr player , MONSTER monster)
    {
        Name = "凡骨";
        BreathCost = 40; 
    }

    public override int Damage(Braveattr player , MONSTER monster)
    {
        int Matk = monster.Atk;
        int Mdef = monster.Def;
        string Msys = monster.System;
        int Batk = Braveattr.GetAttribute("Atk");
        int Bdef = Braveattr.GetAttribute("Def");
        int Level = Braveattr.GetAttribute("Level");

        Braveattr.Tempatk = (int)(1.5f * Level);
        Braveattr.Tempdef = (int)(1.25f * Level);

        player.DecreaseAttribute("Breath", 40);
        player.IncreaseAttribute("Fatigue", 10);
        player.IncreaseAttribute("Atk", Braveattr.Tempatk);
        player.DecreaseAttribute("Def", Braveattr.Tempdef);

        return (int)Mathf.Max(1.5f * (Batk - Mdef), 1);
    }
}

public class Streamstone : Sword
{
    public Streamstone(Braveattr player, MONSTER monster)
    {
        Name = "流石";
        BreathCost = 40;
    }

    public override int Damage(Braveattr player, MONSTER monster)
    {
        int Matk = monster.Atk;
        int Mdef = monster.Def;
        string Msys = monster.System;
        int Mbreath = monster.CurrentBreath;
        int Batk = Braveattr.GetAttribute("Atk");
        int Bdef = Braveattr.GetAttribute("Def");

        player.DecreaseAttribute("Breath", 40);
        player.IncreaseAttribute("Fatigue", 4);
        player.IncreaseAttribute("Breath", Mbreath/2);
        monster.CurrentBreath = monster.CurrentBreath/2;

        return (int)Mathf.Max(1.3f * (Batk - Mdef), 1);
    }
}

public class Crimson : Sword
{
    public Crimson(Braveattr player, MONSTER monster)
    {
        Name = "深紅";
        BreathCost = 40;
    }

    public override int Damage(Braveattr player, MONSTER monster)
    {
        int Matk = monster.Atk;
        int Mdef = monster.Def;
        string Msys = monster.System;
        int Batk = Braveattr.GetAttribute("Atk");
        int Bdef = Braveattr.GetAttribute("Def");

        int damage = (int)Mathf.Min(0.8f * (Batk - Mdef), (monster.CurrentHp-1));

        player.DecreaseAttribute("Breath", 40);
        player.IncreaseAttribute("Fatigue", 4);
        player.IncreaseAttribute("Hp" , (int)0.3f * damage);

        return (int)Mathf.Max(1.3f * (Batk - Mdef), 1);

    }
}

public class Spirit : Sword
{
    public Spirit(Braveattr player, MONSTER monster)
    {
        Name = "天靈";
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
        player.IncreaseAttribute("Fatigue", 8);
        monster.CurrentFatigue = monster.CurrentFatigue + 10;

        return (int)Mathf.Max(1.8f * (Batk - Mdef), 1);

    }
}

public class Sovereign : Sword
{
    public Sovereign(Braveattr player, MONSTER monster)
    {
        Name = "王者";
        BreathCost = 120;
    }

    public override int Damage(Braveattr player, MONSTER monster)
    {
        int Matk = monster.Atk;
        int Mdef = monster.Def;
        string Msys = monster.System;
        int Batk = Braveattr.GetAttribute("Atk");
        int Bdef = Braveattr.GetAttribute("Def");

        int damage = (int)Mathf.Min(5 * (Batk - Mdef), (monster.CurrentHp - 1));

        player.DecreaseAttribute("Breath", 120);
        player.IncreaseAttribute("Fatigue", 30);

        return (int)Mathf.Max(damage, 1);

    }
}



