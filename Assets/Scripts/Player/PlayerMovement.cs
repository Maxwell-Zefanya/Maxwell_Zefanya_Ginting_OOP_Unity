using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // SerializeField supaya bisa disetel langsung di editor
    [SerializeField] Vector2 maxSpeed;
    [SerializeField] Vector2 timeToFullSpeed;
    [SerializeField] Vector2 timeToStop;
    [SerializeField] Vector2 stopClamp;

    Vector2 moveDirection;
    Vector2 moveVelocity;
    Vector2 moveFriction;
    Vector2 stopFriction;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveVelocity = 2*maxSpeed/timeToFullSpeed;
        moveFriction = -2*maxSpeed/(timeToFullSpeed*timeToFullSpeed);
        stopFriction = -2*maxSpeed/(timeToStop*timeToStop);
    }

    public void Move()
    {
        // Input untuk moveDirection
        // Pakai GetAxisRaw supaya input hanya bisa 1 atau 0
        // Akselerasinya dihitung menggunakan rumus
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        // Input hanya bisa tiga nilai, -1, 0, dan 1
        if (Mathf.Abs(moveDirection.x) >= 0.1f) {
            /*  V1 = V0 + at = V0 + (a1+a2)t
                V0 = rb.vel
                a1 = moveDir
                a2 = friction = magnitudenya akan selalu berupa inverse dari moveDir
                kalau move positif, friction akan negatif (move cuma bisa -1, 0, 1)
            */
            moveVelocity.x =
                Mathf.Clamp
                (
                    (moveDirection.x+(moveDirection.x*GetFriction().x))*Time.fixedDeltaTime,
                    -1.0f,
                    1.0f
                );
        } else {
            /*  V1 = V0 - at
                V0 = rb.vel
                a  = friction = magnitudenya akan selalu berupa inverse dari rb.vel.x
                kalau rb.vel positif, friction akan negatif (move cuma bisa -1, 0, 1)
            */

            /* Conditional untuk logika perhitungan friction
                Pakai conditional karena rb.vel tidak selalu -1, 0, 1 (tidak seperti move input)
            */
            if(rb.velocity.x > 0.0f) {
                moveVelocity.x =
                    Mathf.Clamp
                    (
                        (GetFriction().x)*Time.fixedDeltaTime,
                        // Untuk positif, clamp untuk vel adalah -1 s.d. 0, vice versa
                        -1.0f,
                        0.0f
                    );
            } else {
                moveVelocity.x =
                    Mathf.Clamp
                    (
                        (-1.0f*GetFriction().x)*Time.fixedDeltaTime,
                        0.0f,
                        1.0f
                    );
            }
        }

        // Kode untuk input y dan input x dipisah supaya perhitungannya independen satu dengan yang lain
        // (Kondisial satu ga memengaruhi kedua-dua perhitungan)
        if (Mathf.Abs(moveDirection.y) > 0.1f){ 
            moveVelocity.y =
                Mathf.Clamp
                (
                    (moveDirection.y+(moveDirection.y*GetFriction().y))*Time.fixedDeltaTime,
                    -1.0f,
                    1.0f
                );
        } else {
            if(rb.velocity.y > 0.0f) {
                moveVelocity.y =
                    Mathf.Clamp
                    (
                        (GetFriction().y)*Time.fixedDeltaTime,
                        -1.0f,
                        0.0f
                    );
            } else {
                moveVelocity.y =
                    Mathf.Clamp
                    (
                        (-1.0f*GetFriction().y)*Time.fixedDeltaTime,
                        0.0f,
                        1.0f
                    );
            }
        }

        // Bila velocity dibawah stopClamp, maka velocity dijadikan 0
        // Perlu juga dipastikan bahwa tidak ada input move apapun
        if( Mathf.Abs(rb.velocity.x) < stopClamp.x &&
            Mathf.Abs(rb.velocity.y) < stopClamp.y &&
            Mathf.Abs(moveDirection.magnitude) < 0.1f
        )
        {
            rb.velocity = Vector2.zero;
            moveVelocity = Vector2.zero;
        }

        rb.velocity = new Vector2(
            // Set velocity x baru
            Mathf.Clamp
            (
                // V0 + at
                (rb.velocity.x+moveVelocity.x),
                maxSpeed.x*-1.0f,
                maxSpeed.x
            ),
            
            // Set velocity y baru
            Mathf.Clamp
            (
                (rb.velocity.y+moveVelocity.y),
                maxSpeed.y*-1.0f,
                maxSpeed.y
            )
        );


        // Debug.Log("rb: "+rb.velocity.ToString());
        // Debug.Log("md: "+moveDirection.ToString());
        // Debug.Log("mv: "+moveVelocity.ToString());
        // Debug.Log("rb: "+rb.velocity.magnitude + " (mag)");
        // Debug.Log("md: "+moveDirection.magnitude +" (mag)");
    }

    public Vector2 GetFriction()
    {
        if (Mathf.Abs(moveDirection.magnitude) > 0.1f) {
            return moveFriction;
        } else {
            return stopFriction;
        }
    }

    public void MoveBound()
    {

    }

    public bool IsMoving()
    {
        // Vector2.zero --> zero static property dari Vector2
        // Kalau movedir == (0,0)
        if(moveDirection.Equals(Vector2.zero)) {
            return false;
        } else {
            return true;
        }
    }
}
