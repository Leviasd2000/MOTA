using UnityEngine;

// ----------------------------
// ��H����
// ----------------------------
public abstract class Sword
{
    public string Name { get; protected set; }
    public int BreathCost { get; protected set; } // �C�ϥΪ��𮧮���

    // �ק�ˮ`
    public virtual int ModifyDamage(int baseDamage, Braveattr player, MONSTER monster)
    {
        return baseDamage;
    }

    // �M�μC�ĪG�G����B�W�h�ҡB�[�ݩʵ�
    public virtual void ApplyEffect(Braveattr player, MONSTER monster)
    {
        player.DecreaseAttribute("Breath", BreathCost);
    }

    // �p��̲׶ˮ`
    public int CalculateDamage(Braveattr player, MONSTER monster)
    {
        int baseDamage = Mathf.Max(Braveattr.GetAttribute("Atk") - monster.Def, 1);
        int finalDamage = ModifyDamage(baseDamage, player, monster);
        ApplyEffect(player, monster);
        return finalDamage;
    }
}

// ----------------------------
// ����C
// ----------------------------
public class Mundane : Sword
{
    public Mundane(Braveattr player, MONSTER monster)
    {
        Name = "�Z��";
        BreathCost = Braveattr.Breathstock;
    }

    public override int ModifyDamage(int baseDamage, Braveattr player, MONSTER monster)
    {
        int Level = Braveattr.GetAttribute("Level");
        // ���ɧ����O�[��
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
        Name = "�y��";
        BreathCost = Braveattr.Breathstock;
    }

    public override int ModifyDamage(int baseDamage, Braveattr player, MONSTER monster)
    {
        int Mbreath = monster.CurrentBreath;
        // �ˮ`���v
        int damage = Mathf.RoundToInt(baseDamage * 1.3f);
        // ��֩Ǫ���
        monster.CurrentBreath /= 2;
        // �W�[���a��
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
        Name = "�`��";
        BreathCost = Braveattr.Breathstock;
    }

    public override int ModifyDamage(int baseDamage, Braveattr player, MONSTER monster)
    {
        int damage = Mathf.RoundToInt(baseDamage * 1.3f);
        damage = Mathf.Min(damage, monster.CurrentHp - 1);
        // �^�_���a��q
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
        Name = "���F";
        BreathCost = 2 * Braveattr.Breathstock;
    }

    public override int ModifyDamage(int baseDamage, Braveattr player, MONSTER monster)
    {
        int damage = Mathf.RoundToInt(baseDamage * 1.8f);
        // �W�[�Ǫ��h��
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
        Name = "����";
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


