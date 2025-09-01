using LDtkUnity;
using UnityEngine;

public abstract class Shield
{
    public string Name { get; protected set; }
    public int BreathCost { get; protected set; }

    // �ק�ˮ`��ҡA�w�]����
    public virtual int ModifyDamage(int damage, MONSTER mon)
    {
        return damage;
    }

    // ����޵P�ĪG�G����B�W�h�ҡB�[���B�S��ĪG
    public virtual void ApplyEffect(Braveattr player, MONSTER monster)
    {
        player.DecreaseAttribute("Breath", BreathCost);
        player.IncreaseAttribute("Fatigue", 3);
    }
}

// ----------------------------
// ����޵P
// ----------------------------

public class Mirror : Shield
{
    public Mirror(Braveattr player, MONSTER monster)
    {
        Name = "�轤";
        BreathCost = Braveattr.Breathstock;
    }

    public override int ModifyDamage(int damage, MONSTER mon)
    {
        if (!mon.System.Contains("�]�k����"))
            return Mathf.RoundToInt(damage * 0.5f); // ���z�ˮ`��b
        return damage;
    }
}

public class Crystallite : Shield
{
    public Crystallite(Braveattr player, MONSTER monster)
    {
        Name = "����";
        BreathCost = 2 * Braveattr.Breathstock;
    }

    public override int ModifyDamage(int damage, MONSTER mon)
    {
        if (!mon.System.Contains("�]�k����"))
            return Mathf.RoundToInt(damage * 0.66f); // ���z�ˮ`�� 34%
        return damage;
    }

    public override void ApplyEffect(Braveattr player, MONSTER monster)
    {
        base.ApplyEffect(player, monster);
        player.IncreaseAttribute("Fatigue", 9); // �B�~�h��
        monster.Freeze = 1; // �S�ġG�B��Ǫ�
    }
}

public class Reflection : Shield
{
    public Reflection(Braveattr player, MONSTER monster)
    {
        Name = "�Ϯg";
        BreathCost = Braveattr.Breathstock;
    }

    public override int ModifyDamage(int damage, MONSTER mon)
    {
        if (!mon.System.Contains("�]�k����"))
            return Mathf.RoundToInt(damage * 0.8f); // ���z�ˮ`�� 20%
        return damage;
    }
}

public class Fairy : Shield
{
    public Fairy(Braveattr player, MONSTER monster)
    {
        Name = "���F";
        BreathCost = Braveattr.Breathstock;
    }

    public override int ModifyDamage(int damage, MONSTER mon)
    {
        return damage; // �ˮ`����
    }

    public override void ApplyEffect(Braveattr player, MONSTER monster)
    {
        base.ApplyEffect(player, monster);
        int Bdef = Braveattr.GetAttribute("Def");
        int Tempdef = Mathf.RoundToInt(Bdef * 0.1f); // �{�ɨ��m�[��
        player.IncreaseAttribute("Def", Tempdef);
    }
}

public class Sage : Shield
{
    public Sage(Braveattr player, MONSTER monster)
    {
        Name = "���";
        BreathCost = 2 * Braveattr.Breathstock;
    }

    public override int ModifyDamage(int damage, MONSTER mon)
    {
        return damage; // �ˮ`����
    }

    public override void ApplyEffect(Braveattr player, MONSTER monster)
    {
        base.ApplyEffect(player, monster);
        player.IncreaseAttribute("Fatigue", 2);
        // TODO: ���D��q�ഫ�ĪG
    }
}
