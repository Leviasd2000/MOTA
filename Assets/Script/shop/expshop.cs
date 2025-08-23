using TMPro;
using UnityEngine;

public class EXPshop : MonoBehaviour
{
    public RectTransform[] battleChoosePos;
    public GameObject chooseAction;
    public GameObject Player;
    public GameObject MShopUI;
    public GameObject EShopUI;
    public TextMeshProUGUI gold;

    private Braveattr player;
    private Braveplayer Playermovement;
    private int k = 0; // 選擇依據
    public AudioManager audioManager;

    private void Awake()
    {
        Debug.Log("有人嗎");
        player = FindFirstObjectByType<Braveattr>();
        if (player != null)
        {
            Player = player.gameObject;
            Playermovement = Player.GetComponent<Braveplayer>();
        }
        else
        {
            Debug.LogError("❌ 找不到 Braveattr，請確認場景中的物件是否存在！");
        }

        if (gold != null)
        {
            gold.text = $"可憐的勇者啊! \n 我將你一路的經驗化為力量! \n告訴我你想要哪種能力。";
        }
        else
        {
            Debug.LogError("❌ gold (TextMeshProUGUI) 未正確綁定！");
        }
    }

    void Start()
    {
        EShopUI = GameObject.Find("MonShop");
        if (EShopUI != null)
        {
            EShopUI.SetActive(false);
        }

        EShopUI = GameObject.Find("ExpShop");
        if (EShopUI != null)
        {
            EShopUI.SetActive(false);
        }
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    void Update()
    {
        if (EShopUI == null || battleChoosePos == null || battleChoosePos.Length == 0)
        {
            return;
        }

        if (EShopUI.activeSelf)
        {
            Player.GetComponent<Braveplayer>().StopCurrentRepeatMovement();
            Player.GetComponent<Braveplayer>().StopMoving();
            Player.GetComponent<Braveplayer>().enabled = false;

            if (Input.GetKeyDown(KeyCode.W))
            {
                k--;
                audioManager.PlaySFX("Click");
                if (k < 0) k = battleChoosePos.Length - 1;
                MoveChooseAction();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                k++;
                audioManager.PlaySFX("Click");
                if (k >= battleChoosePos.Length) k = 0;
                MoveChooseAction();
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                HandleSelection();
            }
        }
    }

    private void MoveChooseAction()
    {
        if (chooseAction == null || battleChoosePos == null || k >= battleChoosePos.Length || battleChoosePos[k] == null)
        {
            Debug.LogError("❌ chooseAction 或 battleChoosePos 錯誤，請檢查 UI 設定！");
            return;
        }

        RectTransform chooseRect = chooseAction.GetComponent<RectTransform>();
        chooseRect.anchoredPosition = battleChoosePos[k].anchoredPosition;
    }

    private void HandleSelection()
    {
        if (player == null)
        {
            Debug.LogError("❌ 玩家物件未初始化！");
            return;
        }

        switch (k)
        {
            case 0:
                TryPurchase("Exp", "Level", 1 , 100);
                break;
            case 1:
                TryPurchase("Exp", "Atk", 1 , 20);
                break;
            case 2:
                TryPurchase("Exp", "Def", 2 , 30);
                break;
            case 3:
                ShopExit();
                break;
        }
    }

    private void TryPurchase(string currency, string stat, int amount , int expense)
    {
        if (Braveattr.GetAttribute(currency) >= expense)
        {
            player.DecreaseAttribute(currency, expense);
            player.IncreaseAttribute(stat, amount);
            audioManager.PlaySFX("Gold");
        }
        else
        {
            Debug.Log("經驗不夠!");
            audioManager.PlaySFX("Stop");
        }
    }

    private void ShopExit()
    {
        k = 0;
        MoveChooseAction();
        audioManager.PlaySFX("Leave");
        EShopUI.SetActive(false);
        Player.GetComponent<Animator>().enabled = true;
        Playermovement.enabled = true;
    }
}

