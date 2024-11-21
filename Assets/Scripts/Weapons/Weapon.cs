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
        // Startup --> Inisialisasi timer, objectpool, dll.
        timer = 0.0f;
        objectPool = new ObjectPool<Bullet>
            (CreateInstance, TakeFromPool, ReturnToPool, DestroyPool, collectionCheck, defaultCapacity, maxSize);
        bulletSpawnPoint = GetComponent<Transform>();
        parentTransform = GetComponent<Transform>();
        bulletSpawnPoint.transform.parent = parentTransform.transform;
    }

    // Pakai fixedUpdate biar lebih konsisten
    // Shootinterval jangan dibawah <.1s 
    void FixedUpdate() {
        // Timer dipake buat hitung delay dari last shot
        if(timer > shootIntervalInSeconds && objectPool != null) {
            // Refer to CreateInstance() & TakeFromPool
            Bullet newBullet = objectPool.Get();
            if (newBullet == null) return;
            // Fire weapon & reset timer
            newBullet.transform.SetPositionAndRotation(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            newBullet.Fire();
            timer = 0.0f;
        } 
        timer += Time.fixedDeltaTime;
    }

    // Get()?
    Bullet CreateInstance() {
        // Clone instance bullet yang ada di Prefab
        Bullet poolBullet = Instantiate(bullet);
        poolBullet.transform.parent = bulletSpawnPoint.transform;
        poolBullet.transform.position = bulletSpawnPoint.transform.position;
        // Masukin object bullet kedalam pool
        poolBullet.ObjectPool = objectPool;
        return poolBullet;
    }

    // Get() juga kayanya
    void TakeFromPool(Bullet bullet) {
        // Aktivasi objek
        bullet.transform.parent = bulletSpawnPoint.transform;
        bullet.transform.position = bulletSpawnPoint.transform.position;
        bullet.gameObject.SetActive(true);
    }

    // Return()?
    void ReturnToPool(Bullet bullet) {
        // Kembaliin objek ke posisi awal (spawnpoint)
        bullet.transform.parent = bulletSpawnPoint.transform;
        bullet.gameObject.transform.position = bulletSpawnPoint.transform.position;
        // Deaktivasi objek
        bullet.gameObject.SetActive(false);
    }

    // ???
    void DestroyPool(Bullet bullet) {
        Destroy(bullet.gameObject);
    }
}