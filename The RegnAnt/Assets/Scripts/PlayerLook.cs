using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    [SerializeField] Transform cam = null;
    [SerializeField] Transform orientation = null;  
    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    [Header("Camera")]
    [SerializeField] private float xRotation;
    [SerializeField] private float yRotation;    
    
    
    private void Start()
    {        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;        
    }

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, cam.transform.rotation.z);
        orientation.transform.localEulerAngles = new Vector3(orientation.transform.localEulerAngles.x, yRotation, orientation.transform.localEulerAngles.z);   
           
    }
}