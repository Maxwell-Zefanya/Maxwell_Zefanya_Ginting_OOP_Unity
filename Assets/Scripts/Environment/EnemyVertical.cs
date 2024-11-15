using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVertical : Enemy
{
    void Start() {
        // Inisialisasi awal
        gameObject.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        SpawnEnemy();
    }

    protected override void SpawnEnemy() {
        isSpawned = true;
        xSpawn = Random.Range(-8.4f, 8.4f);
        rb.position = new Vector2(xSpawn, 5.0f);        // Enemy selalu spawn diatas map
        rb.velocity = new Vector2(0.0f, -velocity);
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
                if(rb.position.y < -5.0f) {
                    rb.position = new Vector2(xSpawn, 5.0f);
                }
            } else {

            }
        }
    }
}
