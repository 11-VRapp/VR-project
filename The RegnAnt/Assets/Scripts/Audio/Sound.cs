using UnityEngine.Audio;
using UnityEngine;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1;
    [Range(.1f, 3f)]
    public float pitch = 1;
    public bool loop;

    [Range(-1f, 1f)]
    public float stereoPan = 0;

    [Range(0f, 1f)]
    public float audio3D;

    [HideInInspector]
    public AudioSource source;
}
