using UnityEngine;
using System.Collections;

public class Frame1 : MonoBehaviour
{
    public Animator animator; // �N Animator �ե��J�����
    public string triggerName = "StartAnimation"; // Animator ����Ĳ�o���W��
    private bool hasPlayed = false; // �����ʵe�O�_�w�g����
    private AudioManager audiomanager;
    public float animelength = 0.9f;

    private void Start()
    {
        // �T�O animator �Q���T���t
        animator = GetComponent<Animator>();
        audiomanager = FindFirstObjectByType<AudioManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �ˬd�I����H�O�_�O���a�]Tag �� "Player"�^
        if (collision.gameObject.CompareTag("Player") && !hasPlayed)
        {
            // ����ʵe
            animator.SetBool("bump", true);
            Debug.Log("������");

            audiomanager.Play("Door", false);

            // ����ʵe�æb�ʵe������P������
            StartCoroutine(DestroyAfterAnimation(animelength));

            // �O���ʵe�w����
            hasPlayed = true;
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