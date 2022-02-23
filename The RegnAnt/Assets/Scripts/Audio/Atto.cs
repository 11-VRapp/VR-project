using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Atto
{
    public string Name;
    public Sound[] sounds;
    public VoiceOverManager voiceManager;  

/*
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Play();
    }

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
    }*/

    public IEnumerator playAudioSequentially()
    {
        yield return null;
        voiceManager.voiceOverBusy = true;

        //1.Loop through each AudioClip
        for (int i = 0; i < sounds.Length; i++)
        {
            //2.Assign current AudioClip to audiosource
            sounds[i].clip = sounds[i].clip;

            //3.Play Audio
            sounds[i].source.Play();

            //4.Wait for it to finish playing
            while (sounds[i].source.isPlaying)
            {
                yield return null;
            }

            //5. Go back to #2 and play the next audio in the adClips array
        }

        voiceManager.voiceOverBusy = false;
    }
}
