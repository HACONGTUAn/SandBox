using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 public enum audio
{
    vacham,
    phahuy,
    trungdich
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    [Header("audio va cham")]
    public AudioClip audioClip_vacham;

    [Header("audio pha huy")]
    public AudioClip audioClip_phahuy;

    [Header("audio pha trung dich")]
    public AudioClip audioClip_trungdich;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void PlayAudio(audio checkType)
    {
        switch (checkType)
        {
            case global::audio.trungdich:
                audioSource.clip = audioClip_trungdich;
                audioSource.Play();
                break;

            case global::audio.phahuy:
                audioSource.clip = audioClip_phahuy;
                audioSource.Play();
                break;

            case global::audio.vacham:
                audioSource.clip = audioClip_vacham;
                audioSource.Play();
                break;
        }
    }
}
