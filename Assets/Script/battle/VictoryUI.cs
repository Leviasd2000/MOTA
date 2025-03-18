using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class VictoryUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public RectTransform imageRect; // UI Image 的 RectTransform
    public GameObject TextContainer;             // 要顯示的文字
    public float moveDuration = 0.3f; // 移動時間

    void OnEnable()
    {
        // 先隱藏文字
        TextContainer.SetActive(false);

        // 設定圖片初始位置（往上偏移）
        float startY = imageRect.anchoredPosition.y + 88; // 可調整距離
        imageRect.anchoredPosition = new Vector2(imageRect.anchoredPosition.x, startY);

        // 讓圖片下降到原本位置，然後顯示文字
        imageRect.DOAnchorPosY(imageRect.anchoredPosition.y - 88, moveDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // 當動畫結束後顯示文字
                TextContainer.gameObject.SetActive(true);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
