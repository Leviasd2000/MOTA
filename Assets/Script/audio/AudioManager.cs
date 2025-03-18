using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class AudioManager : MonoBehaviour
{
    [Header("聲音來源")]
    public AudioClip firstone;

    private AudioSource audios;

    Dictionary<string , AudioClip> audioclip = new Dictionary<string, AudioClip>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audios = this.gameObject.AddComponent<AudioSource>();
        
        Addressables.LoadAssetsAsync<AudioClip>("Audio", clip =>
        {
            audioclip[clip.name]=clip; // 存入 List
            Debug.Log($"載入片段：{clip.name}");
        }, true);}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string name , bool isLoop)
    {
        var clip = GetAudioClip(name);
        if (clip != null)
        {   
            audios.clip = clip;
            audios.loop = isLoop;
            audios.Play();
           
        }
    }

    AudioClip GetAudioClip(string name)
    {
        return audioclip[name];
    }
      
    
}   
