using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TrainController : MonoBehaviour
{
    public Transform targetBeginning;
    public Transform targetEnding;
    public Transform trigger;
    public float speed = 10f;
    private int enemyCount;
    public bool endLevel;

    private void Start()
    {
        endLevel = false;
    }

    private void Update()
    {
        UpdateEnemyCount();
        if (!endLevel)
            MoveTo(targetBeginning);
        else
            MoveTo(targetEnding);
    }

    //Moves to a target at a certain speed
    public void MoveTo(Transform target)
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 0.001f)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
    }

    //Makes train drive away and changes to specified level
    public void ChangeLevel(string nextScene)
    {
        //Here we will be able to check if we have everything to proceed.
        if(enemyCount == 0)
            StartCoroutine(EndLevel(nextScene));
    }

    IEnumerator EndLevel(string nextScene)
    {
        yield return new WaitForSeconds(1);
        endLevel = true;
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene(nextScene);
    }

    public void UpdateEnemyCount()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
