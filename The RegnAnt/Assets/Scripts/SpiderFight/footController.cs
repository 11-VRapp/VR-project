using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class footController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _antMask;

    private float _smoothing = 30f;
    [SerializeField] private float currentDist;

    public Transform CheckEnemyToAttack()
    {       
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 7f, -Vector3.up, 10f, _antMask);

        if(hits.Count() == 0)
        {
            //print(transform.name +" No object colliding");
            return null;
        }            

        foreach (RaycastHit hit in hits)   
        {
            currentDist = hit.distance;
            //Debug.Log(transform.name + " " + hit.collider.gameObject.name + " " + hit.distance);
            if(hit.transform.tag == "Player")
                return _player;
        }          
                
        return hits.OrderBy(h => h.distance).First().transform;
    }

    public IEnumerator LegAttack(Vector3 target)
    {
        //print("LegAttack");
        while(Vector3.Distance(transform.position, target) > 0.05f) 
        {
            transform.position = Vector3.Lerp(transform.position, target, _smoothing * Time.deltaTime); //fixed?
            yield return null;
        }        
        //print("Reached the target");             
    }    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;        
        Debug.DrawLine(transform.position, transform.position - Vector3.up * currentDist, Color.magenta); 
        Gizmos.DrawWireSphere(transform.position -Vector3.up * currentDist, 7f);
    }
}
