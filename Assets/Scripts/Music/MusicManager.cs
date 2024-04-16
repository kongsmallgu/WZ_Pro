using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance; // ����ģʽ��ȷ��ֻ��һ��ʵ��

    public AudioSource musicSource; // ���ڲ��ű������ֵ�AudioSource���
    public AudioSource soundEffectSource; // ���ڲ�����Ч��AudioSource���

    public AudioClip backgroundMusic; // ��������
    public AudioClip[] soundEffects; // ��Ч���飬������Ӷ����Ч

    void Awake()
    {
        // ȷ��ֻ��һ��ʵ������
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // ���л�����ʱ������ʵ��
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // ���ű�������
        PlayMusic(backgroundMusic);
    }

    // ���ű�������
    public void PlayMusic(AudioClip musicClip)
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    // ֹͣ��������
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // ����ָ����������Ч
    public void PlaySoundEffect(int index)
    {
        if (index >= 0 && index < soundEffects.Length)
        {
            //soundEffectSource.PlayOneShot(soundEffects[index]);
            Debug.Log("����������Ч��=========");
        }
        else
        {
            Debug.LogWarning("��Ч����������Χ��");
        }
    }
}
