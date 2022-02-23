using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenInteract : Interactable
{   
    public override void Interact(GameObject caller) 
    {
        StartCoroutine(FindObjectOfType<TutorialManager>().triggerQueenDialogue());
        transform.GetComponent<DialogueTrigger>().TriggerDialogue();
    }
    
}
