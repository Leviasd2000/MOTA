using System.Threading;
using cfg.monster;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;

public static class BattleCalculate  
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static int CalculateDamage(MONSTER mon, Braveattr brave, string turn, bool critical)
    {
        if (turn == "Brave")
        {   
            if (critical)
            {
                brave.DecreaseAttribute("Breath", Braveattr.Breathstock);
                brave.IncreaseAttribute("Fatigue", 10); 
            }
            else
            {
                brave.IncreaseAttribute("Fatigue", 1);
            }
            return CalculateBraveDamage(mon, brave, critical);

        }
        else
        {
            if (critical)
            {
                mon.CurrentBreath -= mon.Breath;
                mon.CurrentFatigue += 5;
            }
            else
            {
                mon.CurrentFatigue += 1;
            }
            return CalculateMonsterDamage(mon, brave, critical);
        }
    }

    // ------------------------
    // 勇者攻擊怪物
    // ------------------------
    private static int CalculateBraveDamage(MONSTER mon, Braveattr brave, bool critical)
    {
        int basedamage = 0;

        if (Braveattr.AttackCritical + Braveattr.GetAttribute("Atk") > mon.Def)
        {
            basedamage = Mathf.Max(Braveattr.GetAttribute("Atk") - mon.Def , 1);
        }
        else
        {
            basedamage = 0;
        }
        
        if (Braveattr.RecentSword)
        {
            Sword sword = SwordFactory.Create(Braveattr.HoldSword, brave, mon);
            return sword != null ? sword.ModifyDamage(basedamage, brave, mon) : 0;
        }

        return ApplyPhysicalDamage(
            attackerAtk: Braveattr.GetAttribute("Atk"),
            defenderDef: mon.Def,
            critical: critical,
            criticalBonus: Braveattr.AttackCritical
        );
    }

    // ------------------------
    // 怪物攻擊勇者
    // ------------------------
    private static int CalculateMonsterDamage(MONSTER mon, Braveattr brave, bool critical)
    {
        // 1️ 先計算基礎傷害（不管盾牌）
        int baseDamage = CalculateBaseMonsterDamage(mon, brave, critical);

        // 2️ 套用盾牌效果（只對物理或指定攻擊類型有效）
        if (Braveattr.RecentShield)
        {
            Shield shield = ShieldFactory.Create(Braveattr.HoldShield, brave, mon);
            if (shield != null)
            {
                baseDamage = shield.ModifyDamage(baseDamage, mon);
            }
        }

        return baseDamage;
    }

    // ------------------------
    // 計算怪物不同攻擊類型的基礎傷害
    // ------------------------
    private static int CalculateBaseMonsterDamage(MONSTER mon, Braveattr brave, bool critical)
    {
        if (mon.System.Contains("魔法攻擊"))
        {
            return ApplyMagicDamage(mon.Atk, critical);
        }
        else if (mon.System.Contains("毒攻擊"))
        {
            return ApplyPoisonDamage(mon.Atk, critical);
        }
        else
        {
            // 普通物理攻擊
            return ApplyPhysicalDamage(
                attackerAtk: mon.Atk,
                defenderDef: Braveattr.GetAttribute("Def"),
                critical: critical,
                criticalBonus: Braveattr.DefenseCritical
            );
        }
    }

    // ------------------------
    // 共用計算方法
    // ------------------------
    private static int ApplyPhysicalDamage(int attackerAtk, int defenderDef, bool critical, int criticalBonus)
    {
        if (attackerAtk + criticalBonus <= defenderDef) return 0;
        int damage = attackerAtk - defenderDef;
        return critical ? Mathf.Max(damage * 2, 1) : Mathf.Max(damage, 1);
    }

    private static int ApplyMagicDamage(int matk, bool critical)
    {
        int damage = matk;
        return critical ? Mathf.Max(damage * 2, 1) : Mathf.Max(damage, 1);
    }

    private static int ApplyPoisonDamage(int matk, bool critical)
    {
        int damage = Mathf.RoundToInt(matk * 0.5f); // 毒攻50% atk
        return critical ? Mathf.Max(damage * 2, 1) : Mathf.Max(damage, 1);
    }



}
