using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;

	
	[SerializeField] private GameObject _dialogueBox;

	private Queue<string> sentences;

	
	void Start () {
		_dialogueBox.transform.localScale = Vector3.zero;
		sentences = new Queue<string>();
	}

	public void StartDialogue (Dialogue dialogue)
	{
		
		_dialogueBox.transform.DOScale(Vector3.one, .5f).SetEase(Ease.InSine);

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		_dialogueBox.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBounce);
		//Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
	}

}
