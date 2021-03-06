using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    [SerializeField] Transform cam = null;
    [SerializeField] Transform orientation = null;
    float mouseX;
    float mouseY;

    float multiplier = 0.005f;

    private float xRotation;
    private float yRotation;

    private float sensibilityByPrefs;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        sensibilityByPrefs = PlayerPrefs.GetFloat("masterSen");
        if(sensibilityByPrefs == 0f)
            sensibilityByPrefs = 1f;

    }

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier * sensibilityByPrefs;
        xRotation -= mouseY * sensY * multiplier * sensibilityByPrefs;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, cam.transform.rotation.z);
        orientation.transform.localEulerAngles = new Vector3(orientation.transform.localEulerAngles.x, yRotation, orientation.transform.localEulerAngles.z);      
    }   
}