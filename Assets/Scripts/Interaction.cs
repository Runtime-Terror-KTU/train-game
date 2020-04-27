using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float radius = 3f;
    public GameObject interactableObj;
    public Camera cam;
    public LayerMask LayerInteract;
    public WeaponSystem weaponSystem;

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
        Vector3 fwd = cam.transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(cam.transform.position, fwd, out hit, radius, LayerInteract.value))
        {
            if (hit.collider.CompareTag("InteractableObj"))
            {
                interactableObj = hit.collider.gameObject;      
                Debug.Log("Succes");
                interactableObj.SetActive(false);
            }

            if(hit.collider.CompareTag("Guns"))
            {
                if(hit.transform.name=="AK")
                {
                    weaponSystem.foundAK = true;
                    weaponSystem.FoundWeapon();
                    Destroy(hit.collider.gameObject);
                }
                else if(hit.transform.name=="SVD")
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
