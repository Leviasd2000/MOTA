using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class Braveattr : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static Dictionary<string, int> attributes = new Dictionary<string, int>();
    public TextMeshProUGUI bravename;
    public GameObject currentMonster;  // 記錄當前接觸的怪物

    public static List<string> Sword;
    public static List<string> Shield;

    public static bool Critic;
    public static bool RecentSword;
    public static bool RecentShield;
    public static string HoldSword;
    public static string HoldShield;

    public static Sword EquippedSword;
    public static Shield EquippedShield;

    public float actionSpeed;

    public static int AttackCritical;
    public static int DefenseCritical;

    public string Name { get; private set; }
    public string Icon { get; private set; }
    public List<int> Properties { get; private set; }
    public cfg.ItemData[] Items { get; private set; }
    public string System { get; private set; }
    public string Skilltips { get; private set; }
    public string Desc { get; private set; }

    public int Hp { get; private set; }
    public int Atk { get; private set; }
    public int Def { get; private set; }
    public int Gold { get; private set; }
    public int Exp { get; private set; }
    public int Fatigue { get; private set; }
    public int Breath { get; private set; }
    
    public static int Tempatk { get; set; } // 臨時加(減)攻
    public static int Tempdef { get; set; } // 臨時加(減)防

    public static int Tempbreath { get; set; } // 臨時加(減)氣息

    public static int Freeze { get; set; } // 等待回合

    public static int Breathstock { get; set; } // 一格氣息的量 


    void Start()
    {
        // 設定初始值
        attributes["Level"] = 1;
        attributes["Hp"] = 1000;
        attributes["Atk"] = 100;
        attributes["Def"] = 10;
        attributes["Gold"] = 0;
        attributes["Exp"] = 0;
        attributes["Fatigue"] = 0;
        attributes["Breath"] = 120;
        Sword = new List<string> { "None" }; 
        Shield = new List<string> { "None" };
        HoldSword = "None";
        HoldShield = "None";
        RecentSword = false;
        RecentShield = false;
        
        actionSpeed = 2;
        AttackCritical = 25;
        DefenseCritical = 35;
        Tempatk = 0;
        Tempdef = 0;
        Tempbreath = 0;
        Freeze = 0;
        Breathstock = 40;
        Critic = false;

        // 測試: 讀取屬性
        Debug.Log("勇者等級: " + GetAttribute("Level"));
    }

    // 設定屬性
    public static void SetAttribute(string type, int value)
    {
        attributes[type] = value;
    }

    // 取得屬性
    public static int GetAttribute(string type)
    {
        return attributes.ContainsKey(type) ? attributes[type] : 0;
    }

    // 增加屬性值
    public void IncreaseAttribute(string type, int amount)
    {
        if (attributes.ContainsKey(type))
        {
            attributes[type] += amount;
            if (type == "Level")
            {
                attributes["Hp"] += 400;
                attributes["Atk"] += 3;
                attributes["Def"] += 3;
            }
        }
    }

    public void DecreaseAttribute(string type, int amount)
    {
        if (attributes.ContainsKey(type) && attributes[type] >= amount)
        {
            attributes[type] -= amount;
        }
        else
        {
            attributes[type] = 0;
            // 播放死亡動畫
        }
    }

    public int BraveAttack_Damage(int monster_def , string skill , string Sword)
    {   
        if (Sword != null)
        {
            return 0;
        }
        else
        {
            return attributes["Atk"]- monster_def ;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
