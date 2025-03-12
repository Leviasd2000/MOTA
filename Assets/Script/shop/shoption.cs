using TMPro;
using UnityEngine;

public class shoption : MonoBehaviour
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
    public int expense; // 花費金錢

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

        expense = 20;

        if (gold != null)
        {
            gold.text = $"可憐的勇者啊! \n 如果你願意給我 {expense} 金幣的話 \n 我可以幫你提升基礎能力值!";
        }
        else
        {
            Debug.LogError("❌ gold (TextMeshProUGUI) 未正確綁定！");
        }
    }

    void Start()
    {
        MShopUI = GameObject.Find("MonShop");
        if (MShopUI != null)
        {
            MShopUI.SetActive(false);
        }

        EShopUI = GameObject.Find("ExpShop");
        if (EShopUI != null)
        {
            EShopUI.SetActive(false);
        }
    }

    void Update()
    {
        if (MShopUI == null || battleChoosePos == null || battleChoosePos.Length == 0)
        {
            return;
        }

        if (MShopUI.activeSelf)
        {
            Player.GetComponent<Animator>().enabled = false;
            Playermovement.enabled = false;

            if (Input.GetKeyDown(KeyCode.W))
            {
                k--;
                if (k < 0) k = battleChoosePos.Length - 1;
                MoveChooseAction();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                k++;
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
                TryPurchase("Gold", "Hp", 400);
                break;
            case 1:
                TryPurchase("Gold", "Atk", 3);
                break;
            case 2:
                TryPurchase("Gold", "Def", 3);
                break;
            case 3:
                ShopExit();
                break;
        }
    }

    private void TryPurchase(string currency, string stat, int amount)
    {
        if (Braveattr.GetAttribute(currency) >= expense)
        {
            player.DecreaseAttribute(currency, expense);
            expense += 1;
            player.IncreaseAttribute(stat, amount);
            RefreshGoldText();
        }
        else
        {
            Debug.Log("錢不夠!");
        }
    }

    private void ShopExit()
    {
        k = 0;
        MoveChooseAction();
        MShopUI.SetActive(false);
        Player.GetComponent<Animator>().enabled = true;
        Playermovement.enabled = true;
    }

    private void RefreshGoldText()
    {
        if (gold != null)
        {
            gold.text = $"可憐的勇者啊! \n 如果你願意給我 {expense} 金幣的話 \n 我可以幫你提升基礎能力值!";
        }
    }
}
