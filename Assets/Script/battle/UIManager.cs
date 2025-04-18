using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using DG.Tweening;

public class UIManager : MonoBehaviour
{   
    public static UIManager Instance;
    public TextMeshProUGUI bname;
    public TextMeshProUGUI bhp;
    public TextMeshProUGUI batk;
    public TextMeshProUGUI bdef;
    public TextMeshProUGUI bfatigue;

    public TextMeshProUGUI mname;
    public TextMeshProUGUI mhp;
    public TextMeshProUGUI matk;
    public TextMeshProUGUI mdef;
    public TextMeshProUGUI mfatigue;

    public TextMeshProUGUI mmoney;
    public TextMeshProUGUI mexp;

    public Slider MonsterbreathSlider;
    public Slider BravebreathSlider;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    public void BraveUpdateBreath(Braveattr braveattr)
    {
        BravebreathSlider.value = Braveattr.GetAttribute("Breath") % 40;
    }

    public void MonsterUpdateBreath(MONSTER monster)
    {
        MonsterbreathSlider.value = monster.CurrentBreath;
    }

    public void BraveUpProperties(Braveattr braveattr)
    {
        bhp.SetText(Braveattr.GetAttribute("Hp") + "\u00A0:生命");
        Debug.Log(bhp.text);
        batk.text = Braveattr.GetAttribute("Atk") + "\u00A0:攻擊";
        bdef.text = Braveattr.GetAttribute("Def") + "\u00A0:防禦";
        bfatigue.text = Braveattr.GetAttribute("Fatigue") + "\u00A0:疲勞";
    }

    public void MonsterUpProperties(MONSTER monster)
    {   
        if (monster.CurrentHp >= 0)
        {
            mhp.text = "生命:\u00A0" + monster.CurrentHp;
        }
        else
        {
            mhp.text = "生命:\u00A00" ;
        }
        matk.text = "攻擊:\u00A0" + monster.Atk;
        mdef.text = "防禦:\u00A0" + monster.Def;
        mfatigue.text = "疲勞:\u00A0" + monster.Fatigue;
        Debug.Log("改到了!");


    }
    
    public IEnumerator InitHUDBrave(Braveattr braveattr)
    {
        bname = braveattr.bravename;
        bhp.text = Braveattr.GetAttribute("Hp") + "\u00A0:生命";
        batk.text = Braveattr.GetAttribute("Atk") + "\u00A0:攻擊";
        bdef.text = Braveattr.GetAttribute("Def") + "\u00A0:防禦";
        bfatigue.text = Braveattr.GetAttribute("Fatigue") + "\u00A0:疲勞";
        BravebreathSlider.value = Braveattr.GetAttribute("Breath") ;
        BravebreathSlider.maxValue = 40;
        BravebreathSlider.minValue = 0;

        yield return null;
    }
    public IEnumerator InitHUDMonster(MONSTER monster)
    {
        mname.text = monster.Name;
        mhp.text = "生命:\u00A0" + monster.Hp;
        Debug.Log(mhp.text);
        matk.text = "攻擊:\u00A0" + monster.Atk;
        mdef.text = "防禦:\u00A0" + monster.Def;
        mfatigue.text = "疲勞:\u00A0" + monster.Fatigue;
        mmoney.text = "金錢:\u00A0" + "<color=#FFD700>" + monster.Gold + "</color>";
        mexp.text = "經驗值:\u00A0" + "<color=#F37DBF>" + monster.Exp + "</color>";
        MonsterbreathSlider.value = 0;
        MonsterbreathSlider.maxValue = monster.Breath;
        yield return null;

    }
    
}
