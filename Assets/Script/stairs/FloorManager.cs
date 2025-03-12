using UnityEngine;
using UnityEngine.UI;

public class FloorTeleporter : MonoBehaviour
{
    public Camera[] floorCameras; // 每層樓的 Camera
    public RenderTexture[] floorTextures; // 每層樓對應的 RenderTexture
    public RawImage floorPreview; // UI 顯示的畫面
    private int currentFloor = 0; // 當前選擇的樓層

    void Start()
    {
        UpdateFloorPreview(currentFloor);
    }

    // 當玩家選擇樓層時切換畫面
    public void UpFloor()
    {
      currentFloor += 1 ;
      UpdateFloorPreview(currentFloor);
    }

    public void DownFloor()
    {
        currentFloor -= 1;
        UpdateFloorPreview(currentFloor);
    }

    void UpdateFloorPreview(int floorIndex)
    {
        floorPreview.texture = floorTextures[floorIndex]; // 設定 UI 圖像
        Debug.Log("顯示樓層: " + (floorIndex + 1));
    }
}