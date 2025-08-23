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
    public static int CalculateDamage(MONSTER mon , Braveattr brave , bool critical , string turn)
    {
        int Matk = mon.Atk;
        int Mdef = mon.Def;
        int Batk = Braveattr.GetAttribute("Atk");
        int Bdef = Braveattr.GetAttribute("Def");
        string HoldSword = Braveattr.HoldSword;
        string HoldShield = Braveattr.HoldShield;
        bool ReSword = Braveattr.RecentSword;
        bool ReShield = Braveattr.RecentShield;

        if (turn == "Brave"){
            if ( ReSword == true){
                Sword sword = SwordFactory.Create(Braveattr.HoldSword, brave, mon);
                if (sword != null)
                {
                    int damage = sword.Damage(brave, mon);
                    Debug.Log($"�ϥμC {sword.Name} ��Ǫ��y�� {damage} �ˮ`");
                    return damage;
                }
                else
                {
                    Debug.LogWarning("�C�Ыإ���");
                    return 0;
                }
            }
            else{
                if (Batk + Braveattr.AttackCritical > Mdef){      
                    if (critical){
                        return Mathf.Max(2 * (Batk - Mdef) , 1);
                    }
                    else{
                        return Mathf.Max((Batk - Mdef), 1);
                    }
                }
                else{
                    return 0;
                }
            }
        }
        else{
            if (ReShield == true)
            {
                Shield shield = ShieldFactory.Create(Braveattr.HoldShield, brave, mon);
                if (shield != null)
                {
                    int damage = shield.Damage(brave, mon);
                    Debug.Log($"�ϥά� {shield.Name} �Q�Ǫ��y�� {damage} �ˮ`");
                    return damage;
                }
                else
                {
                    Debug.LogWarning("�޳Ыإ���");
                    return 0;
                }
            }
            else
            {
                if (mon.System.Contains("�]�k����"))
                {
                    if (critical)
                    {
                        return Mathf.Max(2 * (Matk - Bdef), 1);
                    }
                    else
                    {
                        return Mathf.Max((Matk - Bdef), 1);
                    }
                }
                else{
                    if ((Matk + Braveattr.DefenseCritical) > Bdef)
                    {
                        if (critical)
                        {
                            return Mathf.Max(2 * (Matk - Bdef), 1);
                        }
                        else
                        {
                            return Mathf.Max((Matk - Bdef), 1);
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

    }
    


}
