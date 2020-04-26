using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mouseXInputName = default;
    [SerializeField] private string mouseYInputName = default;
    [SerializeField] private float mouseSensitivity = default;

    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform lookRoot;

    private Vector2 currentMousePosition;
    private Vector3 lookAngles;


    void Start()
    {
        LockCursor();
    }

    void Update()
    {
        LookAround();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LookAround()
    {
        currentMousePosition = new Vector2(Input.GetAxis(mouseYInputName), Input.GetAxis(mouseXInputName));

        lookAngles.x += currentMousePosition.x * mouseSensitivity * -1f;
        lookAngles.y += currentMousePosition.y * mouseSensitivity;

        lookAngles.x = Mathf.Clamp(lookAngles.x, -80, 80);

        lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, 0f);
        playerBody.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
    }
}
