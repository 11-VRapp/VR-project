using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BrightnessController : MonoBehaviour
{

    ColorGrading colorGradingLayer = null;
    PostProcessVolume volume;
    void Start()
    {        
        volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out colorGradingLayer);
        colorGradingLayer.enabled.value = true;
        colorGradingLayer.brightness.value = PlayerPrefs.GetFloat("masterBrightness");        
    }

    public void updateBrightness(float brightness)
    {
        volume.profile.TryGetSettings(out colorGradingLayer);
        colorGradingLayer.enabled.value = true;
        colorGradingLayer.brightness.value = brightness;   
    }
}
