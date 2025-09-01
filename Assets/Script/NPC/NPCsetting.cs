using LDtkUnity;
using UnityEngine;
using Yarn.Unity;
using static Unity.Collections.Unicode;

public class NPCsetting : MonoBehaviour
{
    LDtkFields goals;
    private string charname;
    private char location; // 樓層數
    private Braveplayer player;
    public ImageLoad ImageLoad;
    public DialogueRunner runner;

    private LineView lineView;
    private RectTransform lineViewRect;
    void Awake()
    {
        runner = FindFirstObjectByType<DialogueRunner>();

        if (runner != null)
        {
            // DialogueRunner 會有一個 dialogueViews 陣列，裡面可能有多種 View
            foreach (var view in runner.dialogueViews)
            {
                if (view is LineView)
                {
                    lineView = view as LineView;
                    lineViewRect = lineView.GetComponent<RectTransform>();
                    Debug.Log("找到 LineView：" + lineView.name);
                }
            }
        }
        else
        {
            Debug.LogWarning("場景中沒有 DialogueRunner！");
        }
    }
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

    public void StartDialogue()
    {

        if (runner != null)
        {
            runner.StartDialogue(charname);
        }
        else
        {
            Debug.LogError("場景中找不到 DialogueRunner，無法開始對話！");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartDialogue();
            player.GetComponent<Braveplayer>().StopCurrentRepeatMovement();
            player.GetComponent<Braveplayer>().StopMoving();
            player.GetComponent<Braveplayer>().enabled = false;

            // 顯示雙方座標
            Vector2 playerPos = other.transform.position;
            Vector2 npcPos = transform.position;
            Debug.Log($"📍 Player 位置: {playerPos}, NPC 位置: {npcPos}");
            // 判斷 y 座標
            if (playerPos.y < 6.5f)
            {
                lineViewRect.anchoredPosition = new Vector2(lineViewRect.anchoredPosition.x, 875f);
            }
            else
            {
                lineViewRect.anchoredPosition = new Vector2(lineViewRect.anchoredPosition.x, 300f);
            }

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
