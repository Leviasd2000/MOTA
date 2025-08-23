using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using cfg.monster;
using Unity.VisualScripting;


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
    public Braveplayer Playermovement;

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
    private int Braveturn; // 勇者回合數
    private int Monsterturn; // 怪物回合數
    private int Bravefreeze; // 勇者暫停回合數
    private int Monsterfreeze; // 怪物暫停回合數

    private Braveattr player; // 用於存儲玩家屬性參考
    private MONSTER enemy; // 用於存儲怪物屬性參考
    private GameObject teki;

    public GameObject canvasTransform;  // Canvas 的 Transform，放置怪物動畫
    public bool critic; // 本回合是否按過爆擊
    private bool animeend = false; // 等待動畫播放完畢(enter)
    public AudioManager audioManager;


    [Header("動畫特效")]
    private Dictionary<string ,AnimationClip> loadedclips = new Dictionary<string, AnimationClip>();
    public GameObject monsterbattle;
    public GameObject bravebattle;
    public GameObject monsterdamage;
    public GameObject bravedamage;
    public GameObject shield;
    private Animator monsterAnime;
    private Animator braveAnime;
    private Animator shieldAnime;
    private Image monsterVFX;
    private Image braveVFX;
    private Image shieldVFX;
    public TextGrowEffect MonsterDamage;
    public TextGrowEffect BraveDamage;


    private void Awake()
    {
        Debug.Log("有人嗎");
        player = FindFirstObjectByType<Braveattr>();
        Player = player.gameObject;
        Debug.Log(player);
    }
    private void Start()
    {   
        audioManager = FindFirstObjectByType<AudioManager>();
        
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


        braveVFX = bravebattle.GetComponent<Image>();
        monsterVFX = monsterbattle.GetComponent<Image>();
        monsterAnime = monsterbattle.GetComponent <Animator>();
        braveAnime = bravebattle.GetComponent<Animator>();
        shieldAnime = shield.GetComponent<Animator>();
        shieldVFX = shield.GetComponent<Image>();
        Playermovement = Player.GetComponent<Braveplayer>();
        Monstercritical = false; // 初始化
        critic = false; // 初始化
        CriticalOK.gameObject.SetActive(false);
        SwordOK.gameObject.SetActive(false);
        ShieldOK.gameObject.SetActive(false);
        Escape.gameObject.SetActive(false);

        StartCoroutine(SetNativeSizeDelayed()); // 確保第一幀能正確設置動畫圖片大小
        monsterVFX.transform.localScale *= 2;
        braveVFX.transform.localScale *= 2;
        shieldVFX.transform.localScale *= 2;
    }

    // Update is called once per frame
    void Update()
    {
        // 按下 Q 鍵時，觸發逃跑
        if (Input.GetKeyDown(KeyCode.Q) && Escape.gameObject.activeSelf)
        {
            braveVFX.enabled = false;
            braveAnime.enabled = false;
            monsterVFX.enabled = false;
            monsterAnime.enabled = false;
            shieldVFX.enabled = false;
            shieldAnime.enabled = false;
            state = BattleState.Escape;
            EscapeBattle();
        }

        // 按下 Enter 鍵時，結束介面
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
            if (critic == false && Braveattr.RecentSword == false)
            {
                critic = true;
                player.DecreaseAttribute("Breath", 40);
                CriticalOK.gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) && Braveattr.GetAttribute("Breath") >= Braveattr.EquippedSword.BreathCost)
        {
            if (critic == false && Braveattr.RecentSword == false)
            {
                SwordOK.gameObject.SetActive(true);
                Braveattr.RecentSword = true;
                // TODO 取消狀態
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && Braveattr.GetAttribute("Breath") >= Braveattr.EquippedShield.BreathCost)
        {
            if (Braveattr.RecentShield == false)
            {
                ShieldOK.gameObject.SetActive(true);
                Braveattr.RecentShield = true;
                // TODO 取消狀態
            }
        }

        if (!BattleUI.activeSelf)
        {
            monsterVFX.enabled = false;
            monsterAnime.enabled = false;
        }
        
        if (!BattleUI.activeSelf)
        {
            braveVFX.enabled = false;
            braveAnime.enabled = false;
        }

        if (!BattleUI.activeSelf)
        {
            shieldVFX.enabled = false;
            shieldAnime.enabled = false;
        }

        if (monsterVFX.enabled == true)
        {
            monsterVFX.SetNativeSize(); // 設定原始大小
        }

        if (braveVFX.enabled == true)
        {
            braveVFX.SetNativeSize();
        }

        if (shieldVFX.enabled == true)
        {
            shieldVFX.SetNativeSize();
        }

    }

    private IEnumerator SetNativeSizeDelayed()
    {
        // 等待一幀確保 Sprite 已經設定完成
        yield return new WaitForEndOfFrame();

        braveVFX.SetNativeSize();
        monsterVFX.SetNativeSize();
        shieldVFX.SetNativeSize();
    }

    private IEnumerator VFXplay(Animator who, Image whose, string name, bool critical)
    {   
        who.enabled = true;
        whose.enabled = true;
        string playName = critical ? name + "critical" : name;
        // 撥動畫並強制從頭開始
        StartCoroutine(SetNativeSizeDelayed());
        who.Play(playName, 0, 0f);
        yield return null;
        float attackLength = who.GetCurrentAnimatorStateInfo(0).length;
        float shieldLength = 0f; // 初始化盾牌動畫長度

        if (Braveattr.RecentShield == true && Braveattr.HoldShield != "None")
        {
            shieldVFX.enabled = true;
            shieldAnime.Play(Braveattr.HoldShield, 0, 0f);
            shieldLength = shieldAnime.GetCurrentAnimatorStateInfo(0).length;
        }

        // 撥音效
        audioManager.PlaySFX(playName);

        yield return null; // 先等待1幀

        float waittime = Mathf.Max(shieldLength , attackLength); 
        
        yield return new WaitForSeconds(waittime);

        who.enabled = false;
        whose.enabled = false;
        shieldVFX.enabled = false;
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

        Player.GetComponent<Braveplayer>().StopCurrentRepeatMovement();
        Player.GetComponent<Braveplayer>().StopMoving();
        Player.GetComponent<Braveplayer>().enabled = false;

        StartCoroutine(UI.InitHUDBrave(player));
        StartCoroutine(UI.InitHUDMonster(enemy));

        SwordEquip(Braveattr.HoldSword, enemy, player); // 裝備劍
        ShieldEquip(Braveattr.HoldShield, enemy, player); // 裝備盾


        if (enemy.System.Contains("先攻"))
        {
            state = BattleState.MonsterTurn;
            Monsterturn += 1;
            StartCoroutine(MonsterTurn());
        }
        else
        {
            state = BattleState.PlayerTurn;
            Braveturn += 1;
            StartCoroutine(PlayerAttack());
        }
        Debug.Log("玩家先攻!");
        
        // BattleChoose();
        
    }

    private void UpdateUI() // 戰鬥更新UI
    {
        UI.MonsterUpProperties(enemy);
        UI.BraveUpProperties(player);
        UI.BraveUpdateBreath(player);
        UI.MonsterUpdateBreath(enemy);
    }


    private void PlayerAttackcalculate(bool critical)
    {
        int atk = Braveattr.GetAttribute("Atk");
        int damage = BattleCalculate.CalculateDamage(enemy, player, critical, "Brave");
        int breathGain = Mathf.Max((enemy.Def * 10) / atk, 0); // 氣息

        enemy.CurrentHp -= damage;
        enemy.CurrentBreath = Mathf.Min(enemy.Breath, enemy.CurrentBreath + breathGain);
        Braveattr.Tempbreath = Braveattr.GetAttribute("Breath"); // 暫存氣息
        player.IncreaseAttribute("Breath", breathGain);
        MonsterDamage.ShowDamageText( damage.ToString() );
        UpdateUI();
    }
    private void MonsterAttackcalculate( bool critical )
    {
        int def = Braveattr.GetAttribute("Def");
        int damage = BattleCalculate.CalculateDamage(enemy, player, critical, "Monster");
        int breathGain = Mathf.Max((enemy.Atk - def) / 10, 0);

        // 扣血與補氣息
        player.DecreaseAttribute("Hp", damage);
        enemy.CurrentBreath = Mathf.Min(enemy.Breath, enemy.CurrentBreath + (def * 10 / enemy.Atk));
        player.IncreaseAttribute("Breath", breathGain);

        // 顯示傷害與更新介面
        BraveDamage.ShowDamageText(damage.ToString());
        UpdateUI();
    }

    public IEnumerator MonsterAttackRoutine( bool Monstercritical, Animator monsterAnime, Image monsterVFX, MONSTER enemy)
    {
        int MonsterAttackCount = enemy.Atktimes;
        for (int i = 0; i < MonsterAttackCount; i++)
        {
            MonsterAttackcalculate(Monstercritical);
            yield return StartCoroutine(VFXplay(monsterAnime, monsterVFX, enemy.Anime, Monstercritical));
            if (Monstercritical)
            {
                Monstercritical = false;
            }
        }
    }

    private IEnumerator PlayerAttack()
    {   
        myText.text = "勇者對" + enemy.name + "使用了攻擊!";
        myText.ForceMeshUpdate(); // 強制更新 UI
        PlayerAttackcalculate(critic);
        if (Braveattr.RecentSword == false)
        {
            yield return StartCoroutine(VFXplay(braveAnime, braveVFX, "None", critic));
        }
        else
        {
            yield return StartCoroutine(VFXplay(braveAnime, braveVFX, Braveattr.HoldSword, critic));
        }
        if (critic) // 爆擊重置
        {
            critic = !critic;
            CriticalOK.gameObject.SetActive(false);
        }
        BraveDamage.gameObject.SetActive(false);
        if (Braveattr.RecentSword) // 使用劍後重置
        {
            Braveattr.RecentSword = false;
            SwordOK.gameObject.SetActive(false);
        }

        if (enemy.CurrentHp <= 0)
        {
            state = BattleState.Win;
            StartCoroutine(BattleEnd(player));
        }
        else
        {
            state = BattleState.MonsterTurn;
            StartCoroutine(NextTurn());
        }
    }

    private IEnumerator MonsterTurn()
    {
        myText.text = enemy.name + "對" + player.name + "使用了攻擊!";
        if (enemy.CurrentBreath == enemy.Breath)  // 怪物自動爆擊
        {
            enemy.CurrentBreath -= enemy.Breath;
            Monstercritical = true;
        }
        yield return StartCoroutine(MonsterAttackRoutine(Monstercritical,monsterAnime,monsterVFX,enemy));

        MonsterDamage.gameObject.SetActive(false);
        if (Braveattr.RecentShield) // 使用劍後重置
        {
            Braveattr.RecentShield = false;
            ShieldOK.gameObject.SetActive(false);
        }
        if (Braveattr.attributes["Hp"] <= 0)
        {
            state = BattleState.Lose;
            StartCoroutine(BattleEnd(player));
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
        BraveDamage.gameObject.SetActive(false);
        MonsterDamage.gameObject.SetActive(false);
        StartCoroutine(BattleEnd(player));
    }

    public void Refresh(MONSTER monster)
    {
        monster.CurrentHp = monster.Hp;
        monster.CurrentBreath = 0;
        Monstercritical = false;
    }

    private void SwordEquip(string name, MONSTER monster, Braveattr player)
    {
        switch (name)
        {
            case "凡骨":
                Braveattr.EquippedSword = new Mundane(player, monster);
                break;
            case "流石":
                Braveattr.EquippedSword = new Streamstone(player, monster);
                break;
            case "深紅":
                Braveattr.EquippedSword = new Crimson(player, monster);
                break;
            case "天靈":
                Braveattr.EquippedSword = new Spirit(player, monster);
                break;
            case "皇者":
                Braveattr.EquippedSword = new Sovereign(player, monster);
                break;
            default:
                Braveattr.EquippedSword = null;
                break;
        }

        Debug.Log($"✅ 裝備了劍：{name}，消耗氣息：{Braveattr.EquippedSword?.BreathCost}");
    }

    private void ShieldEquip(string name, MONSTER monster, Braveattr player)
    {
        switch (name)
        {
            case "鏡膜":
                Braveattr.EquippedShield = new Mirror(player, monster);
                break;
            case "結晶":
                Braveattr.EquippedShield = new Crystallite(player, monster);
                break;
            case "反射":
                Braveattr.EquippedShield = new Reflection(player, monster);
                break;
            case "精靈":
                Braveattr.EquippedShield = new Fairy(player, monster);
                break;
            case "賢者":
                Braveattr.EquippedShield = new Sage(player, monster);
                break;
            default:
                Braveattr.EquippedShield = null;
                break;
        }

        Debug.Log($"✅ 裝備了盾：{name}，消耗氣息：{Braveattr.EquippedShield?.BreathCost}");
    }

    private void CheckEnter()
    {
        if (Playermovement != null)
        {
            Playermovement.enabled = true;
            Debug.Log("勇者的移動被恢復了");
        }

        if (animeend)
        {
            Player.GetComponent<Animator>().enabled = true;
            VictoryUI.SetActive(false);
            BattleUI.SetActive(false);
        }

        animeend = false;

    }


    private IEnumerator BattleEnd(Braveattr player)
    {
        Escape.gameObject.SetActive(false);
        critic = false; // 重置爆擊狀態
        Monstercritical = false; 
        CriticalOK.gameObject.SetActive(false);
        BraveDamage.gameObject.SetActive(false);
        MonsterDamage.gameObject.SetActive(false);

        player.DecreaseAttribute("Atk", Braveattr.Tempatk);
        player.DecreaseAttribute("Def", Braveattr.Tempdef);

        Braveattr.Tempatk = 0;
        Braveattr.Tempdef = 0;

        if (state == BattleState.Win)
        {
            myText.text = "現在是" + player.name + "的勝利!";
            // 戰鬥完消滅怪物
            Debug.Log("勝利!");
            Pamphlet.Refresh(Teki); // 重製怪物手冊資料
            Destroy(Teki);
            myText.SetText("");
            VictoryUI.SetActive(true);
            yield return new WaitForSeconds(0.4f); // 增加一點時間感
            animeend = true;

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
            if (Playermovement != null)
            {
                Playermovement.enabled = true;
                Debug.Log("勇者的移動被恢復了");
            }
            // TODO 逃跑時氣息變成跟原本一樣
            Player.GetComponent<Animator>().enabled = true;
            Braveattr.SetAttribute("Breath", Braveattr.Tempbreath);
        }
    }
}

