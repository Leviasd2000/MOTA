using UnityEngine;
using UnityEngine.UI;

public class FloorTeleporter : MonoBehaviour
{
    public Camera[] floorCameras; // �C�h�Ӫ� Camera
    public RenderTexture[] floorTextures; // �C�h�ӹ����� RenderTexture
    public RawImage floorPreview; // UI ��ܪ��e��
    private int currentFloor = 0; // ��e��ܪ��Ӽh

    void Start()
    {
        UpdateFloorPreview(currentFloor);
    }

    // ���a��ܼӼh�ɤ����e��
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
        floorPreview.texture = floorTextures[floorIndex]; // �]�w UI �Ϲ�
        Debug.Log("��ܼӼh: " + (floorIndex + 1));
    }
}