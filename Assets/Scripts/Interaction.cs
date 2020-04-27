﻿using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float radius = 3f;
    public GameObject interactableObj;
    public LayerMask LayerInteract;

    private void Update()
    {
        if(Input.GetButtonDown(GameConstants.ButtonNameUse))
        {
            Interact();
        }
    }

    public void Interact()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hit, radius, LayerInteract.value))
        {
            if (hit.collider.CompareTag("InteractableObj"))
            {
                interactableObj = hit.collider.gameObject;      
                Debug.Log("Succes");
                interactableObj.SetActive(false);
            }
        }
    }

}
