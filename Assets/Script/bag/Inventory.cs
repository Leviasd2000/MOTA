using System.IO;
using UnityEngine;
using SimpleJSON;
using LDtkUnity;
using System.Collections.Generic;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> itemDictionary;
    public List<Slot> slots; // 存放 UI 格子 (Slot) 列表
    /// <summary>
    /// 遊戲開始時執行
    /// </summary>
    void Start()
    {
        var tables = new cfg.Tables(LoadByteBuf);
        UnityEngine.Debug.LogFormat("item[1].name:{0}", tables.Tbitem["紅寶石"].Properties);
        UnityEngine.Debug.Log("== load succ==");
        itemDictionary = new Dictionary<string, int>();
        foreach (var item in tables.Tbitem.DataList)
        {
            UnityEngine.Debug.Log("ItemData ID: " + item.Properties);
            if (item.UseType !=  cfg.item.EUseType.自動使用)
            {
                itemDictionary[item.Id] = 0;
            }
            
        }
        PrintItemDictionary();


    }

    /// <summary>
    /// 加載 JSON 數據
    /// </summary>
    private static JSONNode LoadByteBuf(string file)
    {
        return JSON.Parse(File.ReadAllText(Application.dataPath + "/Data/" + file + ".json", System.Text.Encoding.UTF8));
    }
    /// <summary>
    /// 打印字典內容
    /// </summary>
    private void PrintItemDictionary()
    {
        UnityEngine.Debug.Log("===== Item Dictionary =====");

        foreach (var kvp in itemDictionary)
        {
            string id = kvp.Key;
            int number = kvp.Value;
            UnityEngine.Debug.Log($"ID: {id}, Number: {number}");
        }

        UnityEngine.Debug.Log("===== End of Dictionary =====");
    }
    public void AddItem(string itemName, int quantity)
    {
        Debug.Log(itemName);
        if (itemDictionary.ContainsKey(itemName))
        {
            itemDictionary[itemName] += quantity;
        }
        else
        {
            itemDictionary[itemName] = quantity;
        }
    }
    public void RemoveItem(string itemName, int quantity)
    {
        if (itemDictionary.ContainsKey(itemName) && itemDictionary[itemName]>=quantity)
        {
            itemDictionary[itemName] -= quantity;
        }
        else
        {
            Debug.Log("太少了!");
        }
    }

    public int GetItemQuantity(string itemName)
    {
        return itemDictionary.ContainsKey(itemName) ? itemDictionary[itemName] : 0;
    }

    public Dictionary<string, int> GetItemDictionary()
    {
        return new Dictionary<string, int>(itemDictionary);
    }

    public void ClearAllItems()
    {
        itemDictionary.Clear();
    }

}
