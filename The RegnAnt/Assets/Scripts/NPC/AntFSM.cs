using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntFSM : MonoBehaviour
{
    [Header("NPC values")]
    [Range(0, 500)] public float walkRadius;
    Vector3 destination;    private FiniteStateMachine<AntFSM> _stateMachine;
    private NavMeshAgent _navMeshAgent; 
    public float offset_y;

    [Header("Pheromones")]
    public Transform objectToLoad;
    [SerializeField] private Transform _nest;
    [SerializeField] private PheromoneRailTrace pheromoneTrace;    
    [SerializeField] private GameObject _pheromonePrefab;
    bool test = false;
    private float timeleft = -1f;


    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _stateMachine = new FiniteStateMachine<AntFSM>(this);
        objectToLoad = null;

        //STATES
        State wanderState = new WanderState("Wander", this);
        State loadFoodState = new LoadFoodState("loadFood", this);
        //State moveFoodToNestState = new MoveFoodToNestState("moveFoodToNest", this);
        State followPheromoneTraceState = new FollowPheromoneTraceState("followPheromoneTrace", this);
        State spawnNewPheromoneTraceState = new SpawnNewPheromoneTraceState("spawnNewPheromoneTrace", this);
        State followPheromoneTraceToNestState = new FollowPheromoneTraceToNestState("followPheromoneTraceToNest", this);
        //

        //TRANSITIONS
        _stateMachine.AddTransition(wanderState, loadFoodState, () => objectToLoad != null);

        _stateMachine.AddTransition(wanderState, followPheromoneTraceState, () => pheromoneTrace != null); //sicuro?
        _stateMachine.AddTransition(followPheromoneTraceState, loadFoodState, () => objectToLoad != null);

        _stateMachine.AddTransition(loadFoodState, spawnNewPheromoneTraceState, () => moveToFood() && pheromoneTrace == null);
        _stateMachine.AddTransition(loadFoodState, followPheromoneTraceToNestState, () => moveToFood() && pheromoneTrace != null);

        //_stateMachine.AddTransition(moveFoodToNestState,wanderState, () => moveToDestination() <= 9f);

        _stateMachine.AddTransition(spawnNewPheromoneTraceState, wanderState, () => test == true);
        _stateMachine.AddTransition(followPheromoneTraceToNestState, wanderState, () => test == true);

        //_stateMachine.AddTransition(stopState,chaseState, () => DistanceFromTarget() > _stoppingDistance);

        //START STATE
        _stateMachine.SetState(wanderState);
    }

    // Update is called once per frame
    void Update() => _stateMachine.Tik();

    /*public void SetWayPointDestination()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _navMeshAgent.velocity.sqrMagnitude <= 0f)
        {
            _currentWayPointIndex = (_currentWayPointIndex + 1) % _waypoints.Count;
            Vector3 nextWayPointPos = _waypoints[_currentWayPointIndex].position;
            _navMeshAgent.SetDestination(new Vector3(nextWayPointPos.x, transform.position.y, nextWayPointPos.z));
        }
    }*/

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
        }
    }

    public bool moveToFood()
    {
        _navMeshAgent.SetDestination(objectToLoad.position - _navMeshAgent.stoppingDistance * transform.forward);
        pheromoneTrace = objectToLoad.GetComponent<FoodManager>().phTrace;
        return (objectToLoad.position - transform.position).magnitude <= 8f;
    }

    public float moveToDestination()
    {
        _navMeshAgent.SetDestination(_nest.position - _navMeshAgent.stoppingDistance * transform.forward);

        if (objectToLoad.GetComponent<FoodManager>().phTrace == null)
        {
            //set up a new phTrace

        }
        else
        {
            //follow trace
        }
        timeleft -= Time.deltaTime;
        if (timeleft <= 0f)
        {
            timeleft += 2f;
        }

        return (_nest.position - transform.position).magnitude;
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

        _navMeshAgent.SetDestination(_nest.position - _navMeshAgent.stoppingDistance * transform.forward);
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

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the destination point
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(destination, 0.5f);
    }
}