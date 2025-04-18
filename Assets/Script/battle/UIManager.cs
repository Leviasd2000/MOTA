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
        bhp.SetText(Braveattr.GetAttribute("Hp") + "\u00A0:�ͩR");
        Debug.Log(bhp.text);
        batk.text = Braveattr.GetAttribute("Atk") + "\u00A0:����";
        bdef.text = Braveattr.GetAttribute("Def") + "\u00A0:���m";
        bfatigue.text = Braveattr.GetAttribute("Fatigue") + "\u00A0:�h��";
    }

    public void MonsterUpProperties(MONSTER monster)
    {   
        if (monster.CurrentHp >= 0)
        {
            mhp.text = "�ͩR:\u00A0" + monster.CurrentHp;
        }
        else
        {
            mhp.text = "�ͩR:\u00A00" ;
        }
        matk.text = "����:\u00A0" + monster.Atk;
        mdef.text = "���m:\u00A0" + monster.Def;
        mfatigue.text = "�h��:\u00A0" + monster.Fatigue;
        Debug.Log("���F!");


    }
    
    public IEnumerator InitHUDBrave(Braveattr braveattr)
    {
        bname = braveattr.bravename;
        bhp.text = Braveattr.GetAttribute("Hp") + "\u00A0:�ͩR";
        batk.text = Braveattr.GetAttribute("Atk") + "\u00A0:����";
        bdef.text = Braveattr.GetAttribute("Def") + "\u00A0:���m";
        bfatigue.text = Braveattr.GetAttribute("Fatigue") + "\u00A0:�h��";
        BravebreathSlider.value = Braveattr.GetAttribute("Breath") ;
        BravebreathSlider.maxValue = 40;
        BravebreathSlider.minValue = 0;

        yield return null;
    }
    public IEnumerator InitHUDMonster(MONSTER monster)
    {
        mname.text = monster.Name;
        mhp.text = "�ͩR:\u00A0" + monster.Hp;
        Debug.Log(mhp.text);
        matk.text = "����:\u00A0" + monster.Atk;
        mdef.text = "���m:\u00A0" + monster.Def;
        mfatigue.text = "�h��:\u00A0" + monster.Fatigue;
        mmoney.text = "����:\u00A0" + "<color=#FFD700>" + monster.Gold + "</color>";
        mexp.text = "�g���:\u00A0" + "<color=#F37DBF>" + monster.Exp + "</color>";
        MonsterbreathSlider.value = 0;
        MonsterbreathSlider.maxValue = monster.Breath;
        yield return null;

    }
    
}
