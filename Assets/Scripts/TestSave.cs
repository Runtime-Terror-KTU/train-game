using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class TestSave : MonoBehaviour
{
    [Header("Manager States")]
    public bool isLoading = false;
    public bool isSaving = false;
    public bool isAfterLoad = false;
    public bool showPos = false;

    [Header("GameObjects")]
    public GameObject player;
    public GameObject weaponHolder;
    public GameObject PauseMenu;

    public string saveName = "Player";

    private WeaponSystem weaponSystem;
    private Player playerSystem;
    private PauseMenu pause;
    private int enemyCount;
    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("LoadState") == 1)
            isAfterLoad = true;

        weaponSystem = weaponHolder.GetComponent<WeaponSystem>();
        playerSystem = player.GetComponent<Player>();
        pause = PauseMenu.GetComponent<PauseMenu>();

        if(isAfterLoad)
        {
            AfterLoad();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isSaving)
        {
            CallSave();
        }

        if (isLoading)
        {
            CallLoad();
        }

        if (showPos)
            CurrentPos();

    }

    void AfterLoad()
    {
        isAfterLoad = false;

        string sceneName = "";
        float playerHealth = 0;
        float[] playerPos = new float[3];
        float[] playerRot = new float[4];
        bool[] foundWeapons = new bool[3];
        int[] reserveAmmo = new int[3];
        int[] currentAmmo = new int[3];
        int enemyCounts = 0;
        string[] enemyNames = new string[100];

        LoadFromFile(ref sceneName, ref playerHealth, ref playerPos, ref playerRot, ref foundWeapons, ref reserveAmmo, ref currentAmmo, ref enemyCounts, ref enemyNames);

        Debug.Log("SceneName: " + sceneName);
        Debug.Log("HP: " + playerHealth);
        Debug.Log("PlayerPos: " + "X:" + playerPos[0] + " Y:" + playerPos[1] + " Z:" + playerPos[2]);
        Debug.Log("PlayerRot: " + "X:" + playerRot[0] + " Y:" + playerRot[1] + " Z:" + playerRot[2] + " W:" + playerRot[3]);
        Debug.Log("PlayersWeapons: " + "Pistol:" + foundWeapons[0] + " AK:" + foundWeapons[1] + " SVD:" + foundWeapons[2]);
        Debug.Log("Reserve Ammo: " + "Pistol:" + reserveAmmo[0] + " AK:" + reserveAmmo[1] + " SVD:" + reserveAmmo[2]);
        Debug.Log("Current Ammo: " + "Pistol:" + currentAmmo[0] + " AK:" + currentAmmo[1] + " SVD:" + currentAmmo[2]);
        Debug.Log("Enemy Count: " + enemyCounts);
        for (int i = 0; i < enemyCounts; i++)
            Debug.Log(enemyNames[i]);

        Vector3 playerPosition = MakePlayerPos(playerPos);
        Quaternion playerRotation = MakePlayerRotation(playerRot);

        SetHealth(playerHealth);
        SetPlayerPosition(playerPosition);
        SetPlayerRotation(playerRotation);
        SetFoundWeapons(foundWeapons);
        SetReserveAmmo(reserveAmmo);
        SetCurrentAmmo(currentAmmo);

        PlayerPrefs.SetInt("LoadState", 0);
    }

    void CallLoad()
    {
        isLoading = false;

        string sceneName = GetSaveSceneName();

        pause.Resume();
        SceneManager.LoadSceneAsync(sceneName);

        isAfterLoad = true;
        PlayerPrefs.SetInt("LoadState", 1);

    }

    void CurrentPos()
    {
        float[] pos = GetPlayerPos();

        Debug.Log("cords: " + "X:" + pos[0] + " Y:" + pos[1] + " Z:" + pos[2]);
        showPos = false;
    }

    void CallSave()
    {
        string sceneName = GetCurrentSceneName();
        float playerHealth = GetPlayerHealth();
        float[] playerPos = GetPlayerPos();
        float[] playerRot = GetPlayerRotation();
        bool[] foundWeapons = GetFoundWeapons();
        int[] reserveAmmo = GetReserveAmmo();
        int[] currentAmmo = GetCurrentAmmo();

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = enemies.Length;

        Debug.Log("SceneName: " + sceneName);
        Debug.Log("HP: " + playerHealth);
        Debug.Log("PlayerPos: " + "X:" + playerPos[0] + " Y:" + playerPos[1] + " Z:" + playerPos[2]);
        Debug.Log("PlayerRot: " + "X:" + playerRot[0] + " Y:" + playerRot[1] + " Z:" + playerRot[2] + " W:" + playerRot[3]);
        Debug.Log("PlayersWeapons: " + "Pistol:" + foundWeapons[0] + " AK:" + foundWeapons[1] + " SVD:" + foundWeapons[2]);
        Debug.Log("Reserve Ammo: " + "Pistol:" + reserveAmmo[0] + " AK:" + reserveAmmo[1] + " SVD:" + reserveAmmo[2]);
        Debug.Log("Current Ammo: " + "Pistol:" + currentAmmo[0] + " AK:" + currentAmmo[1] + " SVD:" + currentAmmo[2]);
        Debug.Log("Enemy Count: " + enemyCount);

        SaveToFile(sceneName, playerHealth, playerPos, playerRot, foundWeapons, reserveAmmo, currentAmmo, enemyCount, enemies);


        isSaving = false;
    }

    // Getters
    float GetPlayerHealth()
    {
        return playerSystem.Health;
    }

    string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
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

        foreach (Transform weapon in weaponHolder.transform)
        {
            if (weaponSystem.foundPistol && weapon.gameObject.name == "Pistol")
            {
                currentAmmo[0] = weapon.GetComponent<GunLogic>().currentAmmo;
            }
            else if (currentAmmo[0] < 0)
            {
                currentAmmo[0] = 0;
            }

            if (weaponSystem.foundAK && weapon.gameObject.name == "AK")
            {
                currentAmmo[1] = weapon.GetComponent<GunLogic>().currentAmmo;
            }
            else if (currentAmmo[1] < 0)
            {
                currentAmmo[1] = 0;
            }

            if (weaponSystem.foundSVD && weapon.gameObject.name == "SVD")
            {
                currentAmmo[2] = weapon.GetComponent<GunLogic>().currentAmmo;
            }
            else if (currentAmmo[2] < 0)
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

        // Stupid ass fix, but it works ¯\_(ツ)_/¯
        weaponSystem.FoundWeapon();
        weaponSystem.FoundWeapon();
    }

    void SetReserveAmmo(int[] reserve)
    {
        weaponSystem.pistol_ammo = reserve[0];
        weaponSystem.ak_ammo = reserve[1];
        weaponSystem.svd_ammo = reserve[2];
    }

    void SetCurrentAmmo(int[] current)
    {
        foreach (Transform weapon in weaponHolder.transform)
        {
            if (weaponSystem.foundPistol && weapon.gameObject.name == "Pistol")
                weapon.GetComponent<GunLogic>().currentAmmo = current[0];

            if (weaponSystem.foundAK && weapon.gameObject.name == "AK")
                weapon.GetComponent<GunLogic>().currentAmmo = current[1];

            if (weaponSystem.foundSVD && weapon.gameObject.name == "SVD")
                weapon.GetComponent<GunLogic>().currentAmmo = current[2];
        }
    }

    void SetPlayerPosition(Vector3 pos)
    {
        player.transform.position = pos;
    }

    void SetPlayerRotation(Quaternion rot)
    {
        player.transform.rotation = rot;
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
    void SaveToFile(string sname, float hp, float[] pos, float[] rot, bool[] weps, int[] reserve, int[] current, int enemyCount, GameObject[] enemies)
    {
        string path = Application.persistentDataPath + "/" + saveName + ".txt";

        if (File.Exists(path))
            File.Delete(path);

        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.WriteLine(sname);
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

            sw.WriteLine(enemyCount);
            foreach (var item in enemies)
                sw.WriteLine(item.name);

            sw.Close();
        }
    }

    void LoadFromFile(ref string sceneName, ref float hp, ref float[] pos, ref float[] rot, ref bool[] weps, ref int[] reserve, ref int[] current, ref int enemyCount, ref string[] enemyNames)
    {
        string path = Application.persistentDataPath + "/" + saveName + ".txt";

        if (!File.Exists(path))
        {
            Debug.Log("SaveFile Is missing!");
        }

        using (StreamReader sr = new StreamReader(path))
        {
            sceneName = sr.ReadLine();
            hp = float.Parse(sr.ReadLine());

            for (int i = 0; i < 3; i++)
                pos[i] = float.Parse(sr.ReadLine());

            for (int i = 0; i < 4; i++)
                rot[i] = float.Parse(sr.ReadLine());

            for (int i = 0; i < 3; i++)
                weps[i] = bool.Parse(sr.ReadLine());


            for (int i = 0; i < 3; i++)
                reserve[i] = int.Parse(sr.ReadLine());

            for (int i = 0; i < 3; i++)
                current[i] = int.Parse(sr.ReadLine());

            enemyCount = int.Parse(sr.ReadLine());

            for (int i = 0; i < enemyCount; i++)
                enemyNames[i] = sr.ReadLine();

            sr.Close();
        }

    }

    string GetSaveSceneName()
    {
        string path = Application.persistentDataPath + "/" + saveName + ".txt";
        string name;

        using(StreamReader sr = new StreamReader(path))
        {
            name = sr.ReadLine();
        }

        return name;
    }
}
