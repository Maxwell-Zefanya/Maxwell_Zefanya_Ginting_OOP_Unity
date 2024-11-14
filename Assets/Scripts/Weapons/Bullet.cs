using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Stats")]
    public float bulletSpeed = 20;
    public int damage = 10;
    private Rigidbody2D rb;

    IObjectPool<Bullet> objectPool;
    public IObjectPool<Bullet> ObjectPool {set => objectPool = value;}
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, bulletSpeed);
    }

    IEnumerator FireRoutine() {
        yield return new WaitForSeconds(5.0f);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, bulletSpeed);
        Debug.Log("Bullet fired");
        objectPool.Release(this);
    }

    public void Fire() {
        StartCoroutine(FireRoutine());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
