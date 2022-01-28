using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLife : MonoBehaviour
{
    public float life;
    [SerializeField] private float _maxlife;
    void Start()
    {
        life = 100f;
        _maxlife = life;
    }

    public void setDamage(float dmg)
    {
        life -= dmg;
        if (life < 0)
        {
            Debug.LogError("MORTOOOOOOOOOOOOOOOOOOOOOOOOO");
        }
    }

    public void setHeal(float qty)
    {
        life += qty;
        if (life > _maxlife)
            life = _maxlife;
    }
}
