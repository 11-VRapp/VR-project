using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class larvaInteract : Interactable
{  
    public override void Interact(GameObject caller) {
        caller.GetComponent<FPSInteractionManager>().feeded = false;
    }
}
