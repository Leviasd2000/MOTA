using UnityEngine;
using System.Collections;
using LDtkUnity;
using UnityEditor;

public class Gate : MonoBehaviour
{
    public Animator animator; // �N Animator �ե��J�����
    public string triggerName = "StartAnimation"; // Animator ����Ĳ�o���W��
    private bool hasPlayed = false; // �����ʵe�O�_�w�g����
    public float animelength = 0.9f;
    private Inventory inventory;
    private AudioManager audiomanager;
    private LDtkFields _fields;
    private string result;
    int key;

    private void Start()
    {
        // �T�O animator �Q���T���t
        animator = GetComponent<Animator>();
        inventory = FindFirstObjectByType<Inventory>();
        audiomanager = FindFirstObjectByType<AudioManager>();
        _fields = GetComponent<LDtkFields>();
        if (_fields == null)
        {
            Debug.LogError($"LDtkFields component missing on {gameObject.name}");
            return;
        }
        if (_fields.GetEntityReference("unlock") != null)
        {
            LDtkReferenceToAnEntityInstance entity = _fields.GetEntityReference("unlock");
            LDtkIid name = entity.FindEntity();
            string abc = name.ToString();
            result = abc.Split(' ')[0];
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �ˬd�I����H�O�_�O���a�]Tag �� "Player"�^
        if (collision.gameObject.CompareTag("Player") && !hasPlayed)
        {   
            print(result);
            if (GameObject.Find(result) == null)
            {
                // ����ʵe
                animator.SetBool("bump", true);
                Debug.Log("������");

                audiomanager.PlaySFX("Door");

                // ����ʵe�æb�ʵe������P������
                StartCoroutine(DestroyAfterAnimation(animelength));

                // �O���ʵe�w����
                hasPlayed = true;
            }
            else
            {
                audiomanager.PlaySFX("Stop");
            }


        }
    }

    private IEnumerator DestroyAfterAnimation(float animationLength)
    {
        // ���ݰʵe���񧹦�
        yield return new WaitForSeconds(animationLength);

        // �P������
        Destroy(gameObject);

        // �ʵe���񵲧���N bump �]�m�� false
        animator.SetBool("bump", false);
    }
}


