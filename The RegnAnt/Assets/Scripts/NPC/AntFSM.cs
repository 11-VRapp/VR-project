using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using DG.Tweening;
public class AntFSM : MonoBehaviour
{
    [Header("NPC values")]
    [Range(0, 500)] public float walkRadius;
    Vector3 destination; private FiniteStateMachine<AntFSM> _stateMachine;
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;

    [Header("Pheromones")]
    public Transform objectToLoad;
    public Transform _objectToLoad_Parent; //cause parent change after hooked
    [SerializeField] private Transform _nest;
    public PheromoneRailTrace pheromoneTrace;
    private LinkedListNode<PheromoneRailPoint> _currentPheromonePoint;
    [SerializeField] private GameObject _pheromonePrefab;
    public Coroutine spawnPheromoneCoroutine;
    public bool returnBack = false;
    [SerializeField] private MultiAimConstraint _headController;
    [SerializeField] private Transform _headController_target;
    private Vector3 _headController_target_startPosition;

    [SerializeField] private Transform _mandibole_hook_position;
    private RaycastHit _hitPoint;

    void Start()
    {
        _headController_target_startPosition = _headController_target.localPosition;

        _navMeshAgent = GetComponent<NavMeshAgent>();

        _stateMachine = new FiniteStateMachine<AntFSM>(this);
        objectToLoad = null;
        _objectToLoad_Parent = null;

        //STATES
        State wanderState = new WanderState("Wander", this);
        State loadFoodState = new LoadFoodState("loadFood", this);

        State followPheromoneTraceState = new FollowPheromoneTraceState("followPheromoneTrace", this);
        State spawnNewPheromoneTraceState = new SpawnNewPheromoneTraceState("spawnNewPheromoneTrace", this);
        State followPheromoneTraceToNestState = new FollowPheromoneTraceToNestState("followPheromoneTraceToNest", this);


        //TRANSITIONS
        _stateMachine.AddTransition(wanderState, loadFoodState, () => objectToLoad != null);

        _stateMachine.AddTransition(wanderState, followPheromoneTraceState, () => pheromoneTrace != null); //sicuro? _currentPheromonePoint == null
        _stateMachine.AddTransition(followPheromoneTraceState, wanderState, () => _currentPheromonePoint == null);
        _stateMachine.AddTransition(followPheromoneTraceState, loadFoodState, () => objectToLoad != null);

        _stateMachine.AddTransition(loadFoodState, spawnNewPheromoneTraceState, () => returnBack && pheromoneTrace == null);
        _stateMachine.AddTransition(loadFoodState, followPheromoneTraceToNestState, () => returnBack && pheromoneTrace != null);

        _stateMachine.AddTransition(spawnNewPheromoneTraceState, wanderState, () => DistanceFromTarget(_nest) <= _navMeshAgent.stoppingDistance);
        _stateMachine.AddTransition(followPheromoneTraceToNestState, wanderState, () => DistanceFromTarget(_nest) <= _navMeshAgent.stoppingDistance);


        //START STATE
        _stateMachine.SetState(wanderState);
    }


    void Update() => _stateMachine.Tik();

    public float DistanceFromTarget(Transform _target) => Vector3.Distance(new Vector3(_target.transform.position.x, 0f, _target.transform.position.z), new Vector3(transform.position.x, 0f, transform.position.z));

    public void SetRandomPointDestination()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 2f)
        {
            destination = RandomNavMeshLocation();
            _navMeshAgent.SetDestination(destination);
        }
    }
    private Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
            finalPosition = hit.position;

        return finalPosition;
    }

    public void LookAround()
    {
        Debug.DrawLine(transform.position, transform.position + 8f * transform.forward);
        if (Physics.SphereCast(transform.position, 1f, transform.forward, out _hitPoint, 8f))
        {
            //if food, layer, pheromones
            if (_hitPoint.transform.gameObject.layer == 7) //food layer
            {
                //move to object state
                //Debug.LogWarning(hit.transform.name); PADREEE
                _objectToLoad_Parent = _hitPoint.transform;
                objectToLoad = _hitPoint.collider.transform; //in questo momento object to load è il padre, devo prendere il figlio
                //Debug.LogWarning(hit.collider.transform.name); //IL FIGLIO      
            }
            else if (_hitPoint.transform.gameObject.layer == 8) //pheromone layer
            {
                Debug.Log("Pheromone: " + _hitPoint.transform.name);
                //move to object state
                pheromoneTrace = _hitPoint.transform.parent.GetComponent<PheromoneRailTrace>();
                _currentPheromonePoint = pheromoneTrace.getNodeByPoint(_hitPoint.transform.GetComponent<PheromoneRailPoint>());

            }
        }
    }
    /*public void moveToFood()
    {
        _navMeshAgent.SetDestination(objectToLoad.position);  //in questo momento object to load è il padre, devo prendere il figlio
        pheromoneTrace = _objectToLoad_Parent.GetComponent<FoodManager>().phTrace; ///problema. Come con spider, con rb se collido mi da automaticamente il padre
        _animator.SetTrigger("loadObject"); 
    }*/


    public IEnumerator moveToFood()
    {    
       _headController_target.DOMove(objectToLoad.position, 1f);
        pheromoneTrace = _objectToLoad_Parent.GetComponent<FoodManager>().phTrace; ///problema. Come con spider, con rb se collido mi da automaticamente il padre       
        yield return StartCoroutine(moveToPhPoint(objectToLoad.position - 1f * transform.forward));           
        yield return new WaitForSeconds(1f);
        _animator.SetTrigger("loadObject");  //grab food Animation
        yield return new WaitForSeconds(1f);

        grabFood();        
    }

    public void grabFood()
    {   /* set food as children */

        //create pivotPoint as hitPoint and parent to foodObject
        GameObject pivot = new GameObject("pivotPoint");
        pivot.transform.position = _hitPoint.point;

        objectToLoad.transform.SetParent(pivot.transform);
        pivot.transform.SetParent(_mandibole_hook_position.transform);
        pivot.transform.localPosition = Vector3.zero;

        _headController_target.DOLocalMove(_headController_target_startPosition, 2f);
        returnBack = true;
    }

    /*public void FoodLoaderHandler()
    {        
        //first go to object
        destination = objectToLoad.position;
        _navMeshAgent.SetDestination(objectToLoad.position);
        pheromoneTrace = objectToLoad.GetComponent<FoodManager>().phTrace;

        //then grab (+Animation)
        StartCoroutine()

        //if parent has phTrace null -> return to nest spawning new Ph Trace

        //if parent has phTrace != null -> just follow that PhTrace
    }*/

    public void newPheromoneTraceHandler()
    {
        destination = _nest.position;
        _navMeshAgent.SetDestination(_nest.position);

        if (pheromoneTrace == null)
        {
            //Spawn new Trace
            GameObject parent = new GameObject("RailTrace");  //only setUp parent                        
            parent.AddComponent<PheromoneRailTrace>();
            pheromoneTrace = parent.GetComponent<PheromoneRailTrace>();
            //pheromoneTrace.foodRelativeTo = objectToLoad;
            _objectToLoad_Parent.GetComponent<FoodManager>().phTrace = pheromoneTrace;
        }

        spawnPheromoneCoroutine = StartCoroutine(newPheromoneSpawner());
    }
    private IEnumerator newPheromoneSpawner()
    {
        yield return new WaitUntil(() => pheromoneTrace != null);
        yield return new WaitForSeconds(2f); //sicuro? per spawnare il primo non subito attaccato, magari diminuire a 1 sec
        while (DistanceFromTarget(_nest) > _navMeshAgent.stoppingDistance)
        {
            Physics.Raycast(transform.position, -transform.up, out RaycastHit groundHit); //on ground Layer!
            GameObject newPoint = GameObject.Instantiate(_pheromonePrefab, groundHit.point, Quaternion.identity, pheromoneTrace.gameObject.transform); //spawn on the terrain...
            PheromoneRailPoint pheromone_point = newPoint.AddComponent<PheromoneRailPoint>();
            pheromoneTrace.pushPointToTrace(pheromone_point);

            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator moveToPhPoint(Vector3 point)
    {
        do{   
            _headController_target.DOMove(point + transform.forward * 1f, 2f);        
            _navMeshAgent.SetDestination(point);
            yield return null;
        }while(_navMeshAgent.remainingDistance > 1f);
    }

    public IEnumerator followPheromoneTraceToFood()
    {       
        for (; _currentPheromonePoint != null; _currentPheromonePoint = pheromoneTrace.getPrevPoint(_currentPheromonePoint))    //exit if life is < 0  
            yield return StartCoroutine(moveToPhPoint(_currentPheromonePoint.Value.transform.position));   
    }

    public IEnumerator followPheromoneTraceToNest()
    {  
        for (_currentPheromonePoint = pheromoneTrace.getHeadNode(); _currentPheromonePoint != null; _currentPheromonePoint = pheromoneTrace.getNextPoint(_currentPheromonePoint))    
        {
            yield return StartCoroutine(moveToPhPoint(_currentPheromonePoint.Value.transform.position)); 
            _currentPheromonePoint.Value.GetComponent<PheromoneRailPoint>().addLife();
        }   
             
        _navMeshAgent.SetDestination(new Vector3(_nest.position.x, transform.position.y, _nest.position.z)); 
        _headController_target.DOLocalMove(_headController_target_startPosition, 2f);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the destination point
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(destination, 0.5f);

        Gizmos.color = Color.red;
        if (_currentPheromonePoint != null)
            Gizmos.DrawWireSphere(_currentPheromonePoint.Value.transform.position, 0.5f);

    }
}