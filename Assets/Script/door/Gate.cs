using System.Collections;
using System.Collections.Generic;
using LDtkUnity;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Animator animator; // �ʵe���
    public string triggerName = "StartAnimation";
    private bool hasPlayed = false; // �קK���Ƽ���
    public float animelength = 0.9f;
    private Inventory inventory;
    private AudioManager audiomanager;
    private LDtkFields _fields;

    private List<string> unlockResults = new List<string>(); // �s�h�� unlock �W��

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

        // ���o���� unlock �Ѧ�
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
            // �ˬd�Ҧ� unlock ����O�_�����s�b
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
                Debug.Log("���Q���}");

                audiomanager.PlaySFX("Door");

                StartCoroutine(DestroyAfterAnimation(animelength));
                hasPlayed = true;
            }
            else
            {
                audiomanager.PlaySFX("Stop");
                Debug.Log("���ٳQ���");
            }
        }
    }

    private IEnumerator DestroyAfterAnimation(float animationLength)
    {
        yield return new WaitForSeconds(animationLength);

        Destroy(gameObject); // �P����
        animator.SetBool("bump", false); // ���]�ʵe���A
    }
}
