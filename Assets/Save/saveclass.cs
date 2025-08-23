using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEntry
{
    public string id;
    public int quantity;
}

[System.Serializable]
public class Saveclass
{
    public string currentLevel;
    public Vector3 playerPosition;
    public Vector3 maincameraPosition;
    public List<string> deletedEntityIDs = new List<string>();

    /// <summary> (玩家當前樓層)
    public int recentfloor;
    public int maxfloor;
    public int minfloor;
    /// 

    /// <summary> (裝備欄)
    public List<string> Sword;
    public List<string> Shield;
    /// 

    /// <summary> (玩家屬性)
    public int Hp;
    public int Atk;
    public int Def;
    public int Gold;
    public int Exp;
    public int Fatigue;
    public int Breath;
    public int AttackCritical;
    public int DefenseCritical;
    /// 
    
    /// <summary> Inventory 存檔欄位
    public List<ItemEntry> inventory = new List<ItemEntry>();
    ///

}
