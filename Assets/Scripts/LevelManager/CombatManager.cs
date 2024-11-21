using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CombatManager : MonoBehaviour
{
    public EnemySpawner[] enemySpawners;
    /*
        [0] Horizontal
        [1] Vertical
        [2] Targeting (tiap 2 wave)
        [3] Boss (tiap 5 wave)
    */
    public float timer = 0;                             // Waktu sebelum berganti Wave
    [SerializeField] private float waveInterval = 5f;   // Waktu sebelum masuk nextWave (jangan diubah?)
    public int waveNumber = 1;
    public int totalEnemies = 0;

    void Start()
    {
        // Cek bila belum ada enemy yang dimasukkan
        Assert.IsTrue(enemySpawners.Length > 0, "Tambahkan setidaknya 1 Prefab EnemySpawner terlebih dahulu!");
    }

    void FixedUpdate()
    {
        if(timer >= waveInterval) {
            waveNumber++;
            foreach(EnemySpawner spawn in enemySpawners) {
                spawn.NextWave();
            }
            if(totalEnemies <= 0) {
                Debug.Log("Attempting to spawn wave "+waveNumber);
                enemySpawners[0].SpawnEnemy();
                if(waveNumber%2 == 0) {                
                    enemySpawners[1].SpawnEnemy();
                }
                if(waveNumber%3 == 0) {
                    enemySpawners[2].SpawnEnemy();
                }
                if(waveNumber%5 == 0) {
                    enemySpawners[3].SpawnEnemy();
                }
            }
            timer = 0f;
        }

        if(totalEnemies <= 0) {
            timer += Time.fixedDeltaTime;
        }
    }
}
