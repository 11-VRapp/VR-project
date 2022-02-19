using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cambiScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
            SceneManager.LoadScene("Nido");

        if (Input.GetKey(KeyCode.Alpha2))
            SceneManager.LoadScene("MondoEsterno");

        if (Input.GetKey(KeyCode.Alpha3))
            SceneManager.LoadScene("SpiderFight");

        if (Input.GetKey(KeyCode.Alpha4))
            SceneManager.LoadScene("SpiderFight2");
    }
}
