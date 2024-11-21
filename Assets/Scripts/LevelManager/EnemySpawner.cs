using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public Enemy spawnedEnemy;
    [SerializeField] private int minimumKillsToIncreaseSpawnCount = 3;      // Berapa kill sebelum spawnCount tiap wave nambah
    public int totalKill = 0;               // Total enemy killed (jenis ini)
    private int totalKillWave = 0;          // Enemy killed dalam wave (selalu dibawah spawnCount?)
    [SerializeField] private float spawnInterval = 3f;      // Delay antar spawn
    [Header("Spawned Enemies Counter")]
    public int spawnCount = 0;              // Active enemy di scene?
    public int defaultSpawnCount = 1;       // Jumlah enemy yang dispawn awalnya
    public int spawnCountMultiplier = 1;        // Menambah spawnCount per-wave
    public int multiplierIncreaseCount = 1;     // Menambah spawnCountMultiplier per-wave (additive)
    public CombatManager combatManager;
    public bool isSpawning;

    float timer = 0f;
    int localCount = 0;
    int holdDestroyed = 0;
    void FixedUpdate()
    {
        if(isSpawning == true && timer >= spawnInterval) {
            Instantiate(spawnedEnemy).gameObject.transform.SetParent(gameObject.transform);
            spawnCount--;
            combatManager.totalEnemies++;
            localCount = gameObject.transform.childCount;
            timer = 0f;
            if(spawnCount == 0) {
                isSpawning = false;
            }
        }
        if(gameObject.transform.childCount < localCount) {
            totalKillWave++;
            if(!isSpawning) {
                if(holdDestroyed == 0) {
                    combatManager.totalEnemies--;
                    Debug.Log(gameObject.name+" Destroyed after every entity is loaded");
                } else {
                    if(localCount == defaultSpawnCount-holdDestroyed) {
                        combatManager.totalEnemies--;
                    }
                    combatManager.totalEnemies -= holdDestroyed;
                    holdDestroyed = 0;
                    Debug.Log(gameObject.name+" Destroyed before every entity is loaded");
                }
            } else {
                holdDestroyed++;
                Debug.Log("Hold: "+holdDestroyed);
            }
            localCount--;
            Debug.Log(gameObject.name+" wave kill: "+totalKillWave);
        }
        timer += Time.fixedDeltaTime;
    }

    public void SpawnEnemy() {
        isSpawning = true;
    }

    public void NextWave() {
        if(combatManager.waveNumber != 1 && totalKill%minimumKillsToIncreaseSpawnCount == 0) {
            spawnCountMultiplier += multiplierIncreaseCount;
            defaultSpawnCount *= spawnCountMultiplier;
        }
        spawnCount = defaultSpawnCount;
        totalKill += totalKillWave;
        totalKillWave = 0;
    }
}
