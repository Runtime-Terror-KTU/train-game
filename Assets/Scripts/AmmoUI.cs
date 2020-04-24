using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public Text currentAmmoText;
    public Text reserveAmmoText;
    public WeaponSystem weaponSystem;

    // Update is called once per frame
    void Update()
    {
        currentAmmoText.text = weaponSystem.currentAmmo.ToString();
        reserveAmmoText.text = weaponSystem.currentReserve.ToString();
    }
}
