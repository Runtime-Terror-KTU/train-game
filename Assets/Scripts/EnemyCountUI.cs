using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCountUI : MonoBehaviour
{
    public Text enemyCountText;
   
    void Update()
    {
        enemyCountText.text = (GameObject.FindGameObjectsWithTag("Enemy").Length).ToString();
    }

}
