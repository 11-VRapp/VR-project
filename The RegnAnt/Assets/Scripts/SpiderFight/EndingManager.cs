using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class EndingManager : MonoBehaviour
{
    [SerializeField] private GameObject _finaleObj;

    [Header("Death")]
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private TMP_Text _deathText;
    [SerializeField] private TMP_Text _deathPhrase;
    [SerializeField] private TMP_Text _deathAchievmentDiary;

    [Header("Win")]
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TMP_Text _winText;
    [SerializeField] private TMP_Text _winPhrase;
    [SerializeField] private TMP_Text _winAchievmentDiary;

    void Start()
    {        
        if(PlayerPrefs.GetInt("diary") == 0) //not win
            StartCoroutine(finalDeath());
        else
            StartCoroutine(finalWin());
    }
  
    public IEnumerator finalDeath()
    {
        //hideAllObjects();
        _deathPanel.SetActive(true);
        yield return StartCoroutine(TypeSentence(_deathText, "SEI MORTO", .5f));
        
        yield return StartCoroutine(TypeSentence(_deathPhrase, "Il tuo sacrificio non sarà vano.\nLa colonia continuerà a vivere e opporsi a ogni ostacolo per garantire la sua sopravvivenza", .1f));
        yield return StartCoroutine(TypeSentence(_deathAchievmentDiary, "Per sbloccare il diario nel menu principale vinci il combattimento iniziando una nuova partita o tramite la modalità apposita", 0f));
        
        StartCoroutine(ToMainScreen());
    }

    public IEnumerator finalWin()
    {     
        _winPanel.SetActive(true);
        _winText.transform.DOScale(Vector3.zero, 0f);
        _winText.transform.DOScale(Vector3.one, 2f).SetEase(Ease.InBounce);
        yield return StartCoroutine(TypeSentence(_winPhrase, "Sei riuscito valorosamente a sconfiggere la minaccia insieme alle tue sorelle, ma i nemici della colonia non finiscono mai", .05f));
        yield return StartCoroutine(TypeSentence(_winAchievmentDiary, "Il diario nel menu principale è stato sbloccato", 0f));

        
        StartCoroutine(ToMainScreen());
    }   

    IEnumerator TypeSentence(TMP_Text field, string sentence, float delay)
    {
        field.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            field.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator ToMainScreen()
    {
        yield return new WaitForSeconds(7f);
        //Load Main Menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
