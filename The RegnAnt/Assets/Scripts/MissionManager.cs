using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _aphid;
    [SerializeField] private Transform _arrow;
    [SerializeField] private Transform _aphidSetPosition;
    
    [SerializeField] private Transform _antNest;
    [SerializeField] private Transform _antToFollow;
    [SerializeField] private GameObject _mosquito;
    [SerializeField] private GameObject _railTraceToNest;
    [SerializeField] private GameObject _railTraceToEnemy;
    [SerializeField] private Transform _fightPosition;
    public string fightScene;
    [SerializeField] private GameObject _popUp;

    [Header("Audio")]
    [SerializeField] private AudioManager _generalAudioManager;
    [SerializeField] private VoiceOverManager _voiceOverAudioManager;
    void Start()
    {
        _player.position = _spawnPosition.position;
        _player.forward = _spawnPosition.forward;

        StartCoroutine(tutorialManager());
    }

    private IEnumerator tutorialManager()
    {
        yield return StartCoroutine(Act1());
        yield return StartCoroutine(Act2());
        yield return StartCoroutine(Act3());
        yield return StartCoroutine(Act4());
        yield return StartCoroutine(Act5());
        yield return StartCoroutine(Act6());
    }

    private IEnumerator Act1()
    {
        yield return new WaitUntil(() => !_voiceOverAudioManager.voiceOverBusy); //per evitare sovrapposizioni di voci
        StartCoroutine(_voiceOverAudioManager.atti[0].playAudioSequentially());

        StartCoroutine(DisplayTextPopupHint("Scala l’albero per posarci l’afide", 4f));

        yield return new WaitUntil(() => Vector3.Distance(_aphid.position, _aphidSetPosition.position) <= 2f); //reached target point with aphid
        _generalAudioManager.Play("QuestCompleted");

        //detach aphid
        _player.GetComponent<FPSInteractionManager>().Drop();
        _aphid.position = _aphidSetPosition.position;
        _aphid.up = _aphidSetPosition.up;

        _aphid.GetComponent<Rigidbody>().useGravity = false;
        _aphid.GetComponent<Rigidbody>().isKinematic = true;
    }

    private IEnumerator Act2()
    {
        yield return new WaitUntil(() => !_voiceOverAudioManager.voiceOverBusy); //per evitare sovrapposizioni di voci
        StartCoroutine(_voiceOverAudioManager.atti[1].playAudioSequentially());

        StartCoroutine(DisplayTextPopupHint("Scendi dall’albero", 4f));

        _antToFollow.gameObject.SetActive(true);
        _arrow.transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);
        StartCoroutine(arrowScaleByDistance(_player, _antToFollow));


        yield return new WaitUntil(() => Vector3.Distance(_player.position, _antToFollow.position) <= 2f);
    }


    private IEnumerator Act3()
    {
        yield return new WaitUntil(() => !_voiceOverAudioManager.voiceOverBusy); //per evitare sovrapposizioni di voci
        StartCoroutine(_voiceOverAudioManager.atti[2].playAudioSequentially());

        StartCoroutine(DisplayTextPopupHint("Segui la sorella", 4f));

        _antToFollow.gameObject.SetActive(true);
        _arrow.transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);
        StartCoroutine(arrowScaleByDistance(_player, _antToFollow));

        yield return new WaitUntil(() => Vector3.Distance(_player.position, _antToFollow.position) <= 2f);

        _mosquito.SetActive(true);
        GameObject trace = GameObject.Instantiate(_railTraceToNest, Vector3.zero, Quaternion.Euler(0f, 0f, 0f)); //spawn already generated trace
        //_mosquito.GetComponent<FoodManager>().phTrace = trace.GetComponent<PheromoneRailTrace>(); correct but not necessary if in tutorial other ants do not follow trail

        StartCoroutine(arrowScaleByDistance(_player, _antToFollow));
        _antToFollow.GetComponent<NavMeshAgent>().SetDestination(new Vector3(520.223389f, 0.514628887f, -359.88269f));  //move to ph point
        yield return new WaitUntil(() => _antToFollow.GetComponent<NavMeshAgent>().remainingDistance < 0.1f);
    }

    private IEnumerator Act4()
    {
        yield return new WaitUntil(() => !_voiceOverAudioManager.voiceOverBusy); //per evitare sovrapposizioni di voci
        StartCoroutine(_voiceOverAudioManager.atti[3].playAudioSequentially());

        StartCoroutine(DisplayTextPopupHint("Segui i feromoni verso il cibo", 4f));

        StartCoroutine(arrowScaleByDistance(_player, _mosquito.transform));

        yield return new WaitUntil(() => Vector3.Distance(_player.position, _mosquito.transform.position) < 5f);

        _generalAudioManager.Play("QuestCompleted");
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator Act5()
    {
        yield return new WaitUntil(() => !_voiceOverAudioManager.voiceOverBusy); //per evitare sovrapposizioni di voci
        StartCoroutine(_voiceOverAudioManager.atti[4].playAudioSequentially());

        StartCoroutine(DisplayTextPopupHint("Segui i feromoni verso il nido", 4f));

        StartCoroutine(arrowScaleByDistance(_player, _antNest));

        yield return new WaitUntil(() => Vector3.Distance(_player.position, _antNest.position) < 3f);

        _arrow.gameObject.SetActive(false);
        _generalAudioManager.Play("QuestCompleted");

        //spawn blue pheromone trail to boss point
        GameObject.Instantiate(_railTraceToEnemy, new Vector3(380.253784f, 0.850000024f, -64.1017075f), Quaternion.Euler(0f, 0f, 0f)); //spawn already generated trace    
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator Act6() //RAGGIUNTO IL NIDO
    {
        yield return new WaitUntil(() => !_voiceOverAudioManager.voiceOverBusy); //per evitare sovrapposizioni di voci
        StartCoroutine(_voiceOverAudioManager.atti[5].playAudioSequentially());

        StartCoroutine(DisplayTextPopupHint("Segui i feromoni verso il nemico", 4f));        

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

    private IEnumerator DisplayTextPopupHint(string testo, float displayTime)
    {
        //yield return new WaitForSeconds(waitTimeBeforeStart);
        _popUp.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = testo;
        _popUp.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        _popUp.SetActive(false);
    }
}
