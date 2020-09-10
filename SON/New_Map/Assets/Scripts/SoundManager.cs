using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSound;
    public AudioSource bgmSound;
    public AudioSource keyboardSound;
    public static SoundManager instance = null;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void PlaySingle(AudioClip clip)
    {
        efxSound.clip = clip;
        efxSound.Play();
    }
    public void BgmSingle(AudioClip clip)
    {
        bgmSound.clip = clip;
        bgmSound.Play();
    }
    public void KeyboardSingle(AudioClip clip)
    {
        keyboardSound.clip = clip;
        keyboardSound.Play();       
    }
    public void StopKeyboard()
    {
        keyboardSound.Stop();
    }
}
