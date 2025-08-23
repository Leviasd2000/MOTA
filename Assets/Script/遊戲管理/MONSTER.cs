using System.Collections.Generic;
using cfg;
using UnityEngine;
using System;
using static UnityEngine.Rendering.DebugUI;
using Mono.Cecil;

public class MONSTER : MonoBehaviour

{
    // 怪物屬性字段
    public string Name { get; private set; }
    public string Anime { get; private set; }
    public List<int> Properties { get; private set; }
    public cfg.ItemData[] Items { get; private set; }
    public string System { get; private set; }
    public string Skilltips { get; private set; }
    public string Desc { get; private set; }

    public int Hp { get; private set; }
    public int Atk { get; private set; }
    public int Def { get; private set; }
    public int Gold { get; private set; }
    public int Exp { get; private set; }
    public int Fatigue { get; private set; }
    public int Breath { get; private set; }

    public int Atktimes { get; private set; }

    public int CurrentHp { get; set; }  // 當前血量
    public int CurrentBreath { get; set; } // 當前氣息
    public int CurrentFatigue { get; set; } // 當前氣息

    public int Freeze { get; set; } // 凍結回合數

    public float actionSpeed;

    private Inventory inventory; // 用於存儲 Inventory 參考

    private Braveattr braveattr; // 用於存儲玩家屬性參考

    public static event Action<GameObject> OnEnemyDefeated; // 事件讓外部訂閱

    private Animator animator;

    public string Animation ;




    /// <summary>
    /// 戰鬥介面
    /// </summary>
    public GameObject Interface;


    /// <summary>
    /// 設置道具屬性
    /// </summary>
    public void SetProperties(cfg.monster.Monster monster, Tables table)
    {
        Debug.Log($"正在設置屬性: {monster.Id}");
        Name = monster.Name;
        Anime = monster.Anime;
        Hp = monster.Properties[0];
        Atk = monster.Properties[1];
        Def = monster.Properties[2];
        Gold = monster.Properties[3];
        Exp = monster.Properties[4];
        Fatigue = monster.Properties[5];
        Breath = monster.Properties[6];
        Atktimes = monster.Properties[7];
        Items = monster.Items;
        System = monster.System;
        Skilltips = monster.SkillTips;
        Desc = monster.Desc;
        CurrentHp = monster.Properties[0]; //當前血量
        CurrentBreath = 0;
        actionSpeed = 2;
        Animation = Name;


        Debug.Log($"已設置物件屬性: Name = {Name}, Anime = {Anime}, Hp = {Hp}, Atk = {Atk}, Def = {Def}, Gold = {Gold}, Exp = {Exp}");
    }

    private void Awake()
    {
        inventory = FindFirstObjectByType<Inventory>();
        braveattr = FindFirstObjectByType<Braveattr>();
        Interface = GameObject.Find("BattleUI");
    }

    private void Start()
    {
        animator = GetComponent<Animator>(); // 確保Animator已經指派
    }

    public void PlayAnimation()
    {
        animator.SetTrigger(Animation); // 播放攻擊動畫
    }

    /*補充：
    1.虛擬方法必須實作。
    2.虛擬方法中必須為公開(public)。因為允許子類別複寫。
    3.子類別可以直接引用或選擇複寫(override)虛擬方法
    */

    private void OnCollisionEnter2D( Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{Name} 被玩家攻擊");

            // 觸發事件通知 GameManager
            OnEnemyDefeated?.Invoke(gameObject);

            if (Interface != null)
            {
                Interface.SetActive(true);
            }
        }
    }
   
}






