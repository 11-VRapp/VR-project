using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntFSM : MonoBehaviour
{
    [Header("NPC values")]
    [Range(0, 500)] public float walkRadius;
    Vector3 destination; private FiniteStateMachine<AntFSM> _stateMachine;
    private NavMeshAgent _navMeshAgent;
    public float offset_y;

    [Header("Pheromones")]
    public Transform objectToLoad;
    [SerializeField] private Transform _nest;
    public PheromoneRailTrace pheromoneTrace;
    private LinkedListNode<PheromoneRailPoint> _currentPheromonePoint;    
    private bool _lastPoint = false;
    [SerializeField] private GameObject _pheromonePrefab;    
    private float timeleft = 2f;


    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _stateMachine = new FiniteStateMachine<AntFSM>(this);
        objectToLoad = null;

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

        _stateMachine.AddTransition(loadFoodState, spawnNewPheromoneTraceState, () => DistanceFromTarget(objectToLoad.gameObject) <= 4f && pheromoneTrace == null);
        _stateMachine.AddTransition(loadFoodState, followPheromoneTraceToNestState, () => DistanceFromTarget(objectToLoad.gameObject) <= 4f && pheromoneTrace != null);

        _stateMachine.AddTransition(spawnNewPheromoneTraceState, wanderState, () => DistanceFromTarget(_nest.gameObject) <= 1f);
        _stateMachine.AddTransition(followPheromoneTraceToNestState, wanderState, () => DistanceFromTarget(_nest.gameObject) <= 1f);



        //START STATE
        _stateMachine.SetState(wanderState);
    }


    void Update() => _stateMachine.Tik();

    private float DistanceFromTarget(GameObject _target) => Vector3.Distance(new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z), transform.position);

    public void SetRandomPointDestination()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 2f)
        {
            //wait for seconds?
            //yield return WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
            destination = RandomNavMeshLocation();
            _navMeshAgent.SetDestination(destination);
        }
    }

    public Vector3 RandomNavMeshLocation()
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
        Debug.DrawLine(transform.position + new Vector3(0, offset_y, 0), transform.position + new Vector3(0, offset_y, 0) + 8f * transform.forward);
        if (Physics.SphereCast(transform.position + new Vector3(0, offset_y, 0), 1f, transform.forward, out RaycastHit hit, 8f))
        {
            //if food, layer, pheromones
            if (hit.transform.gameObject.layer == 7) //food layer
            {
                //move to object state
                objectToLoad = hit.transform;
            }
            else if (hit.transform.gameObject.layer == 8) //pheromone layer
            {
                //move to object state
                pheromoneTrace = hit.transform.GetComponent<PheromoneRailPoint>().trace;
                _currentPheromonePoint = pheromoneTrace.getNodeByPoint(hit.transform.GetComponent<PheromoneRailPoint>());
            }
        }
    }

    public void moveToFood()
    {
        _navMeshAgent.SetDestination(objectToLoad.position);
        pheromoneTrace = objectToLoad.GetComponent<FoodManager>().phTrace;        
    }

    public float spawnNewPheromoneTrace()
    {
        if (pheromoneTrace == null)
        {
            //Spawn new Trace
            GameObject parent = new GameObject("RailTrace");  //only setUp parent                        
            parent.AddComponent<PheromoneRailTrace>();
            pheromoneTrace = parent.GetComponent<PheromoneRailTrace>();
            //pheromoneTrace.foodRelativeTo = objectToLoad;
            objectToLoad.GetComponent<FoodManager>().phTrace = pheromoneTrace;
        }

        _navMeshAgent.SetDestination(new Vector3(_nest.position.x, transform.position.y, _nest.position.z));
        timeleft -= Time.deltaTime;
        if (timeleft <= 0f)
        {
            //Spawn new ph point into trace
            Physics.Raycast(transform.position, -transform.up, out RaycastHit groundHit); //on ground Layer!
            GameObject newPoint = GameObject.Instantiate(_pheromonePrefab, groundHit.point, Quaternion.identity, pheromoneTrace.gameObject.transform); //spawn on the terrain...
            PheromoneRailPoint pheromone_point = newPoint.AddComponent<PheromoneRailPoint>();
            pheromoneTrace.pushPointToTrace(pheromone_point); 

            timeleft += 2f;
        }

        return (_nest.position - transform.position).magnitude;
    }

    public void followPheromoneTraceToNestState()
    {
        if (_navMeshAgent.remainingDistance <= 1f && !_lastPoint)
        {
            Vector3 nextWayPointPos;
            LinkedListNode<PheromoneRailPoint> prev = null;
            if (_currentPheromonePoint == pheromoneTrace.getTailNode())
            {
                Debug.Log("Tail");
                nextWayPointPos = new Vector3(_nest.position.x, transform.position.y, _nest.position.z);
                _lastPoint = true;
            }
            else
            {
                if (_currentPheromonePoint == null)
                    _currentPheromonePoint = pheromoneTrace.getHeadNode();
                else
                {
                    prev = _currentPheromonePoint;
                    _currentPheromonePoint = pheromoneTrace.getNextPoint(_currentPheromonePoint);
                }                  

                nextWayPointPos = _currentPheromonePoint.Value.gameObject.transform.position;
            }

            if(prev != null || prev == pheromoneTrace.getTailNode())
                prev.Value.GetComponent<PheromoneRailPoint>().addLife();
            _navMeshAgent.SetDestination(nextWayPointPos);
        }
    }

    public void followPheromoneTrace()
    {
        if (_currentPheromonePoint == null) //necessario? o il next è nullo?
            return;
        Debug.Log(_currentPheromonePoint.Value.transform.localPosition);

        if (_navMeshAgent.remainingDistance <= 2f)
        {
            Vector3 nextWayPointPos;            

            _currentPheromonePoint = pheromoneTrace.getPrevPoint(_currentPheromonePoint);
            if(_currentPheromonePoint == null)
                return;
            nextWayPointPos = _currentPheromonePoint.Value.gameObject.transform.position;

            _navMeshAgent.SetDestination(nextWayPointPos);
        }
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the destination point
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(destination, 0.5f);
    }
}