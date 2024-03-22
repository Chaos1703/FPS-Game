using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class HealthScript : MonoBehaviour
{
    private EnemyAnimations enemyAnim;
    private NavMeshAgent navAgent;
    private EnemyController enemyController;
    public float health;
    public bool isPlayer, isBoar , isCannibal;
    private bool isDead;
    private EnemyAudio enemyAudio;
    private PlayerStats playerStats;

    void Awake()
    {
        if(isBoar || isCannibal)
        {
            enemyAnim = GetComponent<EnemyAnimations>();
            enemyController = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }
        if(isPlayer)
        {
            playerStats =  GetComponent<PlayerStats>();
        }
    }

    public void ApplyDamage(int damage){
        if(isDead)
            return;
        health -= damage;   
        if(isPlayer)
        {
            playerStats.DisplayHealthStats(health);
        }

        if(isBoar || isCannibal)
        {
            if(enemyController.EnemyState == EnemyState.Patrol)
            {
                enemyController.chaseDistance = 50f;
            }
        }

        if(health <= 0)
        {
            PlayerDied();
            isDead = true;
        }
    }

    void PlayerDied()
    {
        if(isCannibal)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 0f);
            enemyController.enabled = false;
            navAgent.enabled = false;
            enemyAnim.enabled = false;
            StartCoroutine(DeadSound());
            EnemyManager.instance.EnemyDied(true);
        }
        if(isBoar)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemyController.enabled = false;
            enemyAnim.Dead();
            StartCoroutine(DeadSound());
            EnemyManager.instance.EnemyDied(false);
        }

        if(isPlayer){
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for(int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }
            GetComponent<PlayerMovementScript>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);
            EnemyManager.instance.StopSpawning();

        }

        if(tag == "Player")
        {
            Invoke("RestartGame", 3f);
        }
        else
        {
            Invoke("TurnOffGameObject", 3f);
        }
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.PlayDieSound();
    }
}
