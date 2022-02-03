using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneRailPoint : MonoBehaviour
{
    public float life;
    private float _maxLife = 500f;
    public PheromoneRailTrace trace;
    [SerializeField] private ParticleSystem[] ps;   
    private BoxCollider triggerBox;
    
    void Start() 
    {        
        trace = gameObject.GetComponentInParent<PheromoneRailTrace>(); 
        triggerBox = GetComponent<BoxCollider>();
        life = _maxLife;
    }
    void Update() 
    {
        foreach(ParticleSystem p in ps)
        {
            var emission = p.emission;
            emission.rateOverTime = life * 0.02f; 
            var sp = p.main.startSpeed; 
            sp  = life * 0.002f;                
        }     
        
        if(life > 0f)        
            life -= Time.deltaTime;                     
        else        
            triggerBox.enabled = false; 
    }  

    public void addLife()  
    {
        life += 20f;
        triggerBox.enabled = true; 
    }    
}
