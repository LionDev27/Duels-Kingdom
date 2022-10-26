using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChangeSliderValuesWithPlayerprefs : MonoBehaviour
{
    [SerializeField] Slider main, music, sfx;

    private void Start()
    {
        SetPlayerPrefsValuesToSliders();
    }

    void SetPlayerPrefsValuesToSliders()
    {
        AudioController aC = AudioController.instance;
        //La inversa de la fórmula aplicada a los sliders.
        main.value = Mathf.Pow(10, PlayerPrefs.GetFloat("MasterVolume") / 20);
        main.onValueChanged.AddListener((x) => aC.PlaySFX(aC.menuNav));
        music.value = Mathf.Pow(10, PlayerPrefs.GetFloat("MusicVolume") / 20);
        sfx.value = Mathf.Pow(10, PlayerPrefs.GetFloat("FxVolume") / 20);
        sfx.onValueChanged.AddListener((x) => aC.PlaySFX(aC.menuNav));
    }
}
