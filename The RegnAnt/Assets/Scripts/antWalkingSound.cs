using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class antWalkingSound : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private AudioManager _audio;
    private NavMeshAgent _navMeshAgent;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        if (_navMeshAgent != null)
        {
            if (_navMeshAgent.velocity.magnitude <= 2f)
                _audio.Pause("Walk");

            if (_navMeshAgent.velocity.magnitude > 2f && !_audio.audioIsPlaying("Walk"))
                _audio.Play("Walk");
            
            return;
        }

        if (_rigidbody.velocity.magnitude <= 2f)
            _audio.Pause("Walk");

        if (_rigidbody.velocity.magnitude > 2f && !_audio.audioIsPlaying("Walk"))
            _audio.Play("Walk");
    }
}
