using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TrainController : MonoBehaviour
{
    public Transform targetBeginning;
    public Transform targetEnding;
    public Transform trigger;
    public Animator anim;
    public AudioSource sound;

    GameObject player;
    public GameObject blocker;

    public float speed = 10f;
    public int enemyCount;
    public bool endLevel;

    private void Start()
    {
        endLevel = false;
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (enemyCount == 0)
        {
            StartCoroutine(EndLevel());
            anim.SetTrigger("Ending");
            blocker.SetActive(true);
            sound.Play();
        }
    }

    IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(1);
        endLevel = true;
    }

    public void UpdateEnemyCount()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
