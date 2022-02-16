using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioGeneral : MonoBehaviour
{  
    void Start()
    {
        GetComponent<AudioManager>().Play("WindAudio");
    }

    
}
