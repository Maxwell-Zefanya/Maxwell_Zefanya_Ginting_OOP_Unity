using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour
{
    public Health entityHealth;
    void Awake()
    {
        entityHealth = GetComponent<Health>();
        Debug.Log("Health component for "+gameObject.name);
    }

    public void Damage(Bullet bullet) {
        entityHealth.Subtract(bullet.damage);
    }

    public void Damage(int damage) {
        entityHealth.Subtract(damage);
    }
}
