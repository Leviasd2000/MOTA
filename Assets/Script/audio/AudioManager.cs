using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Án­µ¨Ó·½")]
    public AudioClip upperbg;
    public AudioClip lowerbg;
    public AudioClip braveattack;
    public AudioClip monsterattack;
    public AudioClip Door;
    public AudioClip Item;
    public AudioClip Click;

    
    List<AudioSource> audios = new List<AudioSource>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i<7; i++)
        {
            var audio = this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(int index , string name , bool isLoop)
    {
        var clip = GetAudioClip(name);
        if (clip != null)
        {
            var audio = audios[index];
            audio.clip = clip;
            audio.loop = isLoop;
            audio.Play();
           
        }
    }

    AudioClip GetAudioClip(string name)
    {
        switch (name)
        {
            case "upperbg":
                return upperbg;
            case "lowerbg":
                return lowerbg;
            case "Door":
                return Door;
            case "Item":
                return Item;
            case "Click":
                return Click;
        }
        return null;
    }
      
    
}   
