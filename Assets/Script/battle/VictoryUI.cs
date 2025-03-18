using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class VictoryUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public RectTransform imageRect; // UI Image �� RectTransform
    public GameObject TextContainer;             // �n��ܪ���r
    public float moveDuration = 0.3f; // ���ʮɶ�

    void OnEnable()
    {
        // �����ä�r
        TextContainer.SetActive(false);

        // �]�w�Ϥ���l��m�]���W�����^
        float startY = imageRect.anchoredPosition.y + 88; // �i�վ�Z��
        imageRect.anchoredPosition = new Vector2(imageRect.anchoredPosition.x, startY);

        // ���Ϥ��U����쥻��m�A�M����ܤ�r
        imageRect.DOAnchorPosY(imageRect.anchoredPosition.y - 88, moveDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // ��ʵe��������ܤ�r
                TextContainer.gameObject.SetActive(true);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
