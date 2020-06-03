using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;

public class TestSave : MonoBehaviour
{
    [Header("Manager States")]
    public bool isLoading = false;
    public bool isSaving = false;

    [Header("GameObjects")]
    public GameObject player;
    public GameObject weaponHolder;

    public string saveName = "Player";

    private WeaponSystem weaponSystem;
    private Player playerSystem;
    private int enemyCount;
    private GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        weaponSystem = weaponHolder.GetComponent<WeaponSystem>();
        playerSystem = player.GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSaving)
        {
            CallSave();
        }
            
    }

    void CallSave()
    {
        float playerHealth = GetPlayerHealth();
        float[] playerPos = GetPlayerPos();
        float[] playerRot = GetPlayerRotation();
        bool[] foundWeapons = GetFoundWeapons();
        int[] reserveAmmo = GetReserveAmmo();
        int[] currentAmmo = GetCurrentAmmo();

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = enemies.Length;

        Debug.Log("HP: " + playerHealth);
        Debug.Log("PlayerPos: " + "X:" + playerPos[0] + " Y:" + playerPos[1] + " Z:" + playerPos[2]);
        Debug.Log("PlayerRot: " + "X:" + playerRot[0] + " Y:" + playerRot[1] + " Z:" + playerRot[2] + " W:" + playerRot[3]);
        Debug.Log("PlayersWeapons: " + "Pistol:" + foundWeapons[0] + " AK:" + foundWeapons[1] + " SVD:" + foundWeapons[2]);
        Debug.Log("Reserve Ammo: " + "Pistol:" + reserveAmmo[0] + " AK:" + reserveAmmo[1] + " SVD:" + reserveAmmo[2]);
        Debug.Log("Current Ammo: " + "Pistol:" + currentAmmo[0] + " AK:" + currentAmmo[1] + " SVD:" + currentAmmo[2]);
        Debug.Log("Enemy Count: " + enemyCount);

        SaveToFile(playerHealth,playerPos,playerRot,foundWeapons,reserveAmmo, currentAmmo);


        isSaving = false;
    }

    // Getters
    float GetPlayerHealth()
    {
        return playerSystem.Health;
    }

    bool[] GetFoundWeapons()
    {
        bool[] foundWeapons = new bool[3];

        foundWeapons[0] = weaponSystem.foundPistol;
        foundWeapons[1] = weaponSystem.foundAK;
        foundWeapons[2] = weaponSystem.foundSVD;

        return foundWeapons;
    }

    int[] GetReserveAmmo()
    {
        int[] reserveAmmo = new int[3];

        reserveAmmo[0] = weaponSystem.pistol_ammo;
        reserveAmmo[1] = weaponSystem.ak_ammo;
        reserveAmmo[2] = weaponSystem.svd_ammo;

        return reserveAmmo;
    }

    int[] GetCurrentAmmo()
    {
        int[] currentAmmo = new int[3];

        for (int i = 0; i < currentAmmo.Length; i++)
            currentAmmo[i] = -1;

        foreach(Transform weapon in weaponHolder.transform)
        {
            if(weaponSystem.foundPistol && weapon.gameObject.name == "Pistol")
            {
                currentAmmo[0] = weapon.GetComponent<GunLogic>().currentAmmo;
            }
            else if(currentAmmo[0] < 0)
            {
                currentAmmo[0] = 0;
            }

            if(weaponSystem.foundAK && weapon.gameObject.name == "AK")
            {
                currentAmmo[1] = weapon.GetComponent<GunLogic>().currentAmmo;
            }
            else if( currentAmmo[1] < 0)
            {
                currentAmmo[1] = 0;
            }

            if(weaponSystem.foundSVD && weapon.gameObject.name == "SVD")
            {
                currentAmmo[2] = weapon.GetComponent<GunLogic>().currentAmmo;
            }
            else if(currentAmmo[2] < 0)
            {
                currentAmmo[2] = 0;
            }
        }

        return currentAmmo;
    }

    // Setters

    void SetHealth(float hp)
    {
        playerSystem.Health = hp;
    }

    void SetFoundWeapons(bool[] weps)
    {
        weaponSystem.foundPistol = weps[0];
        weaponSystem.foundAK = weps[1];
        weaponSystem.foundSVD = weps[2];
    }

    void SetReserveAmmo(int[] reserve)
    {
        weaponSystem.pistol_ammo = reserve[0];
        weaponSystem.ak_ammo = reserve[1];
        weaponSystem.svd_ammo = reserve[2];
    }

    // Conversion functions for getting variables
    float[] GetPlayerPos()
    {
        Vector3 pos = player.transform.position;
        float[] cors = new float[3];

        cors[0] = pos.x;
        cors[1] = pos.y;
        cors[2] = pos.z;

        return cors;
    }

    float[] GetPlayerRotation()
    {
        Quaternion qRot = player.transform.rotation;
        float[] fRot = new float[4];

        fRot[0] = qRot.x;
        fRot[1] = qRot.y;
        fRot[2] = qRot.z;
        fRot[3] = qRot.w;

        return fRot;
    }

    // Conversion functions for setting variables
    Vector3 MakePlayerPos(float[] cors)
    {
        Vector3 pos = new Vector3();

        pos.x = cors[0];
        pos.y = cors[1];
        pos.z = cors[2];

        return pos;
    }

    Quaternion MakePlayerRotation(float[] rot)
    {
        Quaternion qRot = new Quaternion();

        qRot.x = rot[0];
        qRot.y = rot[1];
        qRot.z = rot[2];
        qRot.w = rot[3];

        return qRot;
    }

    // SaveToFile
    void SaveToFile(float hp, float[] pos, float[] rot, bool[] weps, int[] reserve, int[] current)
    {
        string path = Application.persistentDataPath + "/" + saveName + ".txt";

        if (File.Exists(path))
            File.Delete(path);

        using(StreamWriter sw = new StreamWriter(path))
        {
            sw.WriteLine(hp);
            
            foreach (var item in pos)
                sw.WriteLine(item);

            foreach (var item in rot)
                sw.WriteLine(item);

            foreach (var item in weps)
                sw.WriteLine(item);

            foreach (var item in reserve)
                sw.WriteLine(item);

            foreach (var item in current)
                sw.WriteLine(item);

            sw.Close();
        }
    }
}
