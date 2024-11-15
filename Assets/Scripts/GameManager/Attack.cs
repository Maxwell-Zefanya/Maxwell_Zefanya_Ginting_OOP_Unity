using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Attack : MonoBehaviour
{
    Hitbox otherHitbox;
    public Bullet bullet;
    public int damage;
    
    void Awake()
    {
        otherHitbox = null;    
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        // Ambil hitbox yang di-hit
        otherHitbox = other.gameObject.GetComponent<Hitbox>();

        // Kalau hit body
        if(
            gameObject.CompareTag("Player") != other.gameObject.CompareTag("Player") &&
            gameObject.CompareTag("Enemy") != other.gameObject.CompareTag("Enemy")
            )
        {
            if(otherHitbox != null) {
                Debug.Log("Hit!");
                // Apply collision damage
                otherHitbox.Damage(damage);
            }
        }

        // Kalau bullet hit dengan yang tag beda
        if(
            gameObject.CompareTag("PlayerBullet") != other.gameObject.CompareTag("Player") &&
            gameObject.CompareTag("Enemy") != other.gameObject.CompareTag("Enemy")
            )
        {
            if(otherHitbox != null) {
                // Kalau ada bullet (boss), apply bullet damage
                if (bullet != null)
                {
                    otherHitbox.Damage(bullet);
                }
            }
        }
    }
}
