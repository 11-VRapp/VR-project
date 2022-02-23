using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioGeneral : MonoBehaviour
{
    private bool perc10newAnt = false;
    private bool perc10newSpider = false;
    private bool perc25newAnt = false;
    private bool perc25newSpider = false;
    private bool perc50newAnt = false;
    private bool perc50newSpider = false;
    private bool perc75newAnt = false;
    private bool perc75newSpider = false;

    private AudioManager _audioManager;


    private bool playing = false;
    void Start()
    {
        _audioManager = GetComponent<AudioManager>();
       _audioManager.Play("WindAudio");
    }

    public void antLifeAudio(float remainingLife, float maxLife)
    {        
        if(playing)
            return;
        int perc = (int) Mathf.Round((remainingLife / maxLife) * 100f);

        Debug.LogWarning(perc);
        
        if (0 > perc && perc <= 10 && !perc10newAnt)
        {
            _audioManager.Play("Perc10ant");
            perc10newAnt = true;
            StartCoroutine(resetPlaying(_audioManager.getClip("Perc10ant").source.clip.length));
        }
        else if (10 > perc && perc <= 25 && !perc25newAnt)
        {
            _audioManager.Play("Perc25ant");
            perc25newAnt = true;
            StartCoroutine(resetPlaying(_audioManager.getClip("Perc25ant").source.clip.length));
        }
        else if (25 > perc && perc <= 50 && !perc50newAnt)
        {
            _audioManager.Play("Perc50ant");
            perc50newAnt = true;
            StartCoroutine(resetPlaying(_audioManager.getClip("Perc50ant").source.clip.length));
        }
        else if (50 > perc && perc <= 75 && !perc75newAnt)
        {
            _audioManager.Play("Perc75ant");
            perc75newAnt = true;
            StartCoroutine(resetPlaying(_audioManager.getClip("Perc75ant").source.clip.length));
            Debug.LogWarning("ASDSASDASD");
        }        
    }

    public void spiderLifeAudio(float remainingLife, float maxLife)
    {
        if(playing)
            return;
        int perc = (int) Mathf.Round((remainingLife / maxLife) * 100f);
        Debug.Log(perc);

        if (0 > perc && perc <= 10 && !perc10newSpider)
        {
            _audioManager.Play("perc10spider");
            perc10newSpider = true;
            StartCoroutine(resetPlaying(_audioManager.getClip("perc10spider").source.clip.length));
        }
        else if (10 > perc && perc <= 25 && !perc25newSpider)
        {
            _audioManager.Play("perc25spider");
            perc25newSpider = true;
            StartCoroutine(resetPlaying(_audioManager.getClip("perc25spider").source.clip.length));
        }
        else if (25 > perc && perc <= 50 && !perc50newSpider)
        {
            _audioManager.Play("perc50spider");
            perc50newSpider = true;
            StartCoroutine(resetPlaying(_audioManager.getClip("perc50spider").source.clip.length));
        }
        else if (50 > perc && perc <= 75 && !perc75newSpider)
        {
            _audioManager.Play("perc75spider");
            perc75newSpider = true;
            StartCoroutine(resetPlaying(_audioManager.getClip("perc75spider").source.clip.length));
        }
    }

    private IEnumerator resetPlaying(float clipLenght)
    {
        yield return new WaitForSeconds(clipLenght);
        playing = false;
    }
}
