using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitManager : MonoBehaviour
{


    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    public void quit()
    {
        Application.Quit();
    }


    public void hideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
