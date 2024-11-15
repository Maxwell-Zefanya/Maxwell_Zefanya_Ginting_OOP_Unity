using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Cuma buat holding values doang
    protected Rigidbody2D rb;
    [SerializeField] protected float velocity;
    public int health;
    protected float ySpawn;
    protected float xSpawn;
    protected bool isSpawned = false;

    protected virtual void SpawnEnemy() {
        Debug.Log("Base SpawnEnemy() called, needs fix");
    }
}
