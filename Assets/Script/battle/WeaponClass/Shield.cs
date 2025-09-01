using LDtkUnity;
using UnityEngine;

public abstract class Shield
{
    public string Name { get; protected set; }
    public int BreathCost { get; protected set; }

    // 修改傷害比例，預設不改
    public virtual int ModifyDamage(int damage, MONSTER mon)
    {
        return damage;
    }

    // 執行盾牌效果：扣氣、增疲勞、加防、特殊效果
    public virtual void ApplyEffect(Braveattr player, MONSTER monster)
    {
        player.DecreaseAttribute("Breath", BreathCost);
        player.IncreaseAttribute("Fatigue", 3);
    }
}

// ----------------------------
// 具體盾牌
// ----------------------------

public class Mirror : Shield
{
    public Mirror(Braveattr player, MONSTER monster)
    {
        Name = "鏡膜";
        BreathCost = Braveattr.Breathstock;
    }

    public override int ModifyDamage(int damage, MONSTER mon)
    {
        if (!mon.System.Contains("魔法攻擊"))
            return Mathf.RoundToInt(damage * 0.5f); // 物理傷害減半
        return damage;
    }
}

public class Crystallite : Shield
{
    public Crystallite(Braveattr player, MONSTER monster)
    {
        Name = "結晶";
        BreathCost = 2 * Braveattr.Breathstock;
    }

    public override int ModifyDamage(int damage, MONSTER mon)
    {
        if (!mon.System.Contains("魔法攻擊"))
            return Mathf.RoundToInt(damage * 0.66f); // 物理傷害減 34%
        return damage;
    }

    public override void ApplyEffect(Braveattr player, MONSTER monster)
    {
        base.ApplyEffect(player, monster);
        player.IncreaseAttribute("Fatigue", 9); // 額外疲勞
        monster.Freeze = 1; // 特效：冰凍怪物
    }
}

public class Reflection : Shield
{
    public Reflection(Braveattr player, MONSTER monster)
    {
        Name = "反射";
        BreathCost = Braveattr.Breathstock;
    }

    public override int ModifyDamage(int damage, MONSTER mon)
    {
        if (!mon.System.Contains("魔法攻擊"))
            return Mathf.RoundToInt(damage * 0.8f); // 物理傷害減 20%
        return damage;
    }
}

public class Fairy : Shield
{
    public Fairy(Braveattr player, MONSTER monster)
    {
        Name = "精靈";
        BreathCost = Braveattr.Breathstock;
    }

    public override int ModifyDamage(int damage, MONSTER mon)
    {
        return damage; // 傷害不改
    }

    public override void ApplyEffect(Braveattr player, MONSTER monster)
    {
        base.ApplyEffect(player, monster);
        int Bdef = Braveattr.GetAttribute("Def");
        int Tempdef = Mathf.RoundToInt(Bdef * 0.1f); // 臨時防禦加成
        player.IncreaseAttribute("Def", Tempdef);
    }
}

public class Sage : Shield
{
    public Sage(Braveattr player, MONSTER monster)
    {
        Name = "賢者";
        BreathCost = 2 * Braveattr.Breathstock;
    }

    public override int ModifyDamage(int damage, MONSTER mon)
    {
        return damage; // 傷害不改
    }

    public override void ApplyEffect(Braveattr player, MONSTER monster)
    {
        base.ApplyEffect(player, monster);
        player.IncreaseAttribute("Fatigue", 2);
        // TODO: 公主血量轉換效果
    }
}
