using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _aphid;
    [SerializeField] private Transform _arrow;
    [SerializeField] private Transform _aphidSetPosition;

    [Header("Quest 2")]
    [SerializeField] private Transform _antNest;
    [SerializeField] private Transform _antToFollow;
    [SerializeField] private GameObject _mosquito;
    [SerializeField] private GameObject _railTraceToNest;
    [SerializeField] private GameObject _railTraceToEnemy;
    [SerializeField] private Transform _fightPosition;
    public string fightScene;
    void Start()
    {
        _player.position = _spawnPosition.position;
        _player.forward = _spawnPosition.forward;

        StartCoroutine(tutorialManager());
    }

    private IEnumerator tutorialManager()
    {
        yield return StartCoroutine(FirstPhase());
        yield return StartCoroutine(SecondPhase());
        //yield return StartCoroutine(ThirdPhase());
        //yield return StartCoroutine(FourthPhase());
    }

    private IEnumerator FirstPhase()
    {
        yield return new WaitUntil(() => _player.GetComponent<FPSInteractionManager>()._grabbedObject != null); //signal target point with arrow
        _arrow.position = _aphidSetPosition.position - 2.5f * _aphidSetPosition.up;
        _arrow.transform.Rotate(0.0f, 0.0f, 90.0f, Space.World);
        StartCoroutine(arrowScaleByDistance(_aphid, _aphidSetPosition));


        yield return new WaitUntil(() => Vector3.Distance(_aphid.position, _aphidSetPosition.position) <= 2f); //reached target point with aphid

        //detach aphid
        _player.GetComponent<FPSInteractionManager>().Drop();
        _aphid.position = _aphidSetPosition.position;
        _aphid.up = _aphidSetPosition.up;
             
        _aphid.GetComponent<Rigidbody>().useGravity = false;
        _aphid.GetComponent<Rigidbody>().isKinematic = true;
        //phase 1 complete, now return from tree and encounter (spawn new ant)
    }


    private IEnumerator SecondPhase()
    {
        _antToFollow.gameObject.SetActive(true);
        //_arrow.position = _antToFollow.position + 2.5f * _antToFollow.up;
        _arrow.transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);
        StartCoroutine(arrowScaleByDistance(_player, _antToFollow));
        yield return new WaitUntil(() => _antToFollow.GetComponent<DialogueTrigger>().dialogueEnd); //chatted       

        //follow ant
        //spawn pheromone trail
        _mosquito.SetActive(true);
        GameObject trace = GameObject.Instantiate(_railTraceToNest, Vector3.zero, Quaternion.Euler(0f, 0f, 0f)); //spawn already generated trace
        //_mosquito.GetComponent<FoodManager>().phTrace = trace.GetComponent<PheromoneRailTrace>(); correct but not necessary if in tutorial other ants do not follow trail

        StartCoroutine(arrowScaleByDistance(_player, _antToFollow));
        _antToFollow.GetComponent<NavMeshAgent>().SetDestination(new Vector3(520.223389f, 0.514628887f, -359.88269f));  //move to ph point
        yield return new WaitUntil(() => _antToFollow.GetComponent<NavMeshAgent>().remainingDistance < 0.1f);

        _antToFollow.GetComponent<DialogueTrigger>().dialogueEnd = false; //reset dialogue for new dialogue

        string[] questions = { "Cos'è lo stomaco sociale?" };
        string[] answers = { "Delle volte le formiche hanno bisogno di essere motivate a far qualcosa, o non ne hanno voglia. Quando una formica trova qualcosa di buono, lo mangia e lo conserva in un particolare stomaco detto stomaco sociale. Quando incontra una sorella ne rigurgita parte nella sua bocca per convincerla a seguirla" };
        _antToFollow.GetComponent<DialogueTrigger>().dialogue = new Dialogue("sorella", "Adesso muoviti! Raggiungi il cibo prima che la traccia svanisca!\n\n Poi TORNA AL NIDO",
                                "Segui la traccia che va verso il cibo", false, questions, answers);

        yield return new WaitUntil(() => _antToFollow.GetComponent<DialogueTrigger>().dialogueEnd); //chatted once arrived

        StartCoroutine(arrowScaleByDistance(_player, _mosquito.transform));


        yield return new WaitUntil(() => Vector3.Distance(_player.position, _mosquito.transform.position) < 5f);
        StartCoroutine(arrowScaleByDistance(_player, _antNest));        

        //change dialog to ant on nest
        _antNest.GetComponent<DialogueTrigger>().dialogueEnd = false; //reset dialogue for new dialogue
        string[] questionsNest = { "Abbiamo il sangue?", "Siamo aggressive?", "Come uccidiamo?", "Possiamo percepire la morte?" };
        string[] answersNest = { "No, abbiamo una cosa simile: l'emolinfa. A differenza del sangue, l'emolinfa è di colore giallognolo/verdastro e non trasporta l'ossigeno ma solo i nutrienti",
                                 "Noi Lasius Niger non siamo una specie aggressiva, ma siamo molto territoriali.\nInoltre come avrai già notato la nostra specie non ha la casta delle formiche soldato o guerriere, ma siamo noi operaie a combattere. Le formiche soldato sono più efficienti in battaglia perché hanno subito trasformazioni utili alla guerra, come dimensioni maggiori e mandibole più forti, ma sono inutili a svolgere i nostri compiti",
                                 "Noi Lasius Niger non produciamo l'acido formico, quindi dobbiamo arrangiarci in altri modi.\nSolitamente mordiamo la vittima fino a ucciderla, e l'unione fa la forza. Tendenzialmente ci attacchiamo alle zampe del nemico così da smembrarlo e immobilizzarlo",
                                 "No, o almeno non subito. Noi formiche riconosciamo le altre tramite l'odore, infatti esistono parassiti che lo emulano e ci ingannano. Perciò anche se morta a noi sembra solo ferma, ma dopo un po' di giorni la percepiamo come spazzatura a causa della decomposizione e la trasportiamo dove portiamo la spazzatura" };
        _antNest.GetComponent<DialogueTrigger>().dialogue = new Dialogue("sorella", "Sembra che ci sia una battaglia in atto!\nMolla tutto e segui i feromoni dove portano, io recluto altre sorelle!",
                                "VINCETE PER LA COLONIA!", false, questionsNest, answersNest);

        //wait until distance from nest < ... than force drop object and dialog with antOnNest
        yield return new WaitUntil(() => _antNest.GetComponent<DialogueTrigger>().dialogueEnd); //chatted once arrived
        _arrow.gameObject.SetActive(false);
        //spawn blue pheromone trail to boss point
        GameObject.Instantiate(_railTraceToEnemy, new Vector3(380.253784f,0.850000024f,-64.1017075f), Quaternion.Euler(0f, 0f, 0f)); //spawn already generated trace    

        //? spawn spider     

        yield return new WaitUntil(() => Vector3.Distance(_player.position, _fightPosition.position) <= 30f);  //when arrived change SCENE!
        //change scene to fight!
        SceneManager.LoadScene(fightScene);

    }


    private IEnumerator arrowScaleByDistance(Transform pos1, Transform pos2)
    {
        float distance = Vector3.Distance(pos1.position, pos2.position);

        while (distance > 1f)
        {
            _arrow.position = pos2.position + 2.5f * pos2.up;
            //Debug.Log(distance);
            _arrow.localScale = Vector3.one * distance * 0.1f;

            distance = Vector3.Distance(pos1.position, pos2.position);
            yield return null;
        }

    }
}
