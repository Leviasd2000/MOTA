using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Camera mainCamera;  // �D�n�۾�
    private Transform renderCamTransform;  // Render Camera �� Transform


    void Start()
    {
        renderCamTransform = transform;  // ���{���X��²��
    }

    void LateUpdate()
    {
        // �� Render Camera ��m�M����P Main Camera �@�P
        renderCamTransform.position = mainCamera.transform.position;
        renderCamTransform.rotation = mainCamera.transform.rotation;
    }
}
