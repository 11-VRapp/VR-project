using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    void Awake()
    {
        foreach (Sound s in sounds)
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

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Play();
    }

    public Sound getClip(string name) => Array.Find(sounds, sound => sound.name == name);      

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Stop();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Pause();
    }

    public bool audioIsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            return s.source.isPlaying;
        return false;
    }

    public void PlayWithRandomPitch(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.pitch = UnityEngine.Random.Range(0.1f, 3f);
            s.source.Play();
        }
    }
}
