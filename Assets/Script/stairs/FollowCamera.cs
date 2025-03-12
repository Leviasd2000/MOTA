using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Camera mainCamera;  // 主要相機
    private Transform renderCamTransform;  // Render Camera 的 Transform


    void Start()
    {
        renderCamTransform = transform;  // 讓程式碼更簡潔
    }

    void LateUpdate()
    {
        // 讓 Render Camera 位置和旋轉與 Main Camera 一致
        renderCamTransform.position = mainCamera.transform.position;
        renderCamTransform.rotation = mainCamera.transform.rotation;
    }
}
