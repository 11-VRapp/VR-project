using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for the movement of the leg


public class FootIKSolver : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] private Transform body;
    [SerializeField] private FootIKSolver otherFoot;
    
    private Vector3 oldPosition, currentPosition, newPosition;
    private Vector3 oldNormal, currentNormal, newNormal;
    [SerializeField] private float speed = 3;
    [SerializeField] private float stepDistance = 1f;
    [SerializeField] private float stepLength = 0.6f;
    [SerializeField] private float stepHeight = 0.2f;
    [SerializeField] private float lerp;
    [SerializeField] private float footSpacingLat = 2f;
    [SerializeField] private float footSpacingForw = 2f;

    
    // Start is called before the first frame update
    void Start()
    {
        currentPosition = oldPosition = newPosition = transform.position;
        currentNormal = oldNormal = newNormal = transform.up;
        lerp = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;

        Ray ray = new Ray(body.position + (body.right * footSpacingLat) + (body.forward * footSpacingForw), -body.up);
        //Debug.DrawRay(body.position + (body.right * footSpacingLat) + (body.forward * footSpacingForw), -body.up, Color.yellow);

        
        if(Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
        {      
            //Debug.DrawLine(transform.position, info.point, Color.green);
            //transform.position = info.point; stick it to the point
            if(Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1) 
            //if(Vector3.Distance(newPosition, info.point) > stepDistance && lerp >= 1) 
            {               
                lerp = 0;
                int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                newPosition = info.point + (body.forward * stepLength * direction);
                newNormal = info.normal;
               
            }
        }


        if(lerp < 1)
        {
            Vector3 arcPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            arcPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = arcPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
        
    }


    void OnDrawGizmos()
    {
        /*Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(body.position + (body.right * footSpacingLat) + (body.forward * footSpacingForw), 0.2f);*/
        
    }

    public bool IsMoving()
    {
        return lerp < 1;
    }
}
