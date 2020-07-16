﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FinalEnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    public Transform moveSpot;
    public bool FreeRoamer;
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    private float waitTime;
    public float startWaitTime;
    private float StoppingDist;
    Transform target;
    NavMeshAgent agent;
    CharacterCombat combat;
    Animator animator;
    Vector3 originalPos;
    [HideInInspector]
    public bool PlayerTargeted;
    [HideInInspector]
    public Transform PlayerCompanionTarget;
    [HideInInspector]
    public bool CompanionTargeted;
    public bool easyRoam;
    public float easyRoamX;
    public float easyRoamZ;
    private float initialRoamPointX;
    private float initialRoamPointZ;
    FinalLevelManager finalManager;
    [HideInInspector]
    public bool reachedTargetPoint;
    [HideInInspector]
    public bool hasDisappeared;
    EffectHandler effects;

    // Start is called before the first frame update
    void Start()
    {
        effects = GetComponent<EffectHandler>();
        finalManager = FinalLevelManager.instance;
        PlayerTargeted = false;
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();
        animator = GetComponentInChildren<Animator>();
        originalPos = transform.position;
        if (FreeRoamer && easyRoam)
        {
            initialRoamPointX = moveSpot.transform.position.x;
            initialRoamPointZ = moveSpot.transform.position.z;
        }
        waitTime = startWaitTime;
        if (moveSpot != null)
        {
            if (!easyRoam)
            {
                moveSpot.position = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
            }
            else
            {
                moveSpot.position = new Vector3(Random.Range(moveSpot.position.x - easyRoamX, moveSpot.position.x + easyRoamX), transform.position.y, Random.Range(moveSpot.position.z - easyRoamZ, moveSpot.position.z + easyRoamZ));
            }
        }
        StoppingDist = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (finalManager.AllSpawnerDestroyed)
        {
            agent.SetDestination(finalManager.TargetPoint.position);
            float distance = Vector3.Distance(transform.position, finalManager.TargetPoint.position);
            if (distance <= 2.1 && !reachedTargetPoint)
            {
                reachedTargetPoint = true;
                StartCoroutine(Disappear());
                effects.EnemiesToTargetPointEffectEvent(7, 3f);
                
            }
        }
        else
        {

            if (CompanionTargeted)
            {
                target = PlayerCompanionTarget;
            }
            else
            {
                target = PlayerManager.instance.player.transform;
            }


            float distance = Vector3.Distance(target.position, transform.position);

            if (!combat.dead)
            {
                if (distance <= agent.stoppingDistance)
                {
                    agent.stoppingDistance = StoppingDist;
                    CharacterStats targetStats = target.GetComponent<CharacterStats>();
                    if (targetStats != null)
                    {
                        combat.Attack(targetStats);
                        FaceTarget();
                        if (targetStats.currentHealth <= 0)
                        {

                            CompanionTargeted = false;
                        }

                    }
                }
                else if (distance <= lookRadius)
                {
                    agent.stoppingDistance = StoppingDist;
                    agent.SetDestination(target.position);
                    DecideOnChasing();
                    PlayerTargeted = true;
                }
                else if (distance > lookRadius)
                {
                    if (FreeRoamer)
                    {
                        agent.stoppingDistance = 0;
                        FreeRoam();

                    }
                    else
                    {
                        agent.stoppingDistance = StoppingDist;
                        agent.SetDestination(originalPos);
                    }
                    PlayerTargeted = false;
                }
            }
        }
    }

    public void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    // Stops or starts chasing player based on whether attack and reaction animation is playing
    public void DecideOnChasing()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punching") || animator.GetCurrentAnimatorStateInfo(0).IsName("Reaction"))
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
    }



    private void FreeRoam()
    {
        agent.SetDestination(moveSpot.position);
        //Debug.Log(Vector3.Distance(transform.position, moveSpot.position));
        if (Vector3.Distance(transform.position, moveSpot.position) < 1.5f)
        {
            if (waitTime <= 0)
            {
                if (!easyRoam)
                {
                    moveSpot.position = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
                    waitTime = startWaitTime;
                }
                else
                {
                    moveSpot.position = new Vector3(Random.Range(initialRoamPointX - easyRoamX, initialRoamPointX + easyRoamX), transform.position.y, Random.Range(initialRoamPointZ - easyRoamZ, initialRoamPointZ + easyRoamZ));
                    waitTime = startWaitTime;
                }


            }
            else
            {
                waitTime -= Time.deltaTime;

            }
        }
    }

    public bool Approximately(float a, float b, float epsilon)
    {
        return (Mathf.Abs(a - b) < epsilon) || (Mathf.Approximately(Mathf.Abs(a - b), epsilon));
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(3f);
        hasDisappeared = true;

        StartCoroutine(Pause());
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
