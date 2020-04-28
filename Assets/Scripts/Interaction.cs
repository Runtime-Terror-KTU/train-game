using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float radius = 3f;
    public GameObject interactableObj;
    public Camera cam;
    public LayerMask LayerInteract;
    public WeaponSystem weaponSystem;
    public Explanation explanation;

    private void Update()
    {
       Interact();       
    }

    public void Interact()
    {
        RaycastHit hit;
        Vector3 fwd = cam.transform.TransformDirection(Vector3.forward);
        explanation.Use();
        if (Physics.Raycast(cam.transform.position, fwd, out hit, radius, LayerInteract.value))
        {
            explanation.Use(hit);
            if (Input.GetButtonDown(GameConstants.ButtonNameUse))
            {
                if (hit.collider.CompareTag("InteractableObj"))
                {
                    interactableObj = hit.collider.gameObject;
                    //Do something
                    Debug.Log("Succes");
                }

                if (hit.collider.CompareTag("Guns"))
                {
                    if (hit.transform.name == "AK")
                    {
                        weaponSystem.foundAK = true;
                        weaponSystem.FoundWeapon();
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.transform.name == "SVD")
                    {
                        weaponSystem.foundSVD = true;
                        weaponSystem.FoundWeapon();
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.transform.name == "Pistol")
                    {
                        weaponSystem.foundPistol = true;
                        weaponSystem.FoundWeapon();
                        Destroy(hit.collider.gameObject);
                    }
                }
            }
        }
    }

}
