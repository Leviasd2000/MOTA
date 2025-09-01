using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Braveattr player; // 用於存儲玩家屬性參考
    private Image Sprites;
    private string Names;
    private int lastNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Braveattr>();
        Sprites = GetComponent<Image>();
        lastNumber = int.Parse(gameObject.name[gameObject.name.Length - 1].ToString());
        Debug.Log(lastNumber);
    }

    // Update is called once per frame
    void Update()
    {   
        if (Sprites != null)
        {
            if (Braveattr.attributes["Breath"] <= lastNumber * Braveattr.Breathstock)
            {
                Sprites.color = new Color32(91, 91, 91, 255); // 255 表示完全不透明
            }
            else
            {
                Sprites.color = new Color32(255, 255, 255, 255); // 255 表示完全不透明
            }
        }
       
    }
}
