using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBundaryDeathCheck : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _spawnPosition;
    void OnTriggerEnter()
    {
        _player.position = _spawnPosition.position;
        _player.forward = _spawnPosition.forward;
    }
}
