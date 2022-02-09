using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public Vector3 NorthDirection;
    public Transform Player;
    public Quaternion nestDirection;

    //public RectTransform Northlayer;
    public RectTransform nestLayer;
    public Transform nestPlace;

    void Update()
    {
        ChangeNorthDirection();
        ChangeNestDirection();
    }

    public void ChangeNorthDirection()
    {
        NorthDirection.z = Player.eulerAngles.y;
        //Northlayer.localEulerAngles = NorthDirection;
    }

    public void ChangeNestDirection()
    {
        Vector3 dir = transform.position - nestPlace.position;
            
        nestDirection = Quaternion.LookRotation(dir);

        nestDirection.z = -nestDirection.y;
        nestDirection.x = 0;
        nestDirection.y = 0;

        nestLayer.localRotation = nestDirection * Quaternion.Euler(NorthDirection);
    }
}
