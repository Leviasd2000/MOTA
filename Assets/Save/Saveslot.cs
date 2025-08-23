using UnityEngine;
using UnityEngine.UI;
using TMPro; // 如果你用 TextMeshPro

public class SaveSlot : MonoBehaviour
{
    public int slotIndex;                  // 這個 slot 的編號
    public Button button;                  // Button 本身
    public Image highlightImage;           // 選中時高亮用
    public TMP_Text slotLabel;             // 顯示存檔資訊
    public SaveManager manager;        // 指向 SaveLoadManager

    private void Start()
    {
        highlightImage.gameObject.SetActive(false);
    }
    public void OnClick()
    {
        manager.SelectSlot(slotIndex); // 通知 Manager 選到這個 slot
    }


    // 顯示是否選中
    public void SetHighlight(bool active)
    {
        if (highlightImage != null)
            highlightImage.gameObject.SetActive(active);
    }
}
