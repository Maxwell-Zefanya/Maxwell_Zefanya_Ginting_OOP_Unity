using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Stats")]
    public float bulletSpeed;
    public int damage;
    private Rigidbody2D rb;

    IObjectPool<Bullet> objectPool;
    public IObjectPool<Bullet> ObjectPool {get => objectPool; set => objectPool = value;}

    IEnumerator FireRoutine() {
        yield return new WaitForSeconds(5.0f);
        // Kembalikan bullet kedalam pool
        // Refer to ReturnToPool(Bullet bullet)
        objectPool.Release(this);
    }

    public void Fire() {
        // Aplikasi speed kedalam bullet
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, bulletSpeed);
        StartCoroutine(FireRoutine());
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     // objectPool.Release(this);
    //     Debug.Log("Bullet collided");
    // }
}
