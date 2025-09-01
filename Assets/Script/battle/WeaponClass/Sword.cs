using UnityEngine;

// ----------------------------
// 抽象基類
// ----------------------------
public abstract class Sword
{
    public string Name { get; protected set; }
    public int BreathCost { get; protected set; } // 劍使用的氣息消耗

    // 修改傷害
    public virtual int ModifyDamage(int baseDamage, Braveattr player, MONSTER monster)
    {
        return baseDamage;
    }

    // 套用劍效果：扣氣、增疲勞、加屬性等
    public virtual void ApplyEffect(Braveattr player, MONSTER monster)
    {
        player.DecreaseAttribute("Breath", BreathCost);
    }

    // 計算最終傷害
    public int CalculateDamage(Braveattr player, MONSTER monster)
    {
        int baseDamage = Mathf.Max(Braveattr.GetAttribute("Atk") - monster.Def, 1);
        int finalDamage = ModifyDamage(baseDamage, player, monster);
        ApplyEffect(player, monster);
        return finalDamage;
    }
}

// ----------------------------
// 具體劍
// ----------------------------
public class Mundane : Sword
{
    public Mundane(Braveattr player, MONSTER monster)
    {
        Name = "凡骨";
        BreathCost = Braveattr.Breathstock;
    }

    public override int ModifyDamage(int baseDamage, Braveattr player, MONSTER monster)
    {
        int Level = Braveattr.GetAttribute("Level");
        // 提升攻擊力加成
        Braveattr.Tempatk = (int)(1.5f * Level);
        Braveattr.Tempdef = (int)(1.25f * Level);

        player.IncreaseAttribute("Atk", Braveattr.Tempatk);
        player.DecreaseAttribute("Def", Braveattr.Tempdef);

        return Mathf.RoundToInt(baseDamage * 1.5f);
    }

    public override void ApplyEffect(Braveattr player, MONSTER monster)
    {
        base.ApplyEffect(player, monster);
        player.IncreaseAttribute("Fatigue", 10);
    }
}

public class Streamstone : Sword
{
    public Streamstone(Braveattr player, MONSTER monster)
    {
        Name = "流石";
        BreathCost = Braveattr.Breathstock;
    }

    public override int ModifyDamage(int baseDamage, Braveattr player, MONSTER monster)
    {
        int Mbreath = monster.CurrentBreath;
        // 傷害倍率
        int damage = Mathf.RoundToInt(baseDamage * 1.3f);
        // 減少怪物氣息
        monster.CurrentBreath /= 2;
        // 增加玩家氣息
        player.IncreaseAttribute("Breath", Mbreath / 2);
        return damage;
    }

    public override void ApplyEffect(Braveattr player, MONSTER monster)
    {
        base.ApplyEffect(player, monster);
        player.IncreaseAttribute("Fatigue", 4);
    }
}

public class Crimson : Sword
{
    public Crimson(Braveattr player, MONSTER monster)
    {
        Name = "深紅";
        BreathCost = Braveattr.Breathstock;
    }

    public override int ModifyDamage(int baseDamage, Braveattr player, MONSTER monster)
    {
        int damage = Mathf.RoundToInt(baseDamage * 1.3f);
        damage = Mathf.Min(damage, monster.CurrentHp - 1);
        // 回復玩家血量
        player.IncreaseAttribute("Hp", Mathf.RoundToInt(damage * 0.3f));
        return damage;
    }

    public override void ApplyEffect(Braveattr player, MONSTER monster)
    {
        base.ApplyEffect(player, monster);
        player.IncreaseAttribute("Fatigue", 4);
    }
}

public class Spirit : Sword
{
    public Spirit(Braveattr player, MONSTER monster)
    {
        Name = "天靈";
        BreathCost = 2 * Braveattr.Breathstock;
    }

    public override int ModifyDamage(int baseDamage, Braveattr player, MONSTER monster)
    {
        int damage = Mathf.RoundToInt(baseDamage * 1.8f);
        // 增加怪物疲勞
        monster.CurrentFatigue += 10;
        return damage;
    }

    public override void ApplyEffect(Braveattr player, MONSTER monster)
    {
        base.ApplyEffect(player, monster);
        player.IncreaseAttribute("Fatigue", 8);
    }
}

public class Sovereign : Sword
{
    public Sovereign(Braveattr player, MONSTER monster)
    {
        Name = "王者";
        BreathCost = 3 * Braveattr.Breathstock;
    }

    public override int ModifyDamage(int baseDamage, Braveattr player, MONSTER monster)
    {
        int damage = Mathf.RoundToInt(baseDamage * 5f);
        damage = Mathf.Min(damage, monster.CurrentHp - 1);
        return Mathf.Max(damage, 1);
    }

    public override void ApplyEffect(Braveattr player, MONSTER monster)
    {
        base.ApplyEffect(player, monster);
        player.IncreaseAttribute("Fatigue", 30);
    }
}


