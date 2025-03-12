using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using cfg.monster;
using System.IO;
using static UnityEngine.GraphicsBuffer;
using System.Runtime.CompilerServices;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public enum BattleState
    {
        Start, PlayerTurn, MonsterTurn, Win, Lose, Escape
        // 枚舉切換狀態
    }
    public BattleState state;

    public GameObject MonsterInstance;
    public GameObject BraveInstance;

    public TextMeshProUGUI myText;
    public TextMeshProUGUI Escape;
    public TextMeshProUGUI CriticalOK;
    public TextMeshProUGUI SwordOK;
    public TextMeshProUGUI ShieldOK;
    public UIManager UI;
    
    public Transform[] battleChoosePos;
    public GameObject chooseAction;
    public GameObject choosePlane;
    public GameObject BattleUI;
    public GameObject VictoryUI;
    private GameObject Teki;
    public GameObject Player;
    private bool Monstercritical;
    
    private Braveattr player; // 用於存儲玩家屬性參考
    private MONSTER enemy; // 用於存儲怪物屬性參考
    private GameObject teki;

    public GameObject canvasTransform;  // Canvas 的 Transform，放置怪物動畫
    public bool critic; // 本回合是否按過爆擊


    private void Awake()
    {
        Debug.Log("有人嗎");
        player = FindFirstObjectByType<Braveattr>();
        Player = player.gameObject;
        Debug.Log(player);
    }
    void Start()
    {
        if (myText == null)
        {
            Debug.LogError("dialogText 未指派！請在 Inspector 內拖曳 TextMeshPro UI 物件");
            return;
        }

        myText.text = "要開始了";  // 設定文字

        BattleUI = GameObject.Find("BattleUI");
        if (BattleUI != null )
        {
            BattleUI.SetActive(false);
        }

        VictoryUI = GameObject.Find("Victory");
        if (BattleUI != null)
        {
            VictoryUI.SetActive(false);
        }

        
        Monstercritical = false; // 初始化
        critic = false;
        CriticalOK.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 按下 Q 鍵時，觸發逃跑
        if (Input.GetKeyDown(KeyCode.Q) && Escape.gameObject.activeSelf)
        {
            EscapeBattle();
        }

        // 按下 Enter 鍵時，觸發逃跑
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (state == BattleState.Win)
            {
                CheckEnter();
            }
        }

        // 按下 C 鍵時，觸發爆擊
        if (Input.GetKeyDown(KeyCode.C) && Braveattr.GetAttribute("Breath")>=40)
        {
            if (critic == false)
            {
                critic = true;
                player.DecreaseAttribute("Breath", 40);
                Debug.Log("爆擊");
                CriticalOK.gameObject.SetActive(true);
            }
        }


    }

    private void OnEnable()
    {
        MONSTER.OnEnemyDefeated += HandleEnemyDefeated;
    }

    private void OnDisable()
    {
        MONSTER.OnEnemyDefeated -= HandleEnemyDefeated;
    }

    private void HandleEnemyDefeated(GameObject teki)
    {
        enemy = teki.GetComponent<MONSTER>();
        Debug.Log($"GameManager: {enemy.Name} 被擊敗");
        // 可以在這裡增加分數、更新 UI 或觸發其他遊戲邏輯
        StartBattle(teki);
        Teki = teki;
    }

    private void StartBattle (GameObject monster)
    {
        myText.gameObject.SetActive(true);
        Escape.gameObject.SetActive(true);
        MONSTER enemy = monster.GetComponent<MONSTER>();
        Debug.Log(enemy.name);
        Debug.Log("又一場");

        Braveplayer Playermovement = Player.GetComponent<Braveplayer>();
        Player.GetComponent<Animator>().enabled = false;
        if (Playermovement != null)
        {
            Playermovement.enabled = false;
            Debug.Log("禁止移動");
        }

        StartCoroutine(UI.InitHUDBrave(player));
        StartCoroutine(UI.InitHUDMonster(enemy));

        if (enemy.System.Contains("先攻"))
        {
            state = BattleState.MonsterTurn;
            StartCoroutine(MonsterTurn());
            Debug.Log("怪物先攻!");
        }
        else
        {
            state = BattleState.PlayerTurn;
            PlayerTurn();
            Debug.Log("玩家先攻!");
            StartCoroutine(PlayerAttack());
            // BattleChoose();
        }
    }

    private void PlayerAttackcalculate(bool critical)
    {
        int dm = enemy.MonsterDefense_Damage(Braveattr.attributes["Atk"], enemy.System, critical); // 傷害
        Debug.Log(dm);
        int cm = (enemy.Def * 10) / Braveattr.attributes["Atk"]; // 氣息
        enemy.CurrentHp -= dm;
        enemy.CurrentBreath += dm / 3;
        player.IncreaseAttribute("Breath", cm);
        UI.MonsterUpProperties(enemy);
        UI.BraveUpProperties(player);
        UI.BraveUpdateBreath(player);
        UI.MonsterUpdateBreath(enemy);
    }

    private void MonsterAttackcalculate( bool critical )
    {
        int dm = player.BraveDefense_Damage(enemy.Atk , critical , enemy.System); // 傷害
        int cm = (enemy.Atk - Braveattr.attributes["Def"]) / 10 ; // 氣息
        enemy.CurrentBreath += (Braveattr.attributes["Def"] * 10) / enemy.Atk;
        player.DecreaseAttribute("Hp", dm);
        player.IncreaseAttribute("Breath", cm);
        Debug.Log(enemy.CurrentBreath);
        UI.MonsterUpProperties(enemy);
        UI.BraveUpProperties(player);
        UI.BraveUpdateBreath(player);
        UI.MonsterUpdateBreath(enemy);
    }



    private void PlayerTurn()
    {
        Debug.Log("換我了");
    }

    
    private IEnumerator PlayerAttack()
    {
        myText.text = "勇者對" + enemy.name + "使用了攻擊!";
        myText.ForceMeshUpdate(); // 強制更新 UI
        yield return new WaitForSeconds(0.5f);
        PlayerAttackcalculate(critic);
        yield return new WaitForSeconds(0.5f); // 等待 UI 更新
        CriticalOK.gameObject.SetActive(false);
        Debug.Log(enemy.CurrentHp);

        if (enemy.CurrentHp <= 0)
        {
            state = BattleState.Win;
            StartCoroutine(BattleEnd());
        }
        else
        {
            critic = false;
            state = BattleState.MonsterTurn;
            StartCoroutine(NextTurn());
        }
    }

    private IEnumerator MonsterTurn()
    {
        myText.text = enemy.name + "對" + player.name + "使用了攻擊!";
        yield return new WaitForSeconds(0.5f);
        MonsterAttackcalculate(Monstercritical);
        yield return new WaitForSeconds(0.5f); // 等待 UI 更新
        if (enemy.CurrentBreath > enemy.Breath)
        {
            enemy.CurrentBreath -= enemy.Breath;
            Monstercritical = true;
        }

        if (Braveattr.attributes["Hp"] <= 0)
        {
            state = BattleState.Lose;
            StartCoroutine(BattleEnd());
        }
        else
        {
            // 不要在這裡直接調用 PlayerAttack()
            state = BattleState.PlayerTurn;
            StartCoroutine(NextTurn());
        }
    }

    // 用這個來處理回合轉換，避免遞歸問題
    private IEnumerator NextTurn()
    {
        yield return new WaitForSeconds(0f);

        if (state == BattleState.PlayerTurn)
        {
            choosePlane.SetActive(true); // 顯示選擇介面
            StartCoroutine(PlayerAttack());
        }
        else if (state == BattleState.MonsterTurn)
        {
            StartCoroutine(MonsterTurn());
        }
    }
    public void EscapeBattle()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        StopAllCoroutines(); // 確保戰鬥結束
        Refresh(enemy);
        state = BattleState.Escape;
        StartCoroutine(BattleEnd());
    }

    public void Refresh(MONSTER monster)
    {
        monster.CurrentHp = monster.Hp;
        monster.CurrentBreath = 0;
    }

    public void CheckEnter()
    {
        VictoryUI.SetActive(false);
        BattleUI.SetActive(false);
    }


    private IEnumerator BattleEnd()
    {
        yield return new WaitForSeconds(1f); // 增加一點時間感
        Escape.gameObject.SetActive(false);
        critic = false; // 重置爆擊狀態
        CriticalOK.gameObject.SetActive(false);

        if (state == BattleState.Win)
        {
            myText.text = "現在是" + player.name + "的勝利!";
            // 戰鬥完消滅怪物
            Debug.Log("勝利!");
            Pamphlet.Refresh(Teki); // 重製怪物手冊資料
            Destroy(Teki);
            myText.SetText("");
            VictoryUI.SetActive(true);
            player.IncreaseAttribute("Gold", enemy.Gold);
            player.IncreaseAttribute("Exp", enemy.Exp);
            

        }
        if (state == BattleState.Lose)
        {
            myText.text = "You Dead";
            Debug.Log("失敗!");
        }
        if (state == BattleState.Escape)
        {
            myText.text = "快逃!";
            Debug.Log("逃跑!");
            BattleUI.SetActive(false);
        }

        Braveplayer Playermovement = player.GetComponent<Braveplayer>();
        if (Playermovement != null)
        {
            Playermovement.enabled = true;
            Debug.Log("勇者的移動被恢復了");
        }

        Player.GetComponent<Animator>().enabled = true;
    }
}

