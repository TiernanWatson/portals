using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField] private float xSensitivity = 1f;
    [SerializeField] private float ySensitivity = 1f;
    [SerializeField] private float pitchMax = 80f;
    [SerializeField] private float pitchMin = -45f;

    private float pitch = 0f;
    private float yaw = 0f;

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        pitch += Input.GetAxis("Mouse Y") * ySensitivity;
        yaw += Input.GetAxis("Mouse X") * xSensitivity;

        Quaternion targetRot = Quaternion.Euler(0f, yaw, 0f);

        transform.rotation = targetRot;
    }
}
