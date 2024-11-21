using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeting : Enemy
{
    void Start() {
        // Inisialisasi awal
        gameObject.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        SpawnEnemy();
    }

    protected override void SpawnEnemy() {
        isSpawned = true;
        // Spawnpoint jadi gabungan dari enemyHor & enemyVer
        // Enemy spawnpoint Y dibatasi jadi 1.0-3.0f supaya ga spawn terlalu dekat ke player
        ySpawn = Random.Range(1.0f, 3.75f);
        if(ySpawn < 2.375f) {
            // Spawn di kiri & diatas Player
            rb.position = new Vector2(-9.0f, ySpawn);
        } else {
            // Spawn di kanan & diatas Player
            rb.position = new Vector2(9.0f, ySpawn);
        }
    }

    void FixedUpdate()
    {
        // Bila enemy belum menjadi spawned, maka spawn dulu
        if(!isSpawned) {
            SpawnEnemy();
            Debug.Log("Forgot to spawn, SpawnEnemy() called");
        } else {
            // Bila health musuh masih diatas 0
            if(health > 0) {
                // Collider player beda sama posisi transform asli
                Vector2 playerActual = Player.Instance.transform.position;
                playerActual.y += 0.5f;
                // Interpolate posisi musuh ke posisi Player
                Vector2 newPos = Vector2.MoveTowards(rb.position, playerActual, velocity*Time.fixedDeltaTime);
                rb.position = new Vector2(newPos.x, newPos.y);
            } else {

            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Mengecek bila collider dari "other" di parameter memiliki tag player
        if (other.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
}
