using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Transform _startingAnt;
    [SerializeField] private Transform _eggAnt;
    [SerializeField] private Transform _storageAnt;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _cursor;
    [SerializeField] private GameObject _popUp;
    [SerializeField] private List<Transform> pathList = new List<Transform>();
    public bool grabbedEgg = false;
    private bool larvaFeeding = false;
    public bool grabbedAphid = false;
    [SerializeField] private GameObject _blinkCanvas;
    [SerializeField] private Collider _exitCollider;

    private int phase;
    void Start()
    {
        phase = 0;
        _popUp.SetActive(false);
        StartCoroutine(tutorialManager());
    }

    void Update()
    {
        switch (phase)
        {
            case 1:
                Vector3 dirToTarget = _player.position - _startingAnt.position;
                dirToTarget.y = 0f;
                dirToTarget.Normalize();
                _startingAnt.transform.rotation = Quaternion.Slerp(_startingAnt.transform.rotation, Quaternion.LookRotation(dirToTarget), 3f * Time.deltaTime);
                break;
            case 2:
                //transfer arrow to new ant                
                dirToTarget = _player.position - _storageAnt.position;
                dirToTarget.y = 0f;
                dirToTarget.Normalize();
                _storageAnt.transform.rotation = Quaternion.Slerp(_storageAnt.transform.rotation, Quaternion.LookRotation(dirToTarget), 3f * Time.deltaTime);

                break;
            case 3:
                grabbedAphid = _player.GetComponent<FPSInteractionManager>()._grabbedObject != null ? _player.GetComponent<FPSInteractionManager>()._grabbedObject.tag == "Aphid" : false;
                if (grabbedAphid) _exitCollider.isTrigger = true; else _exitCollider.isTrigger = false;
                break;
        }


        grabbedEgg = _player.GetComponent<FPSInteractionManager>()._grabbedObject != null ? true : false;
    }

    private IEnumerator tutorialManager()
    {
        yield return StartCoroutine(FirstPhase());
        yield return StartCoroutine(SecondPhase());
        yield return StartCoroutine(ThirdPhase());
        yield return StartCoroutine(FourthPhase());
    }

    private IEnumerator FirstPhase()
    {
        phase++;
        _player.GetComponent<AntMovement>().enabled = false;
        yield return StartCoroutine(_startingAnt.GetComponent<AntNPC_int>().moveForward(4f)); //move to player
        _player.GetComponent<FPSInteractionManager>().Interaction(GetComponent<Interactable>());
        _startingAnt.GetComponent<DialogueTrigger>().TriggerDialogue();  // trigger dialog      
        _player.GetComponent<AntMovement>().enabled = true;
        _startingAnt.GetComponent<Rigidbody>().isKinematic = true;

        yield return new WaitUntil(() => _startingAnt.GetComponent<DialogueTrigger>().dialogueEnd);
        //show on screen keys info  wasd   space ...wait time... interact with egg/ant
        yield return StartCoroutine(DisplayTextPopupHint("Premi W/A/S/D per muoverti\nUsa SPACE per smettere di aggrapparti\nUsa E per interagire con le sorelle", 6f));
    }

    private IEnumerator SecondPhase()
    {
        phase++;
        yield return StartCoroutine(DisplayTextPopupHint("Usa CLICK DX per afferrare un oggetto con puntatore giallo\nUsa CLICK SX per rialsciarlo", 6f));
        //wait for grabbing object by user
        yield return new WaitUntil(() => grabbedEgg);
        _cursor.position = new Vector3(156f, 220f, -25.7f);
        // move to another room following path
        foreach (Transform pathEl in pathList)
        {
            //parent egg to ant
            yield return StartCoroutine(_eggAnt.GetComponent<AntNPC_int>().rotateTowards(1f, 3f, pathEl.position));
            yield return StartCoroutine(_eggAnt.GetComponent<AntNPC_int>().moveForward(5f));
        }
        _cursor.position = new Vector3(180f, 225f, 24f);
        //wait until boudnaries && grabbingObject null

        yield return new WaitUntil(() => checkBoundaries() && !grabbedEgg);


        //compare canvas "torna a parlare con Sara"      
        StartCoroutine(DisplayTextPopupHint("Ritorna da Sara", 4f));
        _cursor.position = _startingAnt.position + Vector3.up * 2.5f;

        //new dialog
        _startingAnt.GetComponent<DialogueTrigger>().dialogueEnd = false;
        _startingAnt.GetComponent<DialogueTrigger>().dialogue = new Dialogue("Sara", "Ok, bravissima! Adesso recati fino al magazzino che hanno bisogno di te",
                                "Segui i cartelli se non sai dove andare", false, new string[0], new string[0]);
        yield return new WaitUntil(() => _startingAnt.GetComponent<DialogueTrigger>().dialogueEnd);
        _storageAnt.GetComponent<DialogueTrigger>().dialogueEnd = false; //se l'utente ci ha interagito precedentemente falserebbe il filo del tutorial

    }
    private IEnumerator ThirdPhase()
    {
        phase = 2;
        _cursor.position = _storageAnt.position + Vector3.up * 2.5f;
        string[] questions = {"Cosa sono le larve?", "Che tipo di cibo mangiano le larve?", "Come riconosco una larva che diventerà una regina?", "Cosa succede alle larve una volta che crescono?"};
        string[] answers = {"Ogni sorella nasce da un uovo fecondato deposto dalla regina. Poi una volta schiuso fuoriescono le larve, che vengono nutrite dalle sorelle più giovani.", 
                            "Un po' di tutto, basta che sia masticato per bene dalle sorelle",
                            "Le larve destinate a diventare regine o maschi vengono nutrite molto più abbondantemente delle altre così che possano sviluppare anche gli organi sessuali per riprodursi.",
                            "Man mano che cresce e dopo 3 mute, la larva si rinchiude in un bozzolo che si crea girando su se stessa, diventando una pupa. Inizialmente è bianca, ma più va avanti la metamorfosi, più il bozzolo si iscurisce fino a ché non si rompe"};
        _storageAnt.GetComponent<DialogueTrigger>().dialogue = new Dialogue("sorella", "Eccoti. Ci serve aiuto a nutrire le nostre larve.\nPorta del cibo alle larve nella loro stanza.Ricordati di marsticarlo prima eh",
                                "Cosa ti serve?", false, questions, answers);
        yield return new WaitUntil(() => _storageAnt.GetComponent<DialogueTrigger>().dialogueEnd);
        _storageAnt.GetComponent<DialogueTrigger>().dialogueEnd = false;

        yield return StartCoroutine(DisplayTextPopupHint("Premi CLICK SX per masticare il cibo mentre è per terra", 4f));
        yield return StartCoroutine(DisplayTextPopupHint("Mentre il cibo è nello stomaco sociale, premi E sulla larva per nutrirla", 4f));

        _cursor.position = new Vector3(53.4f, 214f, -110f);
        larvaFeeding = false; // se per caso l'utente l'avesse fatto precedentemente

        yield return new WaitUntil(() => larvaFeeding);

        yield return StartCoroutine(DisplayTextPopupHint("Torna a parlare con la sorella del magazzino", 4f));
        _cursor.position = _storageAnt.position + Vector3.up * 2.5f;
        string[] questions1 = { "Come dormono le formiche?" };
        string[] answers1 = { "Fanno brevi pisolini ogni tanto dato che il loro nutrimento è principalmente a base di zuccheri" };
        _storageAnt.GetComponent<DialogueTrigger>().dialogue = new Dialogue("sorella", "Ottimo, congratulazioni. Ora che hai corso tanto riposati un po'",
                                "Qualche dubbio?", false, questions1, answers1);

        _player.GetComponent<FPSInteractionManager>()._interactingObject = null;
        yield return new WaitUntil(() => _storageAnt.GetComponent<DialogueTrigger>().dialogueEnd);

        //blink animation
        _blinkCanvas.SetActive(true);
        _blinkCanvas.GetComponent<Animator>().SetTrigger("restBlink");
        yield return new WaitForSeconds(6f);
        _blinkCanvas.SetActive(false);
    }

    private IEnumerator FourthPhase()
    {
        _cursor.position = _storageAnt.position + Vector3.up * 2.5f;
        _storageAnt.GetComponent<DialogueTrigger>().dialogue = new Dialogue("sorella", "Rieccoti, ora sei pronta per esplorare fuori dal nido.\nPorta gli afidi dalla loro stanza all'esterno, magari su un albero.",
                                "In bocca al ragno!", false, new string[0], new string[0]);
        yield return new WaitUntil(() => _storageAnt.GetComponent<DialogueTrigger>().dialogueEnd);
        _cursor.gameObject.SetActive(false);
        phase = 3;
    }


    private IEnumerator DisplayTextPopupHint(string testo, float displayTime)
    {
        _popUp.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = testo;
        _popUp.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        _popUp.SetActive(false);
    }

    private bool checkBoundaries() =>
         ((140f < _player.position.x && _player.position.x < 210f) &&
           (220f < _player.position.y && _player.position.y < 260f) &&
           (-6f < _player.position.z && _player.position.z < 50f));

    public void larvaInteraction()
    {
        if (_player.GetComponent<FPSInteractionManager>().feeding)
            larvaFeeding = true;
    }
}
