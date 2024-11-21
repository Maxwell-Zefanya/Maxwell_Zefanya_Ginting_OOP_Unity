using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour
{
    public Health entityHealth;
    Collider2D sr;
    private Invicibility invincibilityComponent;

    void Start()
    {
        sr = GetComponent<Collider2D>();
        invincibilityComponent = GetComponent<Invicibility>();
    }

    public void Damage(Bullet bullet)
    {
        if (invincibilityComponent != null && invincibilityComponent.isInvincible) return;

        if (entityHealth != null)
            entityHealth.Subtract(bullet.damage);
    }

    public void Damage(int damage)
    {
        if (invincibilityComponent != null && invincibilityComponent.isInvincible) return;

        if (entityHealth != null)
            entityHealth.Subtract(damage);
    }
}
