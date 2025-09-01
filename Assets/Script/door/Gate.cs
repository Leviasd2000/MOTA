using System.Collections;
using System.Collections.Generic;
using LDtkUnity;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Animator animator; // 動畫控制器
    public string triggerName = "StartAnimation";
    private bool hasPlayed = false; // 避免重複播放
    public float animelength = 0.9f;
    private Inventory inventory;
    private AudioManager audiomanager;
    private LDtkFields _fields;

    private List<string> unlockResults = new List<string>(); // 存多個 unlock 名稱

    private void Start()
    {
        animator = GetComponent<Animator>();
        inventory = FindFirstObjectByType<Inventory>();
        audiomanager = FindFirstObjectByType<AudioManager>();
        _fields = GetComponent<LDtkFields>();

        if (_fields == null)
        {
            Debug.LogError($"LDtkFields component missing on {gameObject.name}");
            return;
        }

        // 取得全部 unlock 參考
        var entities = _fields.GetEntityReferenceArray("unlock");
        if (entities != null && entities.Length > 0)
        {
            foreach (var entity in entities)
            {
                if (entity == null) continue;
                LDtkIid name = entity.FindEntity();
                string abc = name.ToString();
                string oneResult = abc.Split(' ')[0];
                unlockResults.Add(oneResult);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasPlayed)
        {
            // 檢查所有 unlock 物件是否都不存在
            bool allCleared = true;
            foreach (var res in unlockResults)
            {
                if (GameObject.Find(res) != null)
                {
                    allCleared = false;
                    break;
                }
            }

            if (allCleared)
            {
                animator.SetBool("bump", true);
                Debug.Log("門被撞開");

                audiomanager.PlaySFX("Door");

                StartCoroutine(DestroyAfterAnimation(animelength));
                hasPlayed = true;
            }
            else
            {
                audiomanager.PlaySFX("Stop");
                Debug.Log("門還被鎖住");
            }
        }
    }

    private IEnumerator DestroyAfterAnimation(float animationLength)
    {
        yield return new WaitForSeconds(animationLength);

        Destroy(gameObject); // 銷毀門
        animator.SetBool("bump", false); // 重設動畫狀態
    }
}
