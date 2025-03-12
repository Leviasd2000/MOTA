using UnityEngine;
using System.Collections;

public class Frame1 : MonoBehaviour
{
    public Animator animator; // 將 Animator 組件拖入此欄位
    public string triggerName = "StartAnimation"; // Animator 中的觸發器名稱
    private bool hasPlayed = false; // 紀錄動畫是否已經播放
    public float animelength = 0.9f;

    private void Start()
    {
        // 確保 animator 被正確分配
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 檢查碰撞對象是否是玩家（Tag 為 "Player"）
        if (collision.gameObject.CompareTag("Player") && !hasPlayed)
        {
            // 播放動畫
            animator.SetBool("bump", true);
            Debug.Log("有撞到");

            // 播放動畫並在動畫結束後銷毀物件
            StartCoroutine(DestroyAfterAnimation(animelength));

            // 記錄動畫已播放
            hasPlayed = true;
        }
    }

    private IEnumerator DestroyAfterAnimation(float animationLength)
    {
        // 等待動畫播放完成
        yield return new WaitForSeconds(animationLength);

        // 銷毀物件
        Destroy(gameObject);

        // 動畫播放結束後將 bump 設置為 false
        animator.SetBool("bump", false);
    }
}