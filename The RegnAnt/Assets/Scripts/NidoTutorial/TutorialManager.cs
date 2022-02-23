using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Transform _eggAnt;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _cursor;
    [SerializeField] private GameObject _popUp;
    [SerializeField] private GameObject _blinkCanvas;
    [SerializeField] private AudioManager _generalAudioManager;
    [SerializeField] private VoiceOverManager _voiceOverAudioManager;
    [SerializeField] private List<roomTrigger> _roomTriggers;
    private bool queenSpoken;

    void Start()
    {
        queenSpoken = false;
        _popUp.SetActive(false);
        _blinkCanvas.SetActive(true);
        StartCoroutine(tutorialManager());
        _generalAudioManager.Play("BkAudio");
    }

    private IEnumerator tutorialManager()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(Act1());
        yield return StartCoroutine(Act2());
        yield return StartCoroutine(Act3());
        yield return StartCoroutine(Act4());
        yield return StartCoroutine(Act5());
        yield return StartCoroutine(Act6());
        yield return StartCoroutine(Act7());
    }

    private IEnumerator Act1()
    {
        StartCoroutine(DisplayTextPopupHint("Premi W/A/S/D per muoverti\nUsa SPACE per smettere di aggrapparti\nUsa E per interagire con le sorelle", 6f));

        StartCoroutine(_voiceOverAudioManager.atti[0].playAudioSequentially());

        yield return new WaitForSeconds(6f);
        StartCoroutine(DisplayTextPopupHint("Raggiungi la stanza delle uova seguendo le indicazioni", 6f));

        yield return new WaitUntil(() => _player.GetComponent<FPSInteractionManager>()._grabbedObject != null ? true : false);
        _cursor.position = new Vector3(156f, 220f, -25.7f);

        yield return StartCoroutine(_eggAnt.GetComponent<AntNPC_int>().rotateTowards(1f, 3f, new Vector3(156f, 220f, -25.7f)));
        StartCoroutine(_eggAnt.GetComponent<AntNPC_int>().moveForward(15f));

        _cursor.position = new Vector3(180f, 225f, 24f);
        //wait until boudnaries && grabbingObject null

        yield return new WaitUntil(() => _roomTriggers[0].insideTrigger && _player.GetComponent<FPSInteractionManager>()._grabbedObject == null ? true : false); //wait for grabbing object by user
        _cursor.gameObject.SetActive(false);
        _generalAudioManager.Play("QuestCompleted");
        yield return new WaitForSeconds(2f); //audio complete 
    }

    private IEnumerator Act2()
    {
        yield return new WaitUntil(() => !_voiceOverAudioManager.voiceOverBusy); //per evitare sovrapposizioni di voci
        StartCoroutine(_voiceOverAudioManager.atti[2].playAudioSequentially());

        StartCoroutine(DisplayTextPopupHint("Raggiungi il magazzino seguendo le indicazioni", 4f));

        yield return new WaitUntil(() => _roomTriggers[1].insideTrigger); //raggiunto magazzino

    }

    private IEnumerator Act3()
    {
        yield return new WaitUntil(() => !_voiceOverAudioManager.voiceOverBusy); //per evitare sovrapposizioni di voci
        StartCoroutine(_voiceOverAudioManager.atti[3].playAudioSequentially());

        StartCoroutine(DisplayTextPopupHint("Riempiti lo stomaco mangiando il cibo tramite CLICK SINISTRO", 4f));

        _player.GetComponent<FPSInteractionManager>().feeded = false;
        yield return new WaitUntil(() => _player.GetComponent<FPSInteractionManager>().feeded); //mangiato del cibo

    }
    private IEnumerator Act4()
    {
        yield return new WaitUntil(() => !_voiceOverAudioManager.voiceOverBusy); //per evitare sovrapposizioni di voci
        StartCoroutine(_voiceOverAudioManager.atti[4].playAudioSequentially());

        StartCoroutine(DisplayTextPopupHint("Raggiungi la stanza delle larve seguendo le indicazioni", 4f));

        yield return new WaitUntil(() => _roomTriggers[2].insideTrigger); //raggiunta stanza larva e pupe

        StartCoroutine(DisplayTextPopupHint("Mentre il cibo è nello stomaco sociale, premi E sulla larva per nutrirla", 4f));

        yield return new WaitUntil(() => !_player.GetComponent<FPSInteractionManager>().feeded); //larva nutrita

        _generalAudioManager.Play("QuestCompleted");
    }

    private IEnumerator Act5()
    {
        _blinkCanvas.SetActive(true);
        _blinkCanvas.GetComponent<Animator>().SetTrigger("restBlink");
        yield return new WaitForSeconds(6f);
        _blinkCanvas.SetActive(false);

        yield return new WaitUntil(() => !_voiceOverAudioManager.voiceOverBusy); //per evitare sovrapposizioni di voci
        yield return StartCoroutine(_voiceOverAudioManager.atti[5].playAudioSequentially()); //Pisolino fatto
    }

    private IEnumerator Act6()
    {
        yield return new WaitUntil(() => !_voiceOverAudioManager.voiceOverBusy); //per evitare sovrapposizioni di voci
        StartCoroutine(_voiceOverAudioManager.atti[6].playAudioSequentially());

        StartCoroutine(DisplayTextPopupHint("Raggiungi la stanza degli afidi seguendo le indicazioni", 4f));

        yield return new WaitUntil(() => _roomTriggers[3].insideTrigger); //raggiunta stanza afidi
    }

    private IEnumerator Act7()
    {
        yield return new WaitUntil(() => !_voiceOverAudioManager.voiceOverBusy); //per evitare sovrapposizioni di voci
        StartCoroutine(_voiceOverAudioManager.atti[7].playAudioSequentially());

        StartCoroutine(DisplayTextPopupHint("Raggiungi l'uscita del nido con l'afido", 4f));
        yield return new WaitUntil(() => _roomTriggers[4].insideTrigger && _player.GetComponent<FPSInteractionManager>()._grabbedObject != null ? _player.GetComponent<FPSInteractionManager>()._grabbedObject.tag == "Aphid" : false);

        SceneManager.LoadScene("mondoEsterno");
    }


    private IEnumerator DisplayTextPopupHint(string testo, float displayTime)
    {
        //yield return new WaitForSeconds(waitTimeBeforeStart);
        _popUp.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = testo;
        _popUp.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        _popUp.SetActive(false);
    }

    public IEnumerator triggerQueenDialogue()
    {
        if (!queenSpoken)
        {
            if (!_voiceOverAudioManager.voiceOverBusy) //QUEEN DIALOGUE
            {
                StartCoroutine(_voiceOverAudioManager.atti[1].playAudioSequentially());

                queenSpoken = true;
            }
        }
        yield return null;
    }

}

/*
    private IEnumerator ThirdPhase()
    {
        phase = 2;
        _cursor.position = _storageAnt.position + Vector3.up * 2.5f;
        string[] questions = { "Cosa sono le larve?", "Che tipo di cibo mangiano le larve?", "Come riconosco una larva che diventerà una regina?", "Cosa succede alle larve una volta che crescono?" };
        string[] answers = {"Ogni sorella nasce da un uovo fecondato deposto dalla regina. Poi una volta schiuso fuoriescono le larve, che vengono nutrite dalle sorelle più giovani.",
                            "Un po' di tutto, basta che sia masticato per bene dalle sorelle",
                            "Le larve destinate a diventare regine o maschi vengono nutrite molto più abbondantemente delle altre così che possano sviluppare anche gli organi sessuali per riprodursi.",
                            "Man mano che cresce e dopo 3 mute, la larva si rinchiude in un bozzolo che si crea girando su se stessa, diventando una pupa. Inizialmente è bianca, ma più va avanti la metamorfosi, più il bozzolo si iscurisce fino a ché non si rompe"};
        _storageAnt.GetComponent<DialogueTrigger>().dialogue = new Dialogue("sorella", "Eccoti. Ci serve aiuto a nutrire le nostre larve.\nPorta del cibo alle larve nella loro stanza.\nRicordati di marsticarlo prima eh",
                                "Cosa ti serve?", false, questions, answers);
        yield return new WaitUntil(() => _storageAnt.GetComponent<DialogueTrigger>().dialogueEnd);
        _storageAnt.GetComponent<DialogueTrigger>().dialogueEnd = false;

        yield return StartCoroutine(DisplayTextPopupHint("Premi CLICK SX per masticare il cibo mentre è per terra", 4f));
        yield return StartCoroutine(DisplayTextPopupHint("Mentre il cibo è nello stomaco sociale, premi E sulla larva per nutrirla", 4f));

        _cursor.position = new Vector3(53.4f, 214f, -110f);
        _player.GetComponent<FPSInteractionManager>().feeded = false; // se per caso l'utente l'avesse fatto precedentemente

        yield return new WaitUntil(() => _player.GetComponent<FPSInteractionManager>().feeded);
        _generalAudioManager.Play("QuestCompleted");

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


    */

