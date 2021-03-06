using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class spiderLife : MonoBehaviour
{
    [Header("Spider life stats")]
    [SerializeField] private float life;
    [SerializeField] private float regenTime = 5f;
    [SerializeField] private float regenQty= 2f;
    private float maxLife = 400f;

    void Start()
    {
        life = maxLife;
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
            PlayerPrefs.SetInt("gameFinished", 1);
            PlayerPrefs.SetInt("diary", 1);
            SceneManager.LoadScene("Ending");
            return;
        }

        FindObjectOfType<audioGeneral>().spiderLifeAudio(life, maxLife);
    }

    public void setHeal(float qty) => life += qty;    
}
