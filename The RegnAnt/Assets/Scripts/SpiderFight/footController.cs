using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class footController : MonoBehaviour
{
    [SerializeField] private LayerMask _antMask;

    public Transform CheckEnemyToAttack()
    {
        
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 5, -transform.up, 10f, _antMask);

        foreach (RaycastHit hit in hits) //just for debug        
            Debug.Log(hit.collider.gameObject.name + " " + hit.distance);
        

        return hits.OrderBy(h => h.distance).First().transform;
    }

    /*public int GetIndexOfLowestValue(float[] arr)
    {
        float value = float.PositiveInfinity;
        int index = -1;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] < value)
            {
                index = i;
                value = arr[i];
            }
        }
        return index;
    }*/

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 5f);
    }
}
