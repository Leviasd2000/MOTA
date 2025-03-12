using UnityEngine;
using UnityEngine.UI;

public class MONSTERUI : MonoBehaviour
{

    private MONSTER enemy; // �Ω�s�x�Ǫ��ݩʰѦ�
    public Animator animator;
    public GameObject BattleUI;
    public Image battleimage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Animator animator = GetComponent<Animator>();
        battleimage = GetComponent<UnityEngine.UI.Image>();
        Debug.Log("�Q����");
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
        Debug.Log($"GameManager: {enemy.Name} �Q����");
        // �i�H�b�o�̼W�[���ơB��s UI ��Ĳ�o��L�C���޿�
        animator.Play(enemy.Name+"image");
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleUI.activeSelf == false)
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
