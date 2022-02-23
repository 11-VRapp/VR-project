using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class antNPCfight : MonoBehaviour
{
    private NavMeshAgent _navAgent;
    [SerializeField] private Animator _animator;
    public List<Transform> spiderLegsEnd = new List<Transform>();
    [SerializeField] private Transform _target;
    public float life = 100f;
    void Start()
    {        
        _navAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(randomChangeTarget());        
    }

    private IEnumerator randomChangeTarget()
    {
        while (life > 0)
        {
            _target = spiderLegsEnd[Random.Range(0, spiderLegsEnd.Count - 1)];
            _navAgent.SetDestination(_target.position);
            yield return new WaitForSeconds(10f);
        }
    }

    public void setDamage(float dmg)
    {
        life -= dmg;
        if (life <= 0)
        {
            _navAgent.enabled = false;
            StopAllCoroutines();
            _animator.SetTrigger("death");   
            //death sound
            GetComponent<AudioManager>().Play("Death");       
            FindObjectOfType<antSpawner>().spawnAnt();  
            //disable all components
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Antennas>().enabled = false;
            GetComponent<antWalkingSound>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            this.enabled = false;            
        }
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, _target.position) < 1f) 
        {
            _animator.SetTrigger("attack");
            GetComponent<AudioManager>().PlayWithRandomPitch("Attack");
        }       
            
        
    }
}
