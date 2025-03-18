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
    public string Sword ;
    public string Shield ;
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



    void Start()
    {
        // 設定初始值
        attributes["Level"] = 1;
        attributes["Hp"] = 1000;
        attributes["Atk"] = 10;
        attributes["Def"] = 10;
        attributes["Gold"] = 0;
        attributes["Exp"] = 0;
        attributes["Fatigue"] = 0;
        attributes["Breath"] = 0;
        Sword = "None";
        Shield = "None";
        actionSpeed = 2;
        AttackCritical = 5;
        DefenseCritical = 45;

        // 測試: 讀取屬性
        Debug.Log("勇者等級: " + GetAttribute("Level"));
    }

    // 設定屬性
    public void SetAttribute(string type, int value)
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
            attributes[type] += amount;
    }

    public void DecreaseAttribute(string type, int amount)
    {
        if (attributes.ContainsKey(type) && attributes[type]>=amount)
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

    public int BraveDefense_Damage(int monster_atk , bool critical , string skill )
    {
        if (critical == true)
        {
            return 2*(monster_atk - attributes["Def"]);
        }
        else
        {
            return monster_atk - attributes["Def"];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
