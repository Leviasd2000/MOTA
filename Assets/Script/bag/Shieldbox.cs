using cfg.monster;
using UnityEngine;

public class Shieldbox : MonoBehaviour
{
    public RectTransform[] battleChoosePos;
    public GameObject chooseAction;
    public GameObject Player;
    private Braveattr player;
    private Braveplayer Playermovement;
    public GameObject ShieldUI;
    private int k = 0; // 選擇依據
    public AudioManager audioManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ShieldUI == null || battleChoosePos == null || battleChoosePos.Length == 0)
        {
            return;
        }

        if (ShieldUI.activeSelf)
        {
            Player.GetComponent<Braveplayer>().StopCurrentRepeatMovement();
            Player.GetComponent<Braveplayer>().StopMoving();
            Player.GetComponent<Braveplayer>().enabled = false;

            if (Input.GetKeyDown(KeyCode.A))
            {
                k--;
                audioManager.PlaySFX("Click");
                if (k < 0) k = battleChoosePos.Length - 1;
                MoveChooseAction();
            }
            if (Input.GetKeyDown(KeyCode.D))
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
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ShopExit();
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
                ChangeShield("鏡膜");
                break;
            case 1:
                ChangeShield("結晶");
                break;
            case 2:
                ChangeShield("反射");
                break;
            case 3:
                ChangeShield("精靈");
                break;
            case 4:
                ChangeShield("賢者");
                break;
        }
    }

    private void ChangeShield(string name)
    {
        if (Braveattr.Shield.Contains(name))
        {
            Braveattr.HoldShield = name;
            audioManager.PlaySFX("Click");
        }
        else
        {
            Debug.Log("未擁有這個盾");
            audioManager.PlaySFX("Stop");
        }
    }

    private void ShopExit()
    {
        k = 0;
        MoveChooseAction();
        audioManager.PlaySFX("Leave");
        ShieldUI.SetActive(false);
        Player.GetComponent<Animator>().enabled = true;
        Playermovement.enabled = true;
    }


}
