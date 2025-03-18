using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using LDtkUnity;
using System;
using System.Drawing.Printing;
using static UnityEditor.FilePathAttribute;
using System.Drawing.Text;
using cfg.monster;
using UnityEngine.AddressableAssets;
using UnityEngine.TextCore.Text;

public class Pamphlet : MonoBehaviour
{
    public GameObject platform;
    public GameObject Player;
    public Braveplayer player;
    private static Dictionary<int, List<GameObject>> monsterinstances = new Dictionary<int, List<GameObject>>();
    private List<GameObject> temp = new List<GameObject>();
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
    public static int initialocation = 1; // 圖鑑頁面位置
    public AudioManager audioManager;

    [Header("箭頭")]
    public GameObject Leftarrow;
    public GameObject Rightarrow;

    [Header("角色屬性")]
    public List<TextMeshProUGUI> attributes1;
    public List<TextMeshProUGUI> attributes2;
    public List<TextMeshProUGUI> attributes3;

    [Header("屬性載體")]
    public GameObject attr1;
    public GameObject attr2;    
    public GameObject attr3;

    [Header("圖片")]
    public Image Image1;
    public Image Image2;
    public Image Image3;
        

    void Start()
    {
        Player = FindFirstObjectByType<Braveplayer>().gameObject;
        player = FindFirstObjectByType<Braveplayer>();
        audioManager = FindFirstObjectByType<AudioManager>();
        Addressables.LoadAssetsAsync<Sprite>("Monster", sprite =>
        {
            spriteDict[sprite.name] = sprite; // 存入 List
            Debug.Log($"載入圖片：{sprite.name}");
        }, true);
        platform = GameObject.Find("Monstercatalog");
        platform.SetActive(false);
        GameObject world = GameObject.Find("World"); // 先找到 World 物體
        if (world == null)
        {
            Debug.LogError("World object not found!");
        }

        foreach (Transform level in world.transform) // 遍歷第一層 Level1_x
        {
            char floor = level.name[^1];
            int number = (int)char.GetNumericValue(floor);
            Transform entities = level.Find("Entities"); // 查找該層的 "Entities"
            if (entities == null)
            {
                Debug.LogError($"Entities not found in level: {level.name}");
                continue;
            }

            foreach (Transform child in entities) // 遍歷 Entities 下的所有子物體
            {

                LDtkFields goals = child.GetComponent<LDtkFields>();
                if (goals == null)
                {
                    Debug.LogError($"LDtkFields component missing on {child.name}");
                    continue;
                }

                string itemValue = goals.GetString("Monster");

                if (itemValue != null) // 檢查 item 是否匹配
                {
                    Debug.Log($"Monster '{itemValue}' found: {child.name} 找到了!!!!!!");
                    temp.Add(child.gameObject);
                }
                
            }
            monsterinstances[number] = new List<GameObject>(temp);
            temp.Clear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && platform.activeSelf != true)
        {
            platform.SetActive(true);
            Show(Braveplayer.floor, initialocation , monsterinstances);
            Player.GetComponent<Animator>().enabled = false;
            player.enabled = false;
        }
        if (platform.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && Rightarrow.activeSelf)
            {
                initialocation += 1;
                audioManager.Play("Click",false);
                Show(Braveplayer.floor, initialocation , monsterinstances);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && Leftarrow.activeSelf)
            {
                initialocation -= 1;
                audioManager.Play("Click", false);
                Show(Braveplayer.floor, initialocation , monsterinstances);
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                platform.SetActive(false);
                Player.GetComponent<Animator>().enabled = true;
                player.enabled = true;
                audioManager.Play("Leave", false);
                initialocation = 1;
            }

        }
    }

    public static void Refresh( GameObject ENEMY)
    {
        monsterinstances[Braveplayer.floor].Remove(ENEMY);
        Debug.Log(monsterinstances[Braveplayer.floor].Count);
    }

    private void Show(int recentfloor , int page , Dictionary<int, List<GameObject>> database )
    {
        List<GameObject> currentone = database[recentfloor];
        Dictionary<string, GameObject> monsterpair = new Dictionary<string, GameObject>();
        List<string> monsterName = new List<string>();
        foreach (GameObject ob in currentone)     // 不重複怪物圖鑑
        {
            MONSTER Mname = ob.GetComponent<MONSTER>();
            if (!monsterpair.ContainsKey(Mname.Name))
            {
               monsterpair[Mname.Name] = ob;
            }
           
        }
        Debug.Log(monsterpair.Count);

        foreach ( string key in monsterpair.Keys)
        {
            monsterName.Add(key);
        }

        int length = monsterpair.Count;


        if ( 3* page +1 > length )
        {
            Rightarrow.SetActive(false);
            Rightarrow.GetComponent<Image>().color = new Color32(255, 255, 255, 168); // 255 表示完全不透明
        }
        else
        {
            Rightarrow.SetActive(true);
            Rightarrow.GetComponent<Image>().color = new Color32(255, 255, 255, 255); // 255 表示完全不透明
        }

        if (page == 1)
        {
            Leftarrow.SetActive(false);
            Leftarrow.GetComponent<Image>().color = new Color32(255, 255, 255, 168); // 255 表示完全不透明
        }
        else
        {
            Leftarrow.SetActive(true);
            Leftarrow.GetComponent<Image>().color = new Color32(255, 255, 255, 255); // 255 表示完全不透明
        }

        if ( 3 * page - 3 < length)
        {
            attr1.SetActive(true);
            Setproperties(attributes1, monsterpair[monsterName[3 * page - 3]].GetComponent<MONSTER>());
            Image1.sprite = spriteDict[monsterpair[monsterName[3 * page - 3]].GetComponent<MONSTER>().Name];
        }
        else
        {
            attr1.SetActive(false);
        }
        if (3 * page - 2 < length)
        {   
            attr2.SetActive(true);
            Setproperties(attributes2, monsterpair[monsterName[3 * page - 2]].GetComponent<MONSTER>());
            Image2.sprite = spriteDict[monsterpair[monsterName[3 * page - 2]].GetComponent<MONSTER>().Name];
        }
        else
        {
            attr2.SetActive(false);
        }
        if (3 * page - 1 < length)
        {   
            attr3.SetActive(true);
            Setproperties(attributes3, monsterpair[monsterName[3 * page - 1]].GetComponent<MONSTER>());
            Image3.sprite = spriteDict[monsterpair[monsterName[3 * page - 1]].GetComponent<MONSTER>().Name];
        }
        else
        {
            attr3.SetActive(false);
        }
    }

    private void Setproperties(  List<TextMeshProUGUI> attrword  , MONSTER attr)
    {
        attrword[0].text = $"<color=#0AC5F1>{attr.Name}</color>";
        attrword[1].text = $"<color=#17E7E3>{attr.System}</color>";
        attrword[2].text = "生命:\n" + $"<color=#26D109>{attr.Hp}</color>";
        attrword[3].text = "攻擊力:\n" + $"<color=#FD6364>{attr.Atk}</color>"; 
        attrword[4].text = "防禦力:\n" + $"<color=#EC7A13>{attr.Def}</color>";
        attrword[5].text = "氣息容量:\n" + $"<color=#068C1A>{attr.Breath}</color>";
        attrword[6].text = "攻擊次數:\n" + $"<color=#EE54D2>{attr.Atktimes}</color>";
        attrword[7].text = DamageEstimate(attr);
        attrword[8].text = "EXP:\n" + $"<color=#05CD82>{attr.Exp}</color>";
        attrword[9].text = "GOLD:\n" + $"<color=#EEF311>{attr.Gold}</color>";
    }

    private string DamageEstimate(MONSTER enemy)
    {
        int atk = Braveattr.GetAttribute("Atk");
        int def = Braveattr.GetAttribute("Def");
        if ( (atk + Braveattr.AttackCritical) <  enemy.Def && enemy.Atk > (def+Braveattr.DefenseCritical)) // 低於攻擊臨界
        {
                return "估計傷害:\n" + $"<color=#FA0408>{"DIE"}</color>";
        }

        if (Braveattr.GetAttribute("Def") > (enemy.Atk + Braveattr.DefenseCritical)) // 高於防禦臨界
        {
                return "估計傷害:\n" + $"<color=#FA0408>{0}</color>" ;
        }
        if ((atk + Braveattr.AttackCritical) >= enemy.Def) // 正常情況
        {
            double rounds = enemy.Hp / Math.Max(1, atk - enemy.Def);
            int times = enemy.Atktimes;
            if (enemy.System.Contains("魔法攻擊"))
            {
                    return "估計傷害:\n" + $"<color=#FA0408>{times * enemy.Atk * Math.Floor(rounds)}</color>";
            }
            else
            {
                    return "估計傷害:\n" + $"<color=#FA0408>{times * Math.Max(1, enemy.Atk - def) * Math.Floor(rounds)}</color>";
            }
                
        }
        return "估計傷害:\n" + $"<color=#FA0408>{"？"}</color>";
    }

}


