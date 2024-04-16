using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance; // 单例模式，确保只有一个实例

    public AudioSource musicSource; // 用于播放背景音乐的AudioSource组件
    public AudioSource soundEffectSource; // 用于播放音效的AudioSource组件

    public AudioClip backgroundMusic; // 背景音乐
    public AudioClip[] soundEffects; // 音效数组，可以添加多个音效

    void Awake()
    {
        // 确保只有一个实例存在
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // 在切换场景时不销毁实例
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // 播放背景音乐
        PlayMusic(backgroundMusic);
    }

    // 播放背景音乐
    public void PlayMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    // 停止背景音乐
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // 播放指定索引的音效
    public void PlaySoundEffect(int index)
    {
        if (index >= 0 && index < soundEffects.Length)
        {
            //soundEffectSource.PlayOneShot(soundEffects[index]);
            Debug.Log("播放索引音效！=========");
        }
        else
        {
            Debug.LogWarning("音效索引超出范围！");
        }
    }
}
