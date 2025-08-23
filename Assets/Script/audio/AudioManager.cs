using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;


public class AudioManager : MonoBehaviour
{
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    private AudioSource bgmSource;
    private AudioSource sfxSource;
    private AudioSource loopSource;

    void Awake()
    {
        // 初始化三個分類用 AudioSource
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

        sfxSource = gameObject.AddComponent<AudioSource>();

        loopSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        Addressables.LoadAssetsAsync<AudioClip>("Audio", clip =>
        {
            audioClips[clip.name] = clip;
            Debug.Log($"載入片段：{clip.name}");
        }, true);
    }

    // 播背景音樂（只會播一首）
    public void PlayBGM(string name, float volume = 1f)
    {
        if (audioClips.TryGetValue(name, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.volume = volume;
            bgmSource.Play();
        }
    }

    public void StopBGM() => bgmSource.Stop();

    // 播短音效（例如爆炸、UI）

    public void PlayFX(string name)
    {
        PlaySFX(name, 1f);
    }
    public void PlaySFX(string name, float volume = 1f)
    {
        if (audioClips.TryGetValue(name, out var clip))
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    // 播持續動作音（例如走路）
    public void PlayLoop(string name, float pitch = 1f)
    {
        if (audioClips.TryGetValue(name, out var clip))
        {
            loopSource.clip = clip;
            loopSource.pitch = pitch;
            loopSource.loop = true;
            loopSource.Play();
        }
    }

    public void StopLoop() => loopSource.Stop();

    public void StopLoopAfterThisClip()
    {
        if (loopSource.isPlaying)
        {
            StartCoroutine(WaitToStopLoop());
        }
    }

    private IEnumerator WaitToStopLoop()
    {
        // 先改成不再 loop
        loopSource.loop = false;

        // 等 clip 播放完
        yield return new WaitWhile(() => loopSource.isPlaying);

        // 再 stop 確保狀態乾淨
        loopSource.Stop();
    }

    public void SetLoopPitch(float pitch)
    {
        loopSource.pitch = pitch;
    }
}

