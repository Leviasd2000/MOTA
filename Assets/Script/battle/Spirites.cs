using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Braveattr player; // �Ω�s�x���a�ݩʰѦ�
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
            if (Braveattr.attributes["Breath"] <= lastNumber * 40)
            {
                Sprites.color = new Color32(91, 91, 91, 255); // 255 ��ܧ������z��
            }
            else
            {
                Sprites.color = new Color32(255, 255, 255, 255); // 255 ��ܧ������z��
            }
        }
       
    }
}
