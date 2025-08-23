using UnityEngine;
using UnityEngine.UI;

public class AutoDestroyAfterAnim : MonoBehaviour
{
    public Animator anim;  // ����ʵe��
    public Image img;      // UI �Ϥ��]�i��^

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
