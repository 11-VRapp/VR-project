using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    private NavMeshAgent _speaker;
    [SerializeField] private Transform _player;
    [SerializeField] private Stage0 _stage0;
    [SerializeField] private Stage1 _stage1;
    [SerializeField] private Stage2 _stage2;
    [SerializeField] public Stage3 _stage3;
    [SerializeField] public StageFollow _stageFollow;

    private bool esterno;
    public Stages stages;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _stage0 = GetComponent<Stage0>();
        _stage1 = GetComponent<Stage1>();
        _stage2 = GetComponent<Stage2>();
        _stage3 = GetComponent<Stage3>();
        _stageFollow = GetComponent<StageFollow>();
        _stage0.imageBox.transform.localScale = Vector3.zero;
    }

    public void StartDialogue(Dialogue dialogue, Transform speaker)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _speaker = speaker.GetComponent<NavMeshAgent>();
        _speaker.speed = 0f;
        _player.GetComponent<AntMovement>().canMove = false;
        _player.GetComponent<PlayerLook>().enabled = false;

        Vector3 dirToTarget = _player.GetChild(0).transform.localPosition - speaker.transform.localPosition; //da fixare
        dirToTarget.y = 0f;
        dirToTarget.Normalize();
        speaker.DOLocalRotate(dirToTarget, 1f);

        stages = Stages.stage0;

        _stage0.nameText.text = dialogue.name;
        _stage0.dialogueText_text = dialogue.fraseIniziale;

        _stage1.nameText.text = dialogue.name;
        _stage1.dialogueText_text = dialogue.stage1_CourtesyQuestion;

        _stage2.nameText.text = dialogue.name;
        _stage2.createStage(dialogue);

        _stage3.nameText.text = dialogue.name;

        _stageFollow.nameText.text = dialogue.name;
        _stageFollow.answer_no = dialogue.follow_no;
        _stageFollow.answer_yes = dialogue.follow_yes;
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
                else
                {
                    if (!_speaker.GetComponent<AntFSM>().following)                    
                        _stage1.followBtn.GetComponentInChildren<Text>().text = "Seguimi";
                    else
                        _stage1.followBtn.GetComponentInChildren<Text>().text = "Non seguirmi più";                    
                }
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
        _stageFollow.imageBox.SetActive(false);
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _speaker.speed = 3f;
        _player.GetComponent<AntMovement>().canMove = true;
        _player.GetComponent<PlayerLook>().enabled = true;
    }

    public void followPlayer() //called by button    //if following is already true stop following! (maybe in another stage or like esterno)
    {        
        if (!_speaker.GetComponent<AntFSM>().following)
        {
            _stageFollow.imageBox.SetActive(true);
            StopAllCoroutines();
            //set ant status to follow player
            //if ant busy (not in wander state) show negative response canvas (that has a ok button that just hide it again)
            if (_speaker.GetComponent<AntFSM>().canFollow())
                StartCoroutine(TypeSentence(_stageFollow.answer, _stageFollow.answer_yes));
            else
            {                
                string state = _speaker.GetComponent<AntFSM>().getFSMstate();                
                StartCoroutine(TypeSentence(_stageFollow.answer, _stageFollow.getPhraseByState(state)));
            }
                
            //if yes it will already following, show canvas again
            stages++; //so goback works
        }
        else
        {
            _speaker.GetComponent<AntFSM>().following = false;
            stages--;
            DisplayNextStage();
        }
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
