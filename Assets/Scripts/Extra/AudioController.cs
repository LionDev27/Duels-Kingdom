using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    //Referencias a los audio source de nuestro juego.
    public AudioSource sfxSource, musicSource;

    //Referencia al audiomixer principal.
    public AudioMixer masterMixer;

    public AudioClip menuNav, death, hit, jump, pickItem, throwItem;

    [SerializeField] float fadeOutTime;

    float defaultMusicVolume;

    //SINGLETON
    public static AudioController instance;

    private void Awake()
    {
        if (!instance) instance = this;
        DontDestroyOnLoad(instance);
    }

    private void Start()
    {
        defaultMusicVolume = musicSource.volume;
    }

    public void PlaySFX(AudioClip obj)
    {
        //Reproducimos el sonido obtenido una sóla vez.
        sfxSource.PlayOneShot(obj);
    }

    public void PlayMusic(AudioClip obj)
    {
        musicSource.volume = defaultMusicVolume;
        musicSource.clip = obj;
        musicSource.Play();
    }

    public IEnumerator DecreaseMusicVolumeInTime(AudioClip obj)
    {
        float timer = 0;

        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(defaultMusicVolume, 0f, timer);
            yield return null;
        }

        PlayMusic(obj);
    }

    public void SetMasterVolume(float value)
    {
        //Lo hacemos de esta forma para que el volumen disminuya de forma exponencial.
        //Esto arregla que, a partir de la mitad del slider aproximadamente, no escuchemos nada.
        masterMixer.SetFloat("masterVol", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.Save();
    }
    public void SetMusicVolume(float value)
    {
        masterMixer.SetFloat("musicVol", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.Save();
    }
    public void SetSFXVolume(float value)
    {
        masterMixer.SetFloat("sfxVol", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("FxVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.Save();
    }
}
