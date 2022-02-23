using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomTrigger : MonoBehaviour
{
    public bool insideTrigger;
    void Start()
    {
        insideTrigger = false;
    }

    void OnTriggerEnter(Collider col)
    {       
        if (col.transform.tag == "Player")
            insideTrigger = true;
    }

    void OnTriggerExit(Collider col)
    {       
        if (col.transform.tag == "Player")
            insideTrigger = false;
    }
}
