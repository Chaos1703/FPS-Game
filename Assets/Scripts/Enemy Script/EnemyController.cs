using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState{Patrol , Chase , Attack , Dead};
    
public class EnemyController : MonoBehaviour
{
    private EnemyAnimations enemyAnim;
    private NavMeshAgent navAgent;
    private EnemyState enemyState;
    public float walkSpeed = 0.5f , runSpeed = 4f , chaseDistance = 7f , attackDistance = 1.8f , chaseAfterAttackDistance = 2f , patrolRadiusMin = 20f , patrolRadiusMax = 60f , patrolForThisTime = 15f , waitBeforeAttack = 2f;
    private float currentChaseDistance , patrolTimer , attackTimer;
    private Transform target;
    public GameObject attackPoint;
    private EnemyAudio enemyAudio;

    void Awake()
    {
        enemyAnim = GetComponent<EnemyAnimations>();
        navAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player").transform;
        enemyAudio = GetComponentInChildren<EnemyAudio>();
    }

    void Start()
    {
        enemyState = EnemyState.Patrol;
        patrolTimer = patrolForThisTime;
        attackTimer = waitBeforeAttack;
        currentChaseDistance = chaseDistance;
    }

    void Update()
    {
        if(enemyState == EnemyState.Patrol)
        {
            Patrol();
        }
        if(enemyState == EnemyState.Chase)
        {
            Chase();
        }
        if(enemyState == EnemyState.Attack)
        {
            Attack();
        }
    }

    void Patrol(){
        navAgent.isStopped = false;
        navAgent.speed = walkSpeed;
        patrolTimer += Time.deltaTime;
        if(patrolTimer > patrolForThisTime || navAgent.remainingDistance < 1f)
        {
            SetNewRandomDestination();
            patrolTimer = 0f;
        }
        if(navAgent.velocity.sqrMagnitude > 0)
        {
            enemyAnim.walk(true);
        }
        else
        {
            enemyAnim.walk(false);
        }

        if(Vector3.Distance(transform.position , target.position) <= chaseDistance)
        {
            enemyAnim.walk(false);
            enemyState = EnemyState.Chase;
            enemyAudio.PlayScreamSound();
        }
    }

    void Chase(){
        navAgent.isStopped = false;
        navAgent.speed = runSpeed;
        navAgent.SetDestination(target.position);
        if(navAgent.velocity.sqrMagnitude > 0)
        {
            enemyAnim.run(true);
        }
        else
        {
            enemyAnim.run(false);
        }

        if(Vector3.Distance(transform.position , target.position) <= attackDistance)
        {
            enemyAnim.run(false);
            enemyAnim.walk(false);
            enemyState = EnemyState.Attack;
            if(chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }
        }
        else if(Vector3.Distance(transform.position , target.position) > chaseDistance)
        {
            enemyAnim.run(false);
            enemyState = EnemyState.Patrol;
            patrolTimer = patrolForThisTime;
            if(chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }
        }
    }

    void Attack(){
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;
        attackTimer += Time.deltaTime;
        if(attackTimer > waitBeforeAttack)
        {
            enemyAnim.attack();
            attackTimer = 0f;
            
        }
        if(Vector3.Distance(transform.position , target.position) > attackDistance + chaseAfterAttackDistance)
        {
            enemyState = EnemyState.Chase;
        }
    }

    void SetNewRandomDestination(){
        float randomRadius = Random.Range(patrolRadiusMin , patrolRadiusMax);
        Vector3 randomDirection = Random.insideUnitSphere * randomRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection , out navHit , randomRadius , -1);
        navAgent.SetDestination(navHit.position);
    }

    void TurnOnAttackPoint()
    {
        attackPoint.SetActive(true);
    }

    void TurnOffAttackPoint()
    {
        if (attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(false);
        }
    }

    public EnemyState EnemyState
    {
        get; set;
    }
}
