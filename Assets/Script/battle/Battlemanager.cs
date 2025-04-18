using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;
using System.Linq;

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
    private Animator monsterAnime;
    private Animator braveAnime;
    private Image monsterVFX;
    private Image braveVFX;
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

        monsterVFX = monsterbattle.GetComponent<Image>();
        braveVFX = bravebattle.GetComponent<Image>();
        monsterAnime = monsterbattle.GetComponent <Animator>();
        braveAnime = bravebattle.GetComponent<Animator>();
        Playermovement = Player.GetComponent<Braveplayer>();
        Monstercritical = false; // 初始化
        critic = false;
        CriticalOK.gameObject.SetActive(false);
        

        StartCoroutine(SetNativeSizeDelayed()); // 確保第一幀能正確設置動畫圖片大小
        monsterVFX.transform.localScale *= 2;
        braveVFX.transform.localScale *= 2;
    }

    // Update is called once per frame
    void Update()
    {
        // 按下 Q 鍵時，觸發逃跑
        if (Input.GetKeyDown(KeyCode.Q) && Escape.gameObject.activeSelf)
        {
            braveVFX.enabled = false;
            braveAnime.enabled = false;
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
            if (critic == false)
            {
                critic = true;
                player.DecreaseAttribute("Breath", 40);
                Debug.Log("爆擊");
                CriticalOK.gameObject.SetActive(true);
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

        if (monsterVFX.enabled == true)
        {
            monsterVFX.SetNativeSize(); // 設定原始大小
        }

        if (braveVFX.enabled == true)
        {
            braveVFX.SetNativeSize();
        }

    }

    private IEnumerator SetNativeSizeDelayed()
    {
        yield return new WaitForEndOfFrame();
        monsterVFX.SetNativeSize(); // 設定原始大小
        braveVFX.SetNativeSize();
    }

    private IEnumerator VFXplay(Animator who, Image whose, string name, bool critical)
    {   
        who.enabled = true;
        whose.enabled = true;

        if (critical){
            who.Play(name+"critical");
            audioManager.Play(name+"critical", false);
        }
        else{
            who.Play(name);
            audioManager.Play(name,false);
        }

        float waittime = (who.GetCurrentAnimatorStateInfo(0).length - 0.05f);
        yield return new WaitForSeconds(waittime);

        who.enabled = false;
        whose.enabled = false;
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

        if (enemy.System.Contains("先攻"))
        {
            state = BattleState.MonsterTurn;
            StartCoroutine(MonsterTurn());
        }
        else
        {
            state = BattleState.PlayerTurn;
            StartCoroutine(PlayerAttack());
        }
        Debug.Log("玩家先攻!");
        
        // BattleChoose();
        
    }

    private void PlayerAttackcalculate(bool critical)
    {
        int dm = enemy.MonsterDefense_Damage(Braveattr.attributes["Atk"], enemy.System, critical); // 傷害
        int cm = (enemy.Def * 10) / Braveattr.attributes["Atk"]; // 氣息
        enemy.CurrentHp -= dm;
        enemy.CurrentBreath = Mathf.Min(enemy.Breath , enemy.CurrentBreath + cm);
        MonsterDamage.ShowDamageText( dm.ToString() );
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
        enemy.CurrentBreath = Mathf.Min(enemy.Breath , enemy.CurrentBreath + Braveattr.attributes["Def"] * 10 / enemy.Atk );
        BraveDamage.ShowDamageText(dm.ToString());
        player.DecreaseAttribute("Hp", dm);
        player.IncreaseAttribute("Breath", cm);
        UI.MonsterUpProperties(enemy);
        UI.BraveUpProperties(player);
        UI.BraveUpdateBreath(player);
        UI.MonsterUpdateBreath(enemy);
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
        yield return StartCoroutine(VFXplay(braveAnime, braveVFX, player.Sword , critic));
        if (critic) // 爆擊重置
        {
            critic = !critic;
            CriticalOK.gameObject.SetActive(false);
        }
        BraveDamage.gameObject.SetActive(false);

        if (enemy.CurrentHp <= 0)
        {
            state = BattleState.Win;
            StartCoroutine(BattleEnd());
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
        yield return new WaitForSeconds(0.4f);
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
        StartCoroutine(BattleEnd());
    }

    public void Refresh(MONSTER monster)
    {
        monster.CurrentHp = monster.Hp;
        monster.CurrentBreath = 0;
        Monstercritical = false;
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


    private IEnumerator BattleEnd()
    {
       
        Escape.gameObject.SetActive(false);
        critic = false; // 重置爆擊狀態
        Monstercritical = false; 
        CriticalOK.gameObject.SetActive(false);
        BraveDamage.gameObject.SetActive(false);
        MonsterDamage.gameObject.SetActive(false);

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

            Player.GetComponent<Animator>().enabled = true;
        }

        
    }
}

