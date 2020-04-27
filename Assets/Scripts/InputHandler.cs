using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Tooltip("Sensitivity multiplier for moving the camera around")]
    public float mouseSensitivity = 1f;
    [Tooltip("Used to flip the vertical input axis")]
    public bool invertYAxis = false;
    [Tooltip("Used to flip the horizontal input axis")]
    public bool invertXAxis = false;

    Controls controls;
    bool fireInputWasHeld;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        fireInputWasHeld = GetFireInputHeld();
    }

    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked && !PauseMenu.OnPause;
    }

    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxisRaw(GameConstants.AxisNameHorizontal), 0f, Input.GetAxisRaw(GameConstants.AxisNameVertical));
            move = Vector3.ClampMagnitude(move, 1);
            return move;
        }
        return Vector3.zero;
    }

    public float GetLookInputsHorizontal()
    {
        if(invertXAxis)
            return GetMouseLookAxis(GameConstants.MouseAxisNameHorizontal) * -1;
        return GetMouseLookAxis(GameConstants.MouseAxisNameHorizontal);
    }

    public float GetLookInputsVertical()
    {
        if (invertYAxis)
            return GetMouseLookAxis(GameConstants.MouseAxisNameVertical) * -1;
        return GetMouseLookAxis(GameConstants.MouseAxisNameVertical);
    }

    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown(GameConstants.ButtonNameJump);
        }

        return false;
    }

    public bool GetCrouchInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonDown(GameConstants.ButtonNameCrouch);
        }

        return false;
    }

    public bool GetCrouchInputReleased()
    {
        if (CanProcessInput())
        {
            return Input.GetButtonUp(GameConstants.ButtonNameCrouch);
        }

        return false;
    }

    public bool GetFireInputDown()
    {
        return GetFireInputHeld() && !fireInputWasHeld;
    }

    public bool GetFireInputReleased()
    {
        return !GetFireInputHeld() && fireInputWasHeld;
    }

    public bool GetFireInputHeld()
    {
        if (CanProcessInput())
        {
                return Input.GetButton(GameConstants.ButtonNameFire);
        }
        return false;
    }

    //Justui
    public int GetSelectWeaponInput()
    {
        if (CanProcessInput())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                return 1;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                return 2;
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                return 3;
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                return 4;
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                return 5;
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                return 6;
            else
                return 0;
        }
        return 0;
    }

    float GetMouseLookAxis(string mouseInputName)
    {
        if (CanProcessInput())
        {
            float i = Input.GetAxisRaw(mouseInputName);
            i *= mouseSensitivity;
            i *= 0.01f;
            return i;
        }
        return 0f;
    }
}
