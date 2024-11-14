using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHorizontal : Enemy
{
    bool spawnLeft;
    void Start()
    {
        respawnTime = 3.0f;
    }
    void FixedUpdate()
    {
        if(timer > respawnTime) {
            float rand = Random.Range(-1.0f, 1.0f);
            if(rand < 0.0f) {
                // Spawn di kiri
                spawnLeft = true;
            } else {
                // Spawn di kanan
                spawnLeft = false;
            }
        }
    }
}
