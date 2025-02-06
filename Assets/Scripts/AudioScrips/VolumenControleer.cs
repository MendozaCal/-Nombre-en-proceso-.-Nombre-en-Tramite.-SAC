using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumenControleer : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider Master;
    public Slider musicaSlider;
    public Slider SFXSlider;

    private void Awake()
    {
        musicaSlider.onValueChanged.AddListener(ControlMusicVolumen);
        SFXSlider.onValueChanged.AddListener(ControlSFXVolumen);
        Master.onValueChanged.AddListener(ControlMasterVolumen);
    }
    void Start()
    {
        Cargar();
    }

    private void ControlMusicVolumen(float valor)
    {
        if (valor <= 0.0001f) valor = 0.0001f;
        mixer.SetFloat("VolumenMusic", Mathf.Log10(valor) * 20);
        PlayerPrefs.SetFloat("VolumenMusic", musicaSlider.value);
    }
    private void ControlSFXVolumen(float valor)
    {
        if (valor <= 0.0001f) valor = 0.0001f;
        mixer.SetFloat("VolumenSFX", Mathf.Log10(valor) * 20);
        PlayerPrefs.SetFloat("VolumenSFX", SFXSlider.value);
    }
    private void ControlMasterVolumen(float valor)
    {
        if (valor <= 0.0001f) valor = 0.0001f;
        mixer.SetFloat("VolumenMaster", Mathf.Log10(valor) * 20);
        PlayerPrefs.SetFloat("VolumenMaster", Master.value);
    }
    void Cargar()
    {
        musicaSlider.value = PlayerPrefs.GetFloat("VolumenMusica", 1);
        SFXSlider.value = PlayerPrefs.GetFloat("VolumenSFX", 1);
        Master.value = PlayerPrefs.GetFloat("VolumenMaster", 1);

        ControlMusicVolumen(musicaSlider.value);
        ControlSFXVolumen(SFXSlider.value);
        ControlMasterVolumen(Master.value);
    }
}
