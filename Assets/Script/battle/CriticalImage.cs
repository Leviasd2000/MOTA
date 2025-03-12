using UnityEngine;
using UnityEngine.UI;

public class CriticalImage : MonoBehaviour
{
    private Braveattr player; // 用於存儲玩家屬性參考
    private Image Critical;
    private string Names;
    private int lastNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Braveattr>();
        Critical = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Critical != null)
        {
            if (Braveattr.attributes["Breath"] <= 40)
            {
                Critical.color = new Color32(91, 91, 91, 255); // 255 表示完全不透明
            }
            else
            {
                Critical.color = new Color32(255, 255, 255, 255); // 255 表示完全不透明
            }
        }

    }
}
