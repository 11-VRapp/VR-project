using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorldBoundaryWarningCheck : MonoBehaviour
{
    [SerializeField] private Transform _playerBody;
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private GameObject compass;
    void OnTriggerEnter(Collider col)
    {
        if(col.transform == _playerBody)
            warning();       
    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log(col.transform.name);
        if(col.transform == _playerBody)
        {
            warningPanel.SetActive(false);   
            StopAllCoroutines();
        }
               
    }

    private void warning()
    {
        warningPanel.SetActive(true);
        StartCoroutine(shake());
    }

    private IEnumerator shake()
    {
        float strength = 0.1f;
        while(true)
        {
            compass.transform.GetChild(0).DOShakeRotation(2, new Vector3 (0, 0, 1) * strength);
            strength += 0.01f;
            yield return null;
        }
    }
}
