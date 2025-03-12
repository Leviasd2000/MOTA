using LDtkUnity;
using UnityEngine;
using Yarn.Unity;

public class NPCsetting : MonoBehaviour
{
    LDtkFields goals;
    private string charname;
    private char location; // 樓層數
    private Braveplayer player;
    public ImageLoad ImageLoad;
    public DialogueRunner dialogueRunner;
    private void Start()
    {
        if (GetComponent<LDtkFields>() != null)
        {
            goals = GetComponent<LDtkFields>();
            charname = goals.GetString("NPC");
        }
        else
        {
            return;
        }
        player = FindFirstObjectByType<Braveplayer>();
        ImageLoad = GetComponent<ImageLoad>();
    }

    public void StartDialouge()
    {
        FindFirstObjectByType<DialogueRunner>().StartDialogue(charname);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartDialouge();
            player.GetComponent<Braveplayer>().enabled = false;
            player.GetComponent<Animator>().enabled = false;
        }

        Transform grandparent = gameObject.transform.parent?.parent;
        if (grandparent != null)
        {
            location = grandparent.name[^1];
            Debug.Log("祖父物件名稱：" + location);
        }
        else
        {
            Debug.Log("這個物件的層級不夠深，沒有祖父物件！");
        }
    }
    void OnDialogueEnd()
    {
        Debug.Log("🎉 Yarn Spinner 对话结束！");
        // 在这里执行你的操作，例如切换场景、显示 UI、触发事件等
        player.GetComponent<Braveplayer>().enabled = true;
    }
}
