using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isLoad = false;
    public PlayerData playerData;
    public List<EnemyData> enemyDatas;
    public GameObject[] enemies;
    public GameObject[] collectibles;
    public GameObject[] enemyPrefabs;
    public GameObject[] collectiblePrefabs;
    public SaveData saveData = new SaveData();
    void Start()
    {
        if (isLoad)
        {

        }
        else
        {
            spawnObjects("EnemyLocation", enemies, enemyPrefabs);
            spawnObjects("CollectibleLocation", collectibles, collectiblePrefabs);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayer();
    }

    void spawnObjects(string tag, GameObject[] objects, GameObject[] prefabs)
    {
        objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            GameObject igObj = Instantiate(prefabs[Random.Range(0,2)], obj.transform.position, obj.transform.rotation);
            Destroy(obj);
        }
    }

    public void UpdatePlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        playerData.position = playerObj.transform.position;
        playerData.rotation = playerObj.transform.rotation;

        Player player = playerObj.GetComponent<Player>();
        playerData.health = player.Health;

        GameObject weaponHolder = playerObj.transform.GetChild(0).gameObject;
        weaponHolder = weaponHolder.transform.GetChild(0).gameObject;
        WeaponSystem weaponSystem = weaponHolder.GetComponent<WeaponSystem>();

        playerData.reserveAmmo[0] = weaponSystem.pistol_ammo;
        playerData.reserveAmmo[1] = weaponSystem.ak_ammo;
        playerData.reserveAmmo[2] = weaponSystem.svd_ammo;

        playerData.currentWeapon = weaponSystem.selectedWeapon;

        playerData.ammo[0] = 0;
        playerData.ammo[1] = 0;
        playerData.ammo[2] = 0;

        playerData.weaponsUnlocked[0] = weaponSystem.foundPistol;
        GunLogic gunLogic;
        if (weaponSystem.foundPistol)
        {
            gunLogic = weaponHolder.transform.GetChild(1).gameObject.GetComponent<GunLogic>();
            playerData.ammo[0] = gunLogic.currentAmmo;
        }

        playerData.weaponsUnlocked[1] = weaponSystem.foundAK;
        if (weaponSystem.foundAK)
        {
            gunLogic = weaponHolder.transform.GetChild(2).gameObject.GetComponent<GunLogic>();
            playerData.ammo[1] = gunLogic.currentAmmo;
        }
        playerData.weaponsUnlocked[2] = weaponSystem.foundSVD;
        if (weaponSystem.foundSVD)
        {
            gunLogic = weaponHolder.transform.GetChild(3).gameObject.GetComponent<GunLogic>();
            playerData.ammo[2] = gunLogic.currentAmmo;
        }
    }

    public void UpdateEnemies()
    {

    }

    public void UpdateCollectibles()
    {

    }
}
