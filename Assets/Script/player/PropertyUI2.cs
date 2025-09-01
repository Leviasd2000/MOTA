using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PropertyUI2 : MonoBehaviour
{
    public static PropertyUI2 Instance;
    public Image sword;
    public Image shield;
    private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        Addressables.LoadAssetsAsync<Sprite>("Tool", sprite =>
        {
            spriteDict[sprite.name] = sprite;
        }, true);
    }

    private void Update()
    {
        if (spriteDict.ContainsKey(Braveattr.HoldSword))
        {
            sword.sprite = spriteDict[Braveattr.HoldSword];
        }
        
        if (spriteDict.ContainsKey(Braveattr.HoldShield))
        {
            shield.sprite = spriteDict[Braveattr.HoldShield];
        }
    }
}
