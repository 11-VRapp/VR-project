using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antWalkingSound : MonoBehaviour
{
    private Rigidbody _rigidbody;    
    private AudioManager _audio;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioManager>();
    }


    void Update()
    {
        if (_rigidbody.velocity.magnitude <= 2f)
            _audio.Pause("Walk");

        if (_rigidbody.velocity.magnitude > 2f && !_audio.audioIsPlaying("Walk"))
            _audio.Play("Walk");      
    }
}
