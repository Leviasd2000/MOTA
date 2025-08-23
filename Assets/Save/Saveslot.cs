using UnityEngine;
using UnityEngine.UI;
using TMPro; // �p�G�A�� TextMeshPro

public class SaveSlot : MonoBehaviour
{
    public int slotIndex;                  // �o�� slot ���s��
    public Button button;                  // Button ����
    public Image highlightImage;           // �襤�ɰ��G��
    public TMP_Text slotLabel;             // ��ܦs�ɸ�T
    public SaveManager manager;        // ���V SaveLoadManager

    private void Start()
    {
        highlightImage.gameObject.SetActive(false);
    }
    public void OnClick()
    {
        manager.SelectSlot(slotIndex); // �q�� Manager ���o�� slot
    }


    // ��ܬO�_�襤
    public void SetHighlight(bool active)
    {
        if (highlightImage != null)
            highlightImage.gameObject.SetActive(active);
    }
}
