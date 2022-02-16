using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiderLife : MonoBehaviour
{
    [Header("Spider life stats")]
    [SerializeField] private float life = 400f;
    [SerializeField] private float regenTime = 5f;
    [SerializeField] private float regenQty= 2f;

    void Start()
    {
        StartCoroutine(regeneration());
    }

    private IEnumerator regeneration()
    {
        setHeal(regenQty);
        yield return new WaitForSeconds(regenTime);
    }
    public void setDamage(float dmg)
    {
        life -= dmg;
        if (life < 0)
        {            
            //! GOOD ENDing
            StartCoroutine(GameObject.FindObjectOfType<EndingManager>().finalWin());
        }
    }

    public void setHeal(float qty) => life += qty;    
}
