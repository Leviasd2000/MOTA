using cfg;
using LDtkUnity;
using SimpleJSON;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MainMonster : MonoBehaviour
{
    
    public static List<string> monster_names = new List<string>();
    /// <summary>
    /// �C���}�l�ɰ���
    /// </summary>
    void Awake()
    {
        List<GameObject> data = new List<GameObject>(); // �ΨӦs�x�Ҧ��ǰt������

        var tables = new cfg.Tables(LoadByteBuf);


        data = FindAllMonster();

        foreach (var item in data)
        {
            AddComponentsAndAssignPropertiesMonster(item, tables);
        }

        foreach (var member in tables.Tbmonster.DataList)
        {
           monster_names.Add(member.Id);
        }

        Debug.Log(monster_names[0]+"�b����");

    }

    /// <summary>
    /// �[�� JSON �ƾ�
    /// </summary>
    private static JSONNode LoadByteBuf(string file)
    {
        return JSON.Parse(File.ReadAllText(Application.dataPath + "/Data/" + file + ".json", System.Text.Encoding.UTF8));
    }

    /// <summary>
    /// �M����������A�K�[�ե�ó]�m�ݩ�
    /// </summary>
    /// 

    public List<GameObject> FindAllMonster()
    {
        List<GameObject> matchedItems = new List<GameObject>(); // �ΨӦs�x�Ҧ��ǰt������

        GameObject world = GameObject.Find("World"); // ����� World ����
        if (world == null)
        {
            Debug.LogError("World object not found!");
            return null;
        }

        foreach (Transform level in world.transform) // �M���Ĥ@�h Level1_x
        {
            Transform entities = level.Find("Entities"); // �d��Ӽh�� "Entities"
            if (entities == null)
            {
                Debug.LogError($"Entities not found in level: {level.name}");
                continue;
            }

            foreach (Transform child in entities) // �M�� Entities �U���Ҧ��l����
            {

                LDtkFields goals = child.GetComponent<LDtkFields>();
                if (goals == null)
                {
                    Debug.LogError($"LDtkFields component missing on {child.name}");
                    continue;
                }

                string itemValue = goals.GetString("Monster");

                if (itemValue != null) // �ˬd item �O�_�ǰt
                {
                    Debug.Log($"Monster '{itemValue}' found: {child.name}");
                    matchedItems.Add(child.gameObject);
                }
            }
        }
        if (matchedItems.Count > 0)
        {
            return matchedItems;
        }
        else
        {
            Debug.LogError($"Item is not found!");
            return null; // �p�G�S�����A��^ null
        }
    }

    private void AddComponentsAndAssignPropertiesMonster(GameObject monster, Tables table)
    {
        /// �ھ� ID �d��������C������]���]�W�ٻP item.Id �ۦP�^
        //  GameObject itemObject = FindItem(item.Id);

        LDtkFields fields = monster.GetComponent<LDtkFields>();

        string name = fields.GetString("Monster");

        if (name == null)
        {
            Debug.LogWarning($"�䤣��W�٬� '{monster}' ���C������A�L�k���[�ݩʡI");
            return;
        }

        // �T�O�Ӫ���W�� ItemComponent
        MONSTER MonsterComponent = monster.GetComponent<MONSTER>();
        if (MonsterComponent == null)
        {
            MonsterComponent = monster.AddComponent<MONSTER>();
        }

        foreach (var member in table.Tbmonster.DataList)
        {
            if (name == member.Id)
            {
                MonsterComponent.SetProperties(member, table);
                Debug.Log($"�w������ '{member.Id}' ���[�ݩ�: {member}");  // �]�w�ݩ�
            }
        }
    }
}
