using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiderLeg : Damagable
{
    [Header("Leg Attributes")]
    [SerializeField] private float damage = 7f;
    [SerializeField] private spiderLife _sl;
    public override void Hit()
    {
        _sl.setDamage(damage);
    }
}
