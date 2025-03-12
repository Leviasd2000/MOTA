using System;
using LDtkUnity;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TEST : MonoBehaviour
{
    public GameObject FindItem(string itemName)
    {
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

                string itemValue = goals.GetString("item");

                if (itemValue == itemName) // 檢查 item 是否匹配
                {
                    Debug.LogError($"Item '{itemName}' found: {child.name}");
                    return child.gameObject; // 找到後立即返回該 GameObject
                }
            }
        }

        Debug.LogError($"Item '{itemName}' not found!");
        return null; // 如果沒有找到，返回 null
    }
}
