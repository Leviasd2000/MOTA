using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.U2D;
using TMPro;
using Yarn.Unity;
using System;
using UnityEditor.VersionControl;
using System.Linq;


public class ImageLoad : DialogueViewBase
{
    public TextMeshProUGUI character; 
    public DialogueRunner dialogueRunner;
    public Image targetImage;  // 🎯 指定 UI Image 物件
    public Braveplayer player;
    public GameObject Player;
    public GameObject ContinuousButtom;
    public LineView lineView;
    private List<Sprite> loadedSprites = new List<Sprite>();


    void Start()
    {
        // 載入所有 NPC 相關的圖片
        Addressables.LoadAssetsAsync<Sprite>("NPC", sprite =>
        {
            loadedSprites.Add(sprite); // 存入 List
            Debug.Log($"載入圖片：{sprite.name}");
        }, true);

        player = FindFirstObjectByType<Braveplayer>();
        if (player != null)
        {
            Player = player.gameObject; // 指定 Player
        }
        else
        {
            Debug.LogError("未找到 Braveplayer 物件！");
        }
        ContinuousButtom.SetActive(false);
        character.text = "勇者";
    }

    

    private void Update()
    {
        if (dialogueRunner != null)
        {
            if (ContinuousButtom.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    lineView.UserRequestedViewAdvancement();
                    UpdateCharacterImage();
                    Debug.Log("有");
                }
            }
        }
    }




    // 當角色改變時更新圖片
    public async void UpdateCharacterImage()
    {
        await System.Threading.Tasks.Task.Delay(100);
        if (character == null || string.IsNullOrEmpty(character.text))
        {
            Debug.LogError("character 或 character.text 尚未設置！");
            return;
        }
        
        foreach (Sprite sprite in loadedSprites)
        {   
            if (character.text == null)
            {
                return;
            }
            if (sprite.name == character.text)  // 找到匹配的圖片
            {
                targetImage.sprite = sprite;
                Debug.Log($"更新圖片：{character.text}");
                return;
            }
        }
    }


    public void OnDialogueEnd() // 怪bug player會無緣無故就消失了
    {
        if (Player == null)
        {
            Player = FindFirstObjectByType<Braveplayer>()?.gameObject;
        }

        if (Player != null)
        {
            Debug.Log("🎉 Yarn Spinner 对话结束！");
            Player.GetComponent<Braveplayer>().enabled = true;
            Player.GetComponent<Animator>().enabled = true;
        }
        else
        {
            Debug.LogError("找不到 Player 物件！");
        }
    }


}

