using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCinteract : Interactable
{   
    public override void Interact(GameObject caller) => transform.GetComponent<DialogueTrigger>().TriggerDialogue();
    
}
