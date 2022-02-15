using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiderBody : Damagable
{
    [Header("Body Attributes")]
    [SerializeField] private float damage = 13f;
    [SerializeField] private spiderLife _sl;
    public override void Hit()
    {
        _sl.setDamage(damage);
    }
}
