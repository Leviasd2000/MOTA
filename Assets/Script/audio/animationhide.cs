using UnityEngine;
using UnityEngine.UI;

public class AutoDestroyAfterAnim : MonoBehaviour
{
    public Animator anim;  // 控制動畫用
    public Image img;      // UI 圖片（可選）

    private void Awake()
    {
        anim = GetComponent<Animator>();
        img = GetComponent<Image>();
    }
    public void Show()
    {
        if (anim != null) anim.enabled = true;
        if (img != null) img.enabled = true;
    }

    public void Hide()
    {
        if (anim != null) anim.enabled = false;
        if (img != null) img.enabled = false;
    }

    public void ApplyNativeSize()
    {
        GetComponent<Image>().SetNativeSize();
    }
}
