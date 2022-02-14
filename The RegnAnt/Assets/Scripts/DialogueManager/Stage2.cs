using UnityEngine;
using UnityEngine.UI;

public class Stage2 : MonoBehaviour
{
    public GameObject imageBox;
    public Text nameText;   
    public GameObject btn_prefab;  

    public void createStage(Dialogue dialogue)
    {
        for(int i = 0; i < dialogue.domande.Length; i++)
        {
            GameObject newButton = Instantiate(btn_prefab);
            newButton.transform.SetParent(imageBox.transform, false);
            newButton.GetComponent<RectTransform>().localPosition = new Vector3(11.4f, 40f - i * 30f, 0f);          
            newButton.transform.GetChild(0).GetComponent<Text>().text = dialogue.domande[i];
            newButton.transform.GetChild(1).GetComponent<Text>().text = dialogue.risposte[i];
            newButton.GetComponent<Button>().onClick.AddListener(() => buttonAskHandler(newButton));
            newButton.GetComponent<Button>().onClick.AddListener(() => transform.GetComponent<DialogueManager>().DisplayStage3());            
        }
    }

    void buttonAskHandler(GameObject btn)
    {
        transform.GetComponent<DialogueManager>()._stage3.title.text = btn.transform.GetChild(0).GetComponent<Text>().text;
        transform.GetComponent<DialogueManager>()._stage3.answer.text = btn.transform.GetChild(1).GetComponent<Text>().text; 
    }

}
