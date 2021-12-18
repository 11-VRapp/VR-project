using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneRailPoint : MonoBehaviour
{
    public float life;
    public PheromoneRailTrace trace;
    [SerializeField] private ParticleSystem ps;
    private BoxCollider triggerBox;
    
    void Start() 
    {
        trace = gameObject.GetComponentInParent<PheromoneRailTrace>();        
        ps = GetComponent<ParticleSystem>();
        triggerBox = GetComponent<BoxCollider>();
        life = 20f;
    }
    void Update() 
    {
        var emission = ps.emission;
        emission.rateOverTime = life;  
        
        if(life > 0f)        
            life -= Time.deltaTime;                     
        else        
            triggerBox.enabled = false; 
    }  

    public void addNewLife()  
    {
        life += 20f;
        triggerBox.enabled = true; 
    }    
}
