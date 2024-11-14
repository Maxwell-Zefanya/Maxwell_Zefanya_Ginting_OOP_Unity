using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private float shootIntervalInSeconds = 3f;

    [Header("Bullets")]
    public Bullet bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    
    [Header("Bullet Pool")]
    private IObjectPool<Bullet> objectPool;


    private readonly bool collectionCheck = false;
    private readonly int defaultCapacity = 30;
    private readonly int maxSize = 100;
    private float timer;
    public Transform parentTransform;

    void Awake()
    {
        timer = 0.0f;
        objectPool = new ObjectPool<Bullet>
            (CreateInstance, TakeFromPool, ReturnToPool, DestroyPool, collectionCheck, defaultCapacity, maxSize);
        bulletSpawnPoint = GetComponent<Transform>();
        parentTransform = GetComponent<Transform>();
        bulletSpawnPoint.transform.parent = parentTransform.transform;
    }

    void FixedUpdate() {
        if(timer > shootIntervalInSeconds && objectPool != null) {
            Bullet newBullet = objectPool.Get();
            // Debug.Log("newBullet is "+newBullet.gameObject.name);
            // Debug.Log("newBullet active? "+newBullet.gameObject.activeInHierarchy);
            // Debug.Log("Pool obj avail: " + objectPool.CountInactive);
            if (newBullet == null) return;
            newBullet.Fire();
            timer = 0.0f;
        } 
        timer += Time.fixedDeltaTime;
        // Debug.Log("Timer: "+ timer);
    }

    // Get()
    Bullet CreateInstance() {
        Bullet poolBullet = Instantiate(bullet, bulletSpawnPoint.transform);
        poolBullet.ObjectPool = objectPool;
        // Debug.Log("poolBullet "+poolBullet.gameObject.name+" instantiated");
        return poolBullet;
    }

    // 
    void TakeFromPool(Bullet bullet) {
        bullet.gameObject.SetActive(true);
    }

    // Return()
    void ReturnToPool(Bullet bullet) {
        bullet.gameObject.SetActive(false);
    }

    void DestroyPool(Bullet bullet) {
        Destroy(bullet.gameObject);
    }
}