using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public Text currentAmmoText;
    public Text reserveAmmoText;
    public GunLogic gunLogic;

    // Update is called once per frame
    void Update()
    {
        currentAmmoText.text = gunLogic.currentAmmo.ToString();
        reserveAmmoText.text = gunLogic.reserveAmmo.ToString();
    }
}
