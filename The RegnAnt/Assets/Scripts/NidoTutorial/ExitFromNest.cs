using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitFromNest : MonoBehaviour
{
    public string mondoEsterno;

    void OnTriggerEnter()
    {
        SceneManager.LoadScene(mondoEsterno);
    }
}
