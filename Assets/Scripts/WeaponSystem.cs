using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public PlayerControls playerControls;
    public GameObject weaponStash;

    [Header("Currently selected weapon")]
    public int selectedWeapon = 0;

    [Header("Current weapon stats")]
    public int currentAmmo = 0;
    public int currentReserve = 0;

    [Header("Reserve ammo")]
    public int ak_ammo = 60;
    public int svd_ammo = 15;
    public int pistol_ammo = 25;

    [Header("State variables")]
    public bool isReloading = false;

    public bool foundPistol = false;
    public bool foundAK = false;
    public bool foundSVD = false;

    void Start()
    {
        DisableStash();
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWep = selectedWeapon;

        if(!isReloading)
        {
            if (Input.GetAxis(GameConstants.ButtonNameSwitchWeapon) > 0f)
            {
                if (selectedWeapon >= transform.childCount - 1)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
            }

            if (Input.GetAxis(GameConstants.ButtonNameSwitchWeapon) < 0f)
            {
                if (selectedWeapon <= 0)
                    selectedWeapon = transform.childCount - 1;
                else
                    selectedWeapon--;
            }

            if (previousSelectedWep != selectedWeapon)
            {
                SelectWeapon();
            }
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
          
            if(i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }
    }    

    void DisableStash()
    {    
        foreach(Transform weapon in weaponStash.transform)
        {
            weapon.gameObject.SetActive(false);
        }
    }


    public void FoundWeapon()
    {
        foreach (Transform weapon in weaponStash.transform)
        {
            if (foundPistol == true && weapon.gameObject.name=="Pistol")
            {
                weapon.parent = transform;
                //foundPistol = false;
            }
            if (foundAK == true && weapon.gameObject.name == "AK")
            {
                weapon.parent = transform;
                //foundAK = false;
            }
            if (foundSVD == true && weapon.gameObject.name == "SVD")
            {
                weapon.parent = transform;
                //foundSVD = false;
            }
        }
    }
}
