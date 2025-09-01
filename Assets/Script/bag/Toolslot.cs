using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Toolslot : MonoBehaviour
{
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    [Header("圖片")]
    public Image image;

    private string slotName;

    void Start()
    {
        slotName = gameObject.name;
        image = GetComponent<Image>();

        // 載入圖像
        Addressables.LoadAssetsAsync<Sprite>("Tool", sprite =>
        {
            spriteDict[sprite.name] = sprite;
        }, true).Completed += OnSpriteLoadComplete;


    }

    private void OnSpriteLoadComplete(AsyncOperationHandle<IList<Sprite>> handle)
    {
        // 確保有找到對應圖片再設定
        if (spriteDict.ContainsKey(slotName))
        {
            image.sprite = spriteDict[slotName];
            Debug.Log($"slotName = {slotName}");
            Debug.Log($"所有載入的 sprite key：{string.Join(", ", spriteDict.Keys)}");
        }
        else
        {
            Debug.LogWarning($"找不到圖片對應的名稱：{slotName}");
        }
    }

    private void Update()
    {
        Equip(slotName);
    }

    public void Equip(string name)
    {
        // 根據 Braveattr 判斷是否顯示這個 slot
        if (Braveattr.Sword.Contains(name) || Braveattr.Shield.Contains(name))
        {
            gameObject.GetComponent<Image>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = false;
        }

    }
}
