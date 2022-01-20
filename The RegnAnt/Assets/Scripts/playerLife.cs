using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLife : MonoBehaviour
{
    public float life;
    void Start()
    {
        life = 100f;
    }

    public void setDamage(float dmg)
    {
        life -= dmg;
        if(life < 0)
        {
            Debug.LogError("MORTOOOOOOOOOOOOOOOOOOOOOOOOO");
        }
    }
    
}
