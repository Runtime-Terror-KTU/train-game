using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject playerObj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
