using UnityEngine.Audio;
using System;
using UnityEngine;


public class VoiceOverManager : MonoBehaviour
{
    public Atto[] atti;

    public bool voiceOverBusy = false;

    void Awake()
    {
        foreach (Atto a in atti)
        {
            a.voiceManager = this;
            foreach (Sound s in a.sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.panStereo = s.stereoPan;
                s.source.spatialBlend = s.audio3D;
            }
        }

    }
}
