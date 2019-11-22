using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsPage : MonoBehaviour
{
    public AudioMixer mixer;
    public GameObject volumeSlider;
    public GameObject GlobalController;
    public GameObject sensSlider;

    private GlobalControl globalController;
    private Text volumeVisualNum;
    private Text sensitivityText;
    private Slider volume;
    private Slider sensitivity;
    public float initVolume = 0.5f;
    public float initSens = 2.5f;

    void Awake()
    {

        volume = volumeSlider.GetComponent<Slider>();
        sensitivity = sensSlider.GetComponent<Slider>();
        volumeVisualNum = volumeSlider.GetComponentInChildren<Text>();
        globalController = GlobalController.GetComponent<GlobalControl>();
        sensitivityText = sensSlider.GetComponentInChildren<Text>();

        if (PlayerPrefs.HasKey("volume"))
        {
            // get temporary float
            float wantedVolume = PlayerPrefs.GetFloat("volume", initVolume);

            //set slider val
            volume.value = wantedVolume;

            SetAudioVolume(wantedVolume);

        }
        else
        {
            //set volume on start
            volume.value = initVolume;
            SetAudioVolume(initVolume);
        }

        if (PlayerPrefs.HasKey("sensitivity"))
        {

            // get temporary float
            float wantedSens = PlayerPrefs.GetFloat("sensitivity", initSens);

            //set slider val
            sensitivity.value = wantedSens;
            SetSensitivity(wantedSens);
        }
        else
        {
            //set volume on start
            sensitivity.value = initSens;
            SetSensitivity(initVolume);
        }

        //DISABLE SCREEN 
        gameObject.SetActive(false);

    }

    private void Update()
    {
        volumeVisualNum.text = (volume.value* 10).ToString("f2");
        sensitivityText.text = (sensitivity.value * 5).ToString("f2");
    }

    public void SetAudioVolume(float sliderValue)
    {
        mixer.SetFloat("masterVol", Mathf.Log10(sliderValue) * 20);
        //saves volume for future plays
        PlayerPrefs.SetFloat("volume", sliderValue);
    }

    public void SetSensitivity(float sliderValue)
    {
        globalController.sensitivity = sliderValue * 5;
        // saves volume for future plays
        PlayerPrefs.SetFloat("sensitivity", sliderValue);
    }
}
