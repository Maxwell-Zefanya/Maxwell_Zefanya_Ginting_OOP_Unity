using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{

    void Start() {
        // Inisialisasi awal
        gameObject.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        SpawnEnemy();
    }

    protected override void SpawnEnemy() {
        isSpawned = true;
        // Enemy spawnpoint Y dibatasi jadi 1.0-3.0f
        // Supaya ga spawn terlalu dekat ke player
        ySpawn = Random.Range(1.0f, 3.0f);
        if(ySpawn < 2.0f) {
            // Spawn di kiri & diatas Player
            rb.position = new Vector2(-9.0f, ySpawn);
            rb.velocity = new Vector2(velocity, 0.0f);
        } else {
            // Spawn di kanan & diatas Player
            rb.position = new Vector2(9.0f, ySpawn);
            rb.velocity = new Vector2(-velocity, 0.0f);
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
                // Kalau musuh ada diluar map, invert posisi (velocity jangan)
                // Cek kiri
                if(rb.position.x < -8.5f) {
                    rb.velocity = new Vector2(velocity, 0.0f);
                } 
                // Cek kanan
                if(rb.position.x > 8.5) {
                    rb.velocity = new Vector2(-velocity, 0.0f);
                }
            } else {

            }
        }
    }
}
