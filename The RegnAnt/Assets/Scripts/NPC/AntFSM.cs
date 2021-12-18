using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntFSM : MonoBehaviour
{
    private FiniteStateMachine<AntFSM> _stateMachine;
    private NavMeshAgent _navMeshAgent;
    [Range(0, 500)] public float walkRadius;
    Vector3 destination;
    private RaycastHit _hitGround;    
    public float offset_y;


    public Transform objectToLoad;
    [SerializeField] private Transform _nest;    
    bool test = false;

    private float timeleft = 2f;


    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();        

        _stateMachine = new FiniteStateMachine<AntFSM>(this);
        objectToLoad = null;

        //STATES
        State wanderState = new WanderState("Wander", this);
        State loadFoodState = new LoadFoodState("loadFood", this);
        State moveFoodToNestState = new MoveFoodToNestState("moveFoodToNest", this);
        State followRailState = new FollowRailState("followRail", this);        
        //

        //TRANSITIONS
        _stateMachine.AddTransition(wanderState, loadFoodState, () => objectToLoad != null);
        _stateMachine.AddTransition(loadFoodState,moveFoodToNestState, () => moveToFood() <= 8f);
        _stateMachine.AddTransition(moveFoodToNestState,wanderState, () => moveToDestination() <= 9f);
        _stateMachine.AddTransition(loadFoodState, followRailState, () => test == true);
        _stateMachine.AddTransition(wanderState, followRailState, () => test == true);
        
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
        if(NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
            finalPosition = hit.position;

        return finalPosition;
    }

    public void LookAround()
    {
        Debug.DrawLine(transform.position + new Vector3(0, offset_y, 0), transform.position + new Vector3(0, offset_y, 0) + 8f * transform.forward);
        if(Physics.SphereCast(transform.position + new Vector3(0, offset_y, 0), 1f, transform.forward, out RaycastHit hit, 8f)) 
        {               
            //if food, layer, pheromones
            if(hit.transform.gameObject.layer == 7) //food layer
            {                
                //move to object state
                objectToLoad = hit.transform;
            }            
        } 
    }

    public float moveToFood()
    {
         _navMeshAgent.SetDestination(objectToLoad.position - _navMeshAgent.stoppingDistance * transform.forward);          
         return (objectToLoad.position - transform.position).magnitude;    //_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance   
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
        if(timeleft <= 0f)
        {
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