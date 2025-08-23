using UnityEngine;
using UnityEngine.UI;

public class PamphletUI : MonoBehaviour
{

    private MONSTER enemy; // 用於存儲怪物屬性參考
    public Animator animator;
    public GameObject pamphlet;
    public Image battleimage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Animator animator = GetComponent<Animator>();
        battleimage = GetComponent<UnityEngine.UI.Image>();
        Debug.Log("被播放");
    }

    private void OnEnable()
    {
        MONSTER.OnEnemyDefeated += PlayEnemy;
    }

    private void OnDisable()
    {
        MONSTER.OnEnemyDefeated -= PlayEnemy;
    }

    private void PlayEnemy(GameObject teki)
    {
        animator.gameObject.SetActive(true);
        enemy = teki.GetComponent<MONSTER>();
        Debug.Log($"GameManager: {enemy.Name} 被播放");
        // 可以在這裡增加分數、更新 UI 或觸發其他遊戲邏輯
        animator.Play(enemy.Name + "image");
    }

    // Update is called once per frame
    void Update()
    {
        if (pamphlet.activeSelf == false)
        {
            animator.enabled = false;
            battleimage.enabled = false;
        }
        else
        {
            animator.enabled = true;
            battleimage.enabled = true;
        }
    }
}

