using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public GameObject train;
    public string nextLevel = "TempScene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            train.GetComponent<TrainController>().ChangeLevel(nextLevel);
        }
    }
}
