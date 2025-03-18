using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class TextGrowEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float duration = 0.3f;   // 單個字母變大的時間
    public float fadeOutTime = 0.5f; // 整體淡出時間
    public float moveUpDistance = 50f; // 數字上移的距離
    private RectTransform rectTransform;
    private Vector3 initialPosition; // 存儲初始位置

    void Awake()
    {
        // 用物件池管理，或者手動控制顯示/隱藏
        rectTransform = gameObject.GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        initialPosition = rectTransform.localPosition;
        Debug.Log("Local Position: " + initialPosition);
        if (rectTransform == null)
        {
            Debug.LogError("❌ Missing RectTransform component on " + gameObject.name, this);
        }
        if (textMeshPro == null)
        {
            Debug.LogError("❌ Missing TextMeshProUGUI component on " + gameObject.name, this);
        }
    }

    public void ShowDamageText(string damageText)
    {
        if (textMeshPro == null || rectTransform == null)
        {
            Debug.LogError("❌ ShowDamageText called but required components are missing on " + gameObject.name, this);
            return;
        }
        // 顯示數字時，啟用並設定位置
        gameObject.SetActive(true);

        
        // 更新文本
        textMeshPro.text = damageText;
        rectTransform.localPosition = initialPosition;

        // 啟動協程顯示動畫
        StartCoroutine(GrowAndFade());
    }

    IEnumerator GrowAndFade()
    {   
        yield return new WaitForEndOfFrame();
        rectTransform.pivot = new Vector2(0, 1);

        // 先把 UI 縮小到 0
        rectTransform.localScale = Vector3.zero;

        // 讓 UI 在 1 秒內從 0 放大到 1 (正常大小)
        rectTransform.DOScale(1.2f, 0.5f).SetEase(Ease.OutBack);

    }
}
