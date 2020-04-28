using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explanation : MonoBehaviour
{
    public Text text;

    public void Use(RaycastHit hit)
    {
        if (hit.collider.CompareTag("InteractableObj"))
        {
            text.text = "Press E to use";
        }
        else if (hit.collider.CompareTag("Guns"))
        {
            text.text = "Press E to pick up a " + hit.collider.name; ;
        }
    }

    public void Use()
    {
        text.text = "";
    }


}
