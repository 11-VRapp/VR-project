using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Stage0 _stage0;
    [SerializeField] private Stage1 _stage1;
    [SerializeField] private Stage2 _stage2;
    [SerializeField] public Stage3 _stage3;

    private bool esterno;
    public Stages stages;

    void Start()
    {
        _stage0 = GetComponent<Stage0>();
        _stage1 = GetComponent<Stage1>();
        _stage2 = GetComponent<Stage2>();
        _stage3 = GetComponent<Stage3>();
        _stage0.imageBox.transform.localScale = Vector3.zero;    
    }

    public void StartDialogue(Dialogue dialogue)
    {
        stages = Stages.stage0;

        _stage0.nameText.text = dialogue.name;
        _stage0.dialogueText_text = dialogue.fraseIniziale;

        _stage1.nameText.text = dialogue.name;
        _stage1.dialogueText_text = dialogue.stage1_CourtesyQuestion;

        _stage2.nameText.text = dialogue.name;
        _stage2.createStage(dialogue);

         _stage3.nameText.text = dialogue.name;

        esterno = dialogue.esterno;

        DisplayNextStage();
    }

    public void DisplayNextStage()
    {
        switch (stages)
        {
            case Stages.stage0:
                _stage0.imageBox.SetActive(true);
                _stage0.imageBox.transform.DOScale(Vector3.one, .5f).SetEase(Ease.InSine);
                StopAllCoroutines();
                StartCoroutine(TypeSentence(_stage0.dialogueText, _stage0.dialogueText_text));
                break;
            case Stages.stage1:
                _stage0.imageBox.SetActive(false);
                _stage2.imageBox.SetActive(false);
                _stage1.imageBox.transform.DOScale(Vector3.one, 0f);
                _stage1.imageBox.SetActive(true);                
                if (!esterno)
                    _stage1.followBtn.SetActive(false);
                StopAllCoroutines();
                StartCoroutine(TypeSentence(_stage1.dialogueText, _stage1.dialogueText_text));
                break;
            case Stages.stage2:
                _stage1.imageBox.SetActive(false);
                _stage3.imageBox.SetActive(false);
                _stage2.imageBox.SetActive(true);                
                break;
            case Stages.stage3:
                _stage2.imageBox.SetActive(false);
                _stage3.imageBox.SetActive(true);                
                break;
            case Stages.End:
                EndDialogue();                
                return;
        };
        stages++;
    }

    IEnumerator TypeSentence(Text field, string sentence)
    {
        field.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            field.text += letter;
            yield return null;
        }
    }

    public void goBack()
    {
        stages -= 2;
        DisplayNextStage();
    }

    
    public void writeAnswer()
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(_stage1.dialogueText, _stage1.dialogueText_text));
    }

    public void EndDialogue()
    {
        stages = Stages.stage0;
        _stage1.imageBox.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBounce);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }


    public enum Stages
    {
        stage0,
        stage1,
        stage2,
        stage3,
        End
    }

}
