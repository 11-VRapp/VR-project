using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntNPC_int_INIT : MonoBehaviour
{
    void Start() => StartCoroutine(GetComponent<AntNPC_int>().randomMovement());    
}
