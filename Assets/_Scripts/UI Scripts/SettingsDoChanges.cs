using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDoChanges : MonoBehaviour
{

    public TMPro.TMP_Dropdown resolutionDropdown;

    public Slider brightnessSlider;
    public Light directionalLight;

    // Start is called before the first frame update
    void Start()
    {
        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);

        brightnessSlider.onValueChanged.AddListener(ChangeLightIntensity);
        brightnessSlider.value = directionalLight.intensity;
    }

    void ChangeResolution(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(3840, 2160, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(2560, 1440, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
        }
    }
    void ChangeLightIntensity(float value)
    {
        directionalLight.intensity = value;
    }
}
