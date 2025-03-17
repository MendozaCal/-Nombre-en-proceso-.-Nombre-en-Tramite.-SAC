using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlowController : MonoBehaviour
{
    public Light sceneLight; 
    public Slider brightnessSlider;

    void Start()
    {
        brightnessSlider.value = sceneLight.intensity;
    }

    public void UpdateBrightness(float value)
    {
        sceneLight.intensity = value;
    }
}
