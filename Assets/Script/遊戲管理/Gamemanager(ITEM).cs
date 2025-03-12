using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleJSON;
using cfg;
using LDtkUnity;
using static UnityEngine.Rendering.DebugUI;
using cfg.monster;


public class MainItem : MonoBehaviour
{

    /// <summary>
    /// �C���}�l�ɰ���
    /// </summary>
    void Start()
    {
        List<GameObject> data = new List<GameObject>(); // �ΨӦs�x�Ҧ��ǰt������
        
        var tables = new cfg.Tables(LoadByteBuf);
        

        data = FindAllItem();

        foreach (var item in data)
        {   
            AddComponentsAndAssignPropertiesItem(item,tables);
        }

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

    public List<GameObject> FindAllItem()
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

                string itemValue = goals.GetString("item");


                if (itemValue != null ) // �ˬd item �O�_�ǰt
                {
                    Debug.Log($"Item '{itemValue}' found: {child.name}");
                    matchedItems.Add(child.gameObject);
                }
            }
        }
        if ( matchedItems.Count > 0 )
        {
            return matchedItems;
        }
        else
        {
            Debug.LogError($"Item is not found!");
            return null; // �p�G�S�����A��^ null
        }
    }
    
    private void AddComponentsAndAssignPropertiesItem(GameObject item , Tables table)
    {
        /// �ھ� ID �d��������C������]���]�W�ٻP item.Id �ۦP�^
        //  GameObject itemObject = FindItem(item.Id);

        LDtkFields fields = item.GetComponent<LDtkFields>();

        string name = fields.GetString("item");

        if (name == null)
        {
            Debug.LogWarning($"�䤣��W�٬� '{item}' ���C������A�L�k���[�ݩʡI");
            return;
        }
        
        // �T�O�Ӫ���W�� ItemComponent
        ItemComponent itemComponent = item.GetComponent<ItemComponent>();
        if (itemComponent == null)
        {
            itemComponent = item.AddComponent<ItemComponent>();
        }

        foreach (var member in table.Tbitem.DataList)
        {
            if (name == member.Id)
            {
                itemComponent.SetProperties(member,table);
                Debug.Log($"�w������ '{member.Id}' ���[�ݩ�: {member}");  // �]�w�ݩ�
            }
        }
    }
}
    
