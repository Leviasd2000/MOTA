using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI; // 引入 UI 命名空间

public class Fade : MonoBehaviour
{
    public Image fadeImage; // 绑定到 Canvas 下的 Image
    private TextMeshProUGUI[] childImages; // 用来存储所有子物体的 Image
    [SerializeReference] private float fadeDuration = 1f; // 淡入/淡出的持续时间
    [SerializeReference] private float fadeDurationText = 0.5f; // 淡入/淡出的持续时间
    private Coroutine fadeCoroutine; // 用于保存当前的协程
    public void Start()
    {
        childImages = fadeImage.GetComponentsInChildren<TextMeshProUGUI>();
        fadeImage.enabled = false;
        foreach (var childImage in childImages)
        {
            childImage.enabled = false;
        }
    }

    // 开始淡入效果
    public void FadeIn()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeImage.enabled = true;
        foreach (var childImage in childImages)
        {
            childImage.enabled = true;
        }

        // 同时启动 fadeImage 和 TextMeshProUGUI 的渐变
        fadeCoroutine = StartCoroutine(FadeInOut(1f, 0f));
    }

    // 开始淡出效果
    public void FadeOut()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeImage.enabled = true;
        foreach (var childImage in childImages)
        {
            childImage.enabled = true;
        }

        // 同时启动 fadeImage 和 TextMeshProUGUI 的渐变
        fadeCoroutine = StartCoroutine(FadeInOut(0f, 1f));
    }

    // 淡入和淡出的同时进行
    private IEnumerator FadeInOut(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        float elapsedTimeText = 0f;


        // 处理 fadeImage 的透明度变化
        Color fadeImageColor = fadeImage.color;
        fadeImageColor.a = startAlpha;
        fadeImage.color = fadeImageColor;

        // 处理所有 TextMeshProUGUI 子物体的透明度变化
        Color[] childColors = new Color[childImages.Length];
        for (int i = 0; i < childImages.Length; i++)
        {
            childColors[i] = childImages[i].color;
            childColors[i].a = startAlpha;
            childImages[i].color = childColors[i];
        }

        // 在指定的持续时间内渐变透明度
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            elapsedTimeText = Mathf.Min(elapsedTime, fadeDurationText);

            // 平滑变化 fadeImage 的透明度
            fadeImageColor.a = Mathf.Lerp(startAlpha, endAlpha, (elapsedTime / fadeDuration) * (elapsedTime / fadeDuration));
            fadeImage.color = fadeImageColor;

            // 平滑变化每个子物体的透明度
            for (int i = 0; i < childImages.Length; i++)
            {
                childColors[i].a = Mathf.Lerp(startAlpha, endAlpha, elapsedTimeText / fadeDurationText);
                childImages[i].color = childColors[i];
            }

            yield return null;
        }

        // 确保透明度达到目标值
        fadeImageColor.a = endAlpha;
        fadeImage.color = fadeImageColor;

        for (int i = 0; i < childImages.Length; i++)
        {
            childColors[i].a = endAlpha;
            childImages[i].color = childColors[i];
        }
    }
}