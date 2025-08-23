using UnityEngine;
using TMPro;
using cfg;
using static UnityEngine.Rendering.DebugUI;
using Unity.VisualScripting;
using System.Linq;
using LDtkUnity;

public class ItemComponent : MonoBehaviour
{
    // 道具屬性字段
    public string Name { get; private set; }
    public string Icon { get; private set; }
    public cfg.item.EType Type { get; private set; }
    public cfg.item.EUseType UseType { get; private set; }
    public cfg.PropertyData[] Properties { get; private set; }
    public cfg.PercentPropertyData[] PercentProperties { get; private set; }
    public cfg.ItemData[] Items { get; private set; }
    public string System {  get; private set; }
    public string PickTips { get; private set; }
    public string UseTips { get; private set; }
    public string Desc { get; private set; }

    public string ID { get; private set; }

    [Header("物件UI")]
    private GameObject Itemui;
    private TextMeshProUGUI Itemname;

    private Inventory inventory; // 用於存儲 Inventory 參考

    private Braveattr braveattr; // 用於存儲玩家屬性參考

    private AudioManager audiomanager;
    
    private GameObject Player;



    /// <summary>
    /// 設置道具屬性
    /// </summary>
    public void SetProperties(cfg.item.Item item , Tables table)
    {
        Debug.Log($"正在設置屬性: {item.Id}");
        Name = item.Name;
        Icon = item.Icon;
        Type = item.Type;
        UseType = item.UseType;
        Properties = item.Properties;
        PercentProperties = item.PercentProperties;
        Items = item.Items;
        System = item.System;
        PickTips = item.PickTips;
        UseTips = item.UseTips;
        Desc = item.Desc;

        Debug.Log($"已設置物件屬性: Name = {Name}, Icon = {Icon}, Type = {Type}, UseType = {UseType}, Properties = {Properties}");
    }

    private void Awake()
    {
        GameObject parent = GameObject.Find("TipsUI");  // 確定這個是啟用的
        Itemui = parent.transform.Find("Tips").gameObject;
        Itemname = Itemui.GetComponentInChildren<TextMeshProUGUI>(true);
        inventory = FindFirstObjectByType<Inventory>();
        braveattr = FindFirstObjectByType<Braveattr>();
        audiomanager = FindFirstObjectByType<AudioManager>();
        Player = FindFirstObjectByType<Braveplayer>().gameObject;
        var entity = GetComponent<LDtkComponentEntity>();
        if (entity != null)
        {
            ID = entity.Iid.ToString();
        }
    }

    private void Start()
    {
        Itemui.SetActive(false);
        GameObject self = this.gameObject; // 不能加gameobject，因為會在宣稱一個變數但在start內不會創立。
    }

    private void Update()
    {
        if (Itemui.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || (Input.GetKeyDown(KeyCode.Return)))
            {
                Itemui.SetActive(false);
                Player.GetComponent<Braveplayer>().enabled = true;
                Player.GetComponent<Animator>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            if (Type == cfg.item.EType.物品 | Type == cfg.item.EType.寶物)
            {   
                if (UseType != cfg.item.EUseType.自動使用)
                {
                    char lastChar = ID[ID.Length - 1];
                    Debug.Log($"拾取道具: {Name} , {lastChar}");
                    inventory.AddItem(Name,1);
                }
                if (UseType == cfg.item.EUseType.自動使用)
                {
                    if (Properties != null)
                    {
                        foreach (var prop in Properties)
                        {
                            braveattr.IncreaseAttribute(prop.Attribute, prop.Count);
                            Debug.Log(Braveattr.GetAttribute(prop.Attribute));
                        }
                    }
                    

                    if (PercentProperties != null)
                    {

                    }
                    
                    if (Items != null)
                    {
                        foreach (var item in Items)
                        {
                            inventory.AddItem(item.Id, 1);
                        }
                    }

                    if (System != null)
                    {
                        string[] skillList = System.Split(',');
                        
                        if (skillList.Contains("劍技") )
                        {
                            Braveattr.Sword.Add(skillList[0]);
                            Braveattr.HoldSword = skillList[0];
                        }
                        if (skillList.Contains("盾術"))
                        {
                            Braveattr.Shield.Add(skillList[0]);
                            Braveattr.HoldShield = skillList[0];
                        }
                    }

                }
                
            }
            ItemUI(gameObject.GetComponent<ItemComponent>(), Itemui, Itemname);
            Destroy(gameObject); // 拾取后销毁道具
            audiomanager.PlaySFX("Item");
            Debug.Log(inventory.GetItemQuantity(Name));
            Debug.Log(Name);
        }
    }

    private void ItemUI ( ItemComponent mono , GameObject Itemui , TextMeshProUGUI Itemname )
    {
        Itemui.SetActive(true);
        Player.GetComponent<Braveplayer>().StopCurrentRepeatMovement();
        Player.GetComponent<Braveplayer>().StopMoving();
        Player.GetComponent<Braveplayer>().enabled = false;
        if (mono.UseType == cfg.item.EUseType.不可使用)
        {
            Itemname.text = mono.PickTips;
        }
        else if (mono.UseType == cfg.item.EUseType.自動使用)
        {
            Itemname.text = mono.UseTips;
        }
    }




}
