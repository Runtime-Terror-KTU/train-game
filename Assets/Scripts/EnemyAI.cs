using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public bool isMelee;
    public bool isRanged;

    public Transform player;
    public float maxRadius = 7;
    public float maxFov = 45;
    public float reach = 2;

    public bool isMoving = false;
    public bool isInFov = false;
    public bool isInRange = false;

    UnityEngine.AI.NavMeshAgent myNavMesh;

    public float checkRate = 0.01f;
    float nextCehck;
    private float time;

    // Paskutinė žinoma žaidėjo pozicija
    Vector3 lastPos;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player").activeInHierarchy)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        myNavMesh = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        myNavMesh.speed = GetComponent<Enemy>().MovementSpeed;

        if(isMelee)
        {
            time = GetComponent<Enemy>().MeleeSpeed;
        }
        
        if(isRanged)
        {
            time = GetComponent<Enemy>().RangeSpeed;
            reach = maxRadius-0.5f;
        }

        lastPos = transform.position;
    }


    void Update()
    {
        if(isMelee && isRanged)
        {
            isMelee = false;
        }

        if (Mathf.Round(myNavMesh.remainingDistance) <= 0)
        {
            isMoving = false;
            myNavMesh.isStopped = true;
        }
        else
        {
            isMoving = true;
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        isInFov = inFOV(transform, player, maxFov, maxRadius);
        isInRange = inRange(transform, out hit);
        
        if(isMelee)
        {
            MeleeAttack(isInFov,isInRange,hit);
        }

        if(isRanged)
        {
            RangedAttack(isInFov,isInRange,hit);
        }

        
    }

    void FacePlayer()
    {
        myNavMesh.transform.LookAt(player);
    }

    void GoToPosition(Vector3 position)
    {
        FacePlayer();
        myNavMesh.destination = position;
        myNavMesh.isStopped = false;
    }

    void FollowPlayer()
    {
        myNavMesh.isStopped = false;
        FacePlayer();
        myNavMesh.destination = player.position;
    }

    void StopFollowing()
    {
        myNavMesh.isStopped = true;
        isMoving = false;
    }


    public static bool inFOV(Transform checkingObject, Transform target, float maxFov, float maxRadius)
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        for (int i = 0; i < count; i++)
        {
            if (overlaps[i] != null)
            {
                Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                directionBetween.y *= 0;

                float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                if (angle <= maxFov)
                {
                    Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, maxRadius))
                    {
                        if (hit.transform == target.transform)
                            return true;
                    }
                }

            }

        }

        return false;
    }

    public bool inRange(Transform checkingObject, out RaycastHit hit)
    {
        Ray ray = new Ray(checkingObject.position, checkingObject.forward);

        if(Physics.Raycast(ray, out hit, reach, -1, QueryTriggerInteraction.Collide))
        {
            if(hit.collider.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }

    private void MeleeAttack(bool isInFov, bool isInRange, RaycastHit hit)
    {
        if (isInFov)
        {
            isMoving = true;

            if (Time.time > nextCehck)
            {
                nextCehck = Time.time + checkRate;
                if (!isInRange)
                {
                    FollowPlayer();
                }
            }

            if (isInRange)
            {
                StopFollowing();
                time -= Time.deltaTime;
                if (time <= 0)
                {
                    hit.collider.GetComponent<Player>().TakeDamage(GetComponent<Enemy>().MeleeDamage);
                    time = GetComponent<Enemy>().MeleeSpeed;
                }
            }
            else
            {
                time = GetComponent<Enemy>().MeleeSpeed;
            }
        }
        else
        {
            isMoving = false;
        }
    }

    private void RangedAttack(bool isInFov, bool isInRange, RaycastHit hit)
    {
        if(isInFov)
        {
            if(isInRange)
            {
                //Atsisuka į žaidėja
                FacePlayer();
                lastPos = player.position;

                time -= Time.deltaTime;
                if(time <=0 )
                {
                    // Nusprendžia ar pataikyti šį šūvį į žaidėja ar ne
                    if((Random.Range(1,11) % 2) == 0)
                    {
                        hit.collider.GetComponent<Player>().TakeDamage(GetComponent<Enemy>().RangeDamage);
                    }

                    time = GetComponent<Enemy>().RangeSpeed;
                }
            }
            else
            {
                GoToPosition(lastPos);
                time = GetComponent<Enemy>().RangeSpeed;
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxFov, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxFov, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        if (!isInFov)
            Gizmos.color = Color.blue;
        else
            Gizmos.color = Color.green;

        Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * reach);
    }
}
