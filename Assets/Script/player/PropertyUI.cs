using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;

public class PropertyUI : MonoBehaviour
{
    public static PropertyUI Instance;
    public Braveattr player;
    public Inventory inventory;
    public TextMeshProUGUI bstate;
    public TextMeshProUGUI state;
    public TextMeshProUGUI blevel;
    public TextMeshProUGUI bhp;
    public TextMeshProUGUI batk;
    public TextMeshProUGUI bdef;
    public TextMeshProUGUI bexp;


    public TextMeshProUGUI bykey;
    public TextMeshProUGUI bbkey;
    public TextMeshProUGUI brkey;
    public TextMeshProUGUI bgold;

    public TextMeshProUGUI bfloor;

    public Slider BravebreathSlider;


    private void Awake()
    {
        Instance = this;
        player = FindFirstObjectByType<Braveattr>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BravebreathSlider.value = 0;
        BravebreathSlider.maxValue = 40;
    }

    private void Update()
    {   
        BravebreathSlider.value = Braveattr.GetAttribute("Breath") % 40;
        state.text = "狀態:";
        bstate.text = " ";
        blevel.text = "等級: " + Braveattr.GetAttribute("Level") ;
        bhp.text = "生命: " + Braveattr.GetAttribute("Hp");
        batk.text = "攻擊: " + Braveattr.GetAttribute("Atk");
        bdef.text = "防禦: " + Braveattr.GetAttribute("Def");
        bexp.text = "經驗: " + Braveattr.GetAttribute("Exp");
        bykey.text = "X  " + inventory.GetItemQuantity("黃鑰匙");
        bbkey.text = "X  " + inventory.GetItemQuantity("藍鑰匙");
        brkey.text = "X  " + inventory.GetItemQuantity("紅鑰匙");
        bgold.text = "X  " + Braveattr.GetAttribute("Gold");
        bfloor.text = "主塔 " + Braveplayer.floor +"F";
    }

    // Update is called once per frame

        



        



}

