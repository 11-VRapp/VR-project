using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameObject _antSpawnPrefab;
    [SerializeField] private Transform fightersAntsParent;

    [SerializeField] private List<Transform> spiderLegsEnd = new List<Transform>();
    
    public void spawnAnt()
    {
        GameObject ant = GameObject.Instantiate(_antSpawnPrefab, _spawnPosition.position, Quaternion.Euler(0f, 0f, 0f), fightersAntsParent);
        ant.GetComponent<antNPCfight>().spiderLegsEnd = this.spiderLegsEnd;
    }
}
