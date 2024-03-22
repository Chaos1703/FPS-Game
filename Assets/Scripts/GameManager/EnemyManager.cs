using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [SerializeField] private GameObject cannibalPrefab , boarPrefab;
    public Transform[] cannibalSpawnPoints , boarSpawnPoints;
    [SerializeField] private int cannibalCount , boarCount;
    private int initialCannibalCount , initialBoarCount;
    public float waitBeforeSpawnTime = 10f;

    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(this);
        }
    }

    void Start()
    {
        initialCannibalCount = cannibalCount;
        initialBoarCount = boarCount;
        SpawnEnemies();
        StartCoroutine("CheckToSpawnEnemies");
    }

    void SpawnEnemies(){
        SpawnCannibals();
        SpawnBoars();
    }

    void SpawnCannibals(){
        int index = 0;
        for (int i = 0; i < cannibalCount; i++)
        {
            if(index >= cannibalSpawnPoints.Length){
                index = 0;
            }
            Instantiate(cannibalPrefab , cannibalSpawnPoints[index].position , Quaternion.identity);
            index++;
        }
        cannibalCount = 0;
    }

    void SpawnBoars(){
        int index = 0;
        for (int i = 0; i < boarCount; i++)
        {
            if(index >= boarSpawnPoints.Length){
                index = 0;
            }
            Instantiate(boarPrefab , boarSpawnPoints[index].position , Quaternion.identity);
            index++;
        }
        boarCount = 0;
    }

    public void EnemyDied(bool cannibal){
        if(cannibal){
            cannibalCount++;
            if(cannibalCount > initialCannibalCount){
                cannibalCount = initialCannibalCount;
            }
        }
        else{
            boarCount++;
            if(boarCount > initialBoarCount){
                boarCount = initialBoarCount;
            }
        }
    }

    IEnumerator CheckToSpawnEnemies(){
        yield return new WaitForSeconds(waitBeforeSpawnTime);
        SpawnEnemies();
        StartCoroutine("CheckToSpawnEnemies");
    }

    public void StopSpawning(){
        StopCoroutine("CheckToSpawnEnemies");
    }
}
