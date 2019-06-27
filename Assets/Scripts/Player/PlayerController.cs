using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1f;

    [SerializeField] private Transform cam;

    private CharacterController charControl;

    private void Start()
    {
        charControl = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 moveDir = Input.GetAxis("Vertical") * cam.transform.forward
            + Input.GetAxis("Horizontal") * cam.transform.right;

        Vector3 velocity = moveDir * walkSpeed;

        charControl.Move(velocity * Time.deltaTime);
    }

    public void SetCharControlActive(bool state)
    {
        charControl.enabled = state;
    }
}
