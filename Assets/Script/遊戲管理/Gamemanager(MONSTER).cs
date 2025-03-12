using cfg;
using LDtkUnity;
using SimpleJSON;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MainMonster : MonoBehaviour
{
    
    public static List<string> monster_names = new List<string>();
    /// <summary>
    /// 遊戲開始時執行
    /// </summary>
    void Awake()
    {
        List<GameObject> data = new List<GameObject>(); // 用來存儲所有匹配的物件

        var tables = new cfg.Tables(LoadByteBuf);


        data = FindAllMonster();

        foreach (var item in data)
        {
            AddComponentsAndAssignPropertiesMonster(item, tables);
        }

        foreach (var member in tables.Tbmonster.DataList)
        {
           monster_names.Add(member.Id);
        }

        Debug.Log(monster_names[0]+"在哪裡");

    }

    /// <summary>
    /// 加載 JSON 數據
    /// </summary>
    private static JSONNode LoadByteBuf(string file)
    {
        return JSON.Parse(File.ReadAllText(Application.dataPath + "/Data/" + file + ".json", System.Text.Encoding.UTF8));
    }

    /// <summary>
    /// 遍歷場景物件，添加組件並設置屬性
    /// </summary>
    /// 

    public List<GameObject> FindAllMonster()
    {
        List<GameObject> matchedItems = new List<GameObject>(); // 用來存儲所有匹配的物件

        GameObject world = GameObject.Find("World"); // 先找到 World 物體
        if (world == null)
        {
            Debug.LogError("World object not found!");
            return null;
        }

        foreach (Transform level in world.transform) // 遍歷第一層 Level1_x
        {
            Transform entities = level.Find("Entities"); // 查找該層的 "Entities"
            if (entities == null)
            {
                Debug.LogError($"Entities not found in level: {level.name}");
                continue;
            }

            foreach (Transform child in entities) // 遍歷 Entities 下的所有子物體
            {

                LDtkFields goals = child.GetComponent<LDtkFields>();
                if (goals == null)
                {
                    Debug.LogError($"LDtkFields component missing on {child.name}");
                    continue;
                }

                string itemValue = goals.GetString("Monster");

                if (itemValue != null) // 檢查 item 是否匹配
                {
                    Debug.Log($"Monster '{itemValue}' found: {child.name}");
                    matchedItems.Add(child.gameObject);
                }
            }
        }
        if (matchedItems.Count > 0)
        {
            return matchedItems;
        }
        else
        {
            Debug.LogError($"Item is not found!");
            return null; // 如果沒有找到，返回 null
        }
    }

    private void AddComponentsAndAssignPropertiesMonster(GameObject monster, Tables table)
    {
        /// 根據 ID 查找對應的遊戲物件（假設名稱與 item.Id 相同）
        //  GameObject itemObject = FindItem(item.Id);

        LDtkFields fields = monster.GetComponent<LDtkFields>();

        string name = fields.GetString("Monster");

        if (name == null)
        {
            Debug.LogWarning($"找不到名稱為 '{monster}' 的遊戲物件，無法附加屬性！");
            return;
        }

        // 確保該物件上有 ItemComponent
        MONSTER MonsterComponent = monster.GetComponent<MONSTER>();
        if (MonsterComponent == null)
        {
            MonsterComponent = monster.AddComponent<MONSTER>();
        }

        foreach (var member in table.Tbmonster.DataList)
        {
            if (name == member.Id)
            {
                MonsterComponent.SetProperties(member, table);
                Debug.Log($"已為物件 '{member.Id}' 附加屬性: {member}");  // 設定屬性
            }
        }
    }
}
