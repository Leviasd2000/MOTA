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
        // ��l�ƤT�Ӥ����� AudioSource
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
            Debug.Log($"���J���q�G{clip.name}");
        }, true);
    }

    // ���I�����֡]�u�|���@���^
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

    // ���u���ġ]�Ҧp�z���BUI�^

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

    // ������ʧ@���]�Ҧp�����^
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
        // ���令���A loop
        loopSource.loop = false;

        // �� clip ����
        yield return new WaitWhile(() => loopSource.isPlaying);

        // �A stop �T�O���A���b
        loopSource.Stop();
    }

    public void SetLoopPitch(float pitch)
    {
        loopSource.pitch = pitch;
    }
}

