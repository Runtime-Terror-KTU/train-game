using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SerializationManager serializationManager = new SerializationManager();
    public bool isLoad = false;
    public bool isSave = false;
    public PlayerData playerData;
    public List<EnemyData> enemyDatas;
    public List<CollectibleData> collectibleDatas;
    public GameObject[] enemies;
    public GameObject[] collectibles;
    public GameObject[] enemyPrefabs;
    public GameObject[] collectiblePrefabs;
    public SaveData saveData = new SaveData();
    public SaveData loadData = new SaveData();

    public GameObject playerObj;
    void Start()
    {
        if (isLoad)
            CallLoad();
        else
        {
            spawnObjects("EnemyLocation", enemies, enemyPrefabs);
            spawnObjects("CollectibleLocation", collectibles, collectiblePrefabs);
        }
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isSave)
            CallSave();
    }

    void spawnObjects(string tag, GameObject[] objects, GameObject[] prefabs)
    {
        objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            GameObject igObj = Instantiate(prefabs[Random.Range(0,prefabs.Length)], obj.transform.position, obj.transform.rotation);
            Destroy(obj);
        }
    }

    public void UpdateSaveData()
    {
        saveData.player = playerData;
        saveData.enemies = enemyDatas;
        saveData.collectibles = collectibleDatas;
    }

    public void UpdatePlayerData()
    {
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

    public void UpdateEnemiesData()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObj in enemies)
        {
            
            EnemyData enemyData = new EnemyData();
            EnemyAI enemy = enemyObj.GetComponent<EnemyAI>();
            if (enemy.isRanged)
                enemyData.enemyType = EnemyType.Shooter;
            else
                enemyData.enemyType = EnemyType.Melee;
            enemyData.position = enemyObj.transform.position;
            enemyData.rotation = enemyObj.transform.rotation;

            enemyDatas.Add(enemyData);
        }
    }

    public void UpdateCollectiblesData()
    {
        collectibles = GameObject.FindGameObjectsWithTag("Object");
        foreach (GameObject collectibleObj in collectibles)
        {
            CollectibleData collectibleData = new CollectibleData();

            collectibleData.position = collectibleObj.transform.position;
            collectibleData.rotation = collectibleObj.transform.rotation;
            AmmoPickup ammoPickup;
            if (collectibleObj.TryGetComponent<AmmoPickup>(out ammoPickup))
                collectibleData.collectibleType = CollectibleType.Ammo;
            else
                collectibleData.collectibleType = CollectibleType.Health;

            collectibleDatas.Add(collectibleData);
        }
    }

    public void CallSave()
    {
        UpdatePlayerData();
        UpdateEnemiesData();
        UpdateCollectiblesData();
        UpdateSaveData();
        serializationManager.Save("SaveTest", saveData);
    }

    public void CallLoad()
    {
        string path = Application.persistentDataPath + "/SaveTest.save";
        loadData = (SaveData)serializationManager.Load(path);
    }
}
