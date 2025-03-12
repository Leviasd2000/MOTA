using UnityEngine;
using UnityEngine.UI;

public class CriticalImage : MonoBehaviour
{
    private Braveattr player; // �Ω�s�x���a�ݩʰѦ�
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
                Critical.color = new Color32(91, 91, 91, 255); // 255 ��ܧ������z��
            }
            else
            {
                Critical.color = new Color32(255, 255, 255, 255); // 255 ��ܧ������z��
            }
        }

    }
}
