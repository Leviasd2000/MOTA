using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Toolslot : MonoBehaviour
{
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    [Header("�Ϥ�")]
    public Image image;

    private string slotName;

    void Start()
    {
        slotName = gameObject.name;
        image = GetComponent<Image>();

        // ���J�Ϲ�
        Addressables.LoadAssetsAsync<Sprite>("Tool", sprite =>
        {
            spriteDict[sprite.name] = sprite;
        }, true).Completed += OnSpriteLoadComplete;


    }

    private void OnSpriteLoadComplete(AsyncOperationHandle<IList<Sprite>> handle)
    {
        // �T�O���������Ϥ��A�]�w
        if (spriteDict.ContainsKey(slotName))
        {
            image.sprite = spriteDict[slotName];
            Debug.Log($"slotName = {slotName}");
            Debug.Log($"�Ҧ����J�� sprite key�G{string.Join(", ", spriteDict.Keys)}");
        }
        else
        {
            Debug.LogWarning($"�䤣��Ϥ��������W�١G{slotName}");
        }
    }

    private void Update()
    {
        Equip(slotName);
    }

    public void Equip(string name)
    {
        // �ھ� Braveattr �P�_�O�_��ܳo�� slot
        if (Braveattr.Sword.Contains(name) || Braveattr.Shield.Contains(name))
        {
            Debug.Log("�ثe�֦����C���G" + string.Join(", ", Braveattr.Sword));
            gameObject.GetComponent<Image>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = false;
        }

    }
}
