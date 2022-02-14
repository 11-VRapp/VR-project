using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    private NavMeshAgent _speaker;
    private Transform _speakerTransform;
    [SerializeField] private Transform _player;
    [SerializeField] private Stage0 _stage0;
    [SerializeField] private Stage1 _stage1;
    [SerializeField] private Stage2 _stage2;
    [SerializeField] public Stage3 _stage3;
    [SerializeField] public StageFollow _stageFollow;
    private Transform _lastPosition;

    private bool esterno;

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

        if (speaker.GetComponent<NavMeshAgent>() != null) //all'interno non si usa la navmesh
        {
            _speaker = speaker.GetComponent<NavMeshAgent>();
            _speaker.speed = 0f;
        }

        _speakerTransform = speaker;

        _player.GetComponent<AntMovement>().canMove = false;
        _player.GetComponent<PlayerLook>().enabled = false;
        _player.GetComponent<FPSInteractionManager>().Interaction(speaker.GetComponent<Interactable>());

        speaker.GetComponent<DialogueTrigger>().dialogueEnd = false;
        
        _lastPosition = _speakerTransform.transform;
        StartCoroutine(rotateTowards(5f, 3f, _player.position));       

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

        DisplayStage0();
    }    

    public void DisplayStage0()
    {
        _stage0.imageBox.SetActive(true);
        _stage0.imageBox.transform.DOScale(Vector3.one, .5f).SetEase(Ease.InBounce);       

        StartCoroutine(TypeSentence(_stage0.dialogueText, _stage0.dialogueText_text));
    }

    public void DisplayStage1()
    {   
        if (!esterno)
            _stage1.followBtn.SetActive(false);
        else
        {
            if (!_speaker.GetComponent<AntFSM>().following)
                _stage1.followBtn.GetComponentInChildren<Text>().text = "Seguimi";
            else
                _stage1.followBtn.GetComponentInChildren<Text>().text = "Non seguirmi più";
        }

        StartCoroutine(TypeSentence(_stage1.dialogueText, _stage1.dialogueText_text));
    }

    public void DisplayStage3()
    {
        _stage3.imageBox.SetActive(true);
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

    public void EndDialogue()
    {     
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (_speaker != null)
            _speaker.speed = 3f;

        _player.GetComponent<AntMovement>().canMove = true;
        _player.GetComponent<PlayerLook>().enabled = true;
        _player.GetComponent<FPSInteractionManager>().Interaction(null);        

        _speakerTransform.GetComponent<DialogueTrigger>().dialogueEnd = true;
        StartCoroutine(rotateTowards(5f, 3f, _lastPosition.position));        
    }

    public void followPlayer() //called by button    //if following is already true stop following! (maybe in another stage or like esterno)
    {
        if (!_speaker.GetComponent<AntFSM>().following)
        {          
            //set ant status to follow player
            //if ant busy (not in wander state) show negative response canvas (that has a ok button that just hide it again)
            if (_speaker.GetComponent<AntFSM>().canFollow())
                StartCoroutine(TypeSentence(_stageFollow.answer, _stageFollow.answer_yes));
            else
            {
                string state = _speaker.GetComponent<AntFSM>().getFSMstate();
                StartCoroutine(TypeSentence(_stageFollow.answer, _stageFollow.getPhraseByState(state)));
            }           
        }
        else        
            _speaker.GetComponent<AntFSM>().following = false;           
        
    }

    public IEnumerator rotateTowards(float timeNeeded, float speed, Vector3 _target)
    {
        do
        {
            Vector3 dirToTarget = _target - _speakerTransform.position;
            dirToTarget.y = 0f;
            dirToTarget.Normalize();
            _speakerTransform.localRotation = Quaternion.Slerp(_speakerTransform.localRotation, Quaternion.LookRotation(dirToTarget), speed * Time.deltaTime);
            timeNeeded -= Time.deltaTime;
            yield return null;
        } while (timeNeeded > 0f);
    }
}
