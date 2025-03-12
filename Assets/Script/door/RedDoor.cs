using UnityEngine;
using System.Collections;

public class RedDoor : MonoBehaviour
{
    public Animator animator; // �N Animator �ե��J�����
    public string triggerName = "StartAnimation"; // Animator ����Ĳ�o���W��
    private bool hasPlayed = false; // �����ʵe�O�_�w�g����
    public float animelength = 0.9f;
    private Inventory inventory;
    int key;

    private void Start()
    {
        // �T�O animator �Q���T���t
        animator = GetComponent<Animator>();
        inventory = FindFirstObjectByType<Inventory>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �ˬd�I����H�O�_�O���a�]Tag �� "Player"�^
        if (collision.gameObject.CompareTag("Player") && !hasPlayed)
        {
            key = inventory.GetItemQuantity("���_��");
            Debug.Log(key);
            if (key >= 1)
            {
                // ����ʵe
                animator.SetBool("bump", true);
                Debug.Log("������");

                // ����ʵe�æb�ʵe������P������
                StartCoroutine(DestroyAfterAnimation(animelength));

                // �O���ʵe�w����
                hasPlayed = true;
            }
            inventory.RemoveItem("���_��", 1);
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
