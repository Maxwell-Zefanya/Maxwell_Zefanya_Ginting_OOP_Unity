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

    // Temporary border clamp method
    // Hardcode of the century
    // Ganti metode lain (kalo sempet)
    [SerializeField] Vector2 borderClampVer;
    [SerializeField] Vector2 borderClampHor;
    // BoxCollider2D border;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Lock rotation
        rb.freezeRotation = true;
        moveVelocity = 2*maxSpeed/timeToFullSpeed;
        moveFriction = -2*maxSpeed/(timeToFullSpeed*timeToFullSpeed);
        stopFriction = -2*maxSpeed/(timeToStop*timeToStop);

        // Coba-coba supaya non hardcoded
        // border = GameObject.Find("/MapBorder").GetComponent<BoxCollider2D>();
        // borderClamp = new Vector2(
        //     BoxCollider2D.size.x/2.0f,
        //     BoxCollider2D.size.y/2.0f
        // );
    }

    public void Move()
    {
        // 1. Get Input!
        // Input untuk moveDirection
        // Pakai GetAxisRaw supaya input hanya bisa 1 atau 0
        // Akselerasinya dihitung menggunakan rumus
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        // 2. Cari v yang akan ditambahkan kedalam rigidbody Player
        // Input hanya bisa tiga nilai, -1, 0, dan 1
        if (moveDirection.x != 0.0f) {
            /*  V1 = V0 + at = V0 + (a1+a2)t
                V0 = rb.vel
                a1 = moveDir
                a2 = friction = magnitudenya akan selalu berupa inverse dari moveDir
                kalau move positif, friction akan negatif (move cuma bisa -1, 0, 1),
                jadi bisa dikali langsung
                Perhitungan menggunakan moveFriction
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
                kalau rb.vel positif, friction akan negatif
            */

            /* Conditional untuk logika perhitungan friction
                Pakai conditional karena rb.vel tidak selalu -1, 0, 1 (tidak seperti move input)
                Perhitungan menggunakan stopFriction
            */
            if(rb.velocity.x > 0.0f) {
            // Rumus saat velocity positif
                moveVelocity.x =
                    Mathf.Clamp
                    (
                        (-1.0f*GetFriction().x)*Time.fixedDeltaTime,
                        // Untuk kecepatan player positif, clamp untuk vel adalah 0 s.d. 1, vice versa
                        0.0f,
                        1.0f
                    );
            } else {
            // Rumus saat velocity negatif
                moveVelocity.x =
                    Mathf.Clamp
                    (
                        (GetFriction().x)*Time.fixedDeltaTime,
                        -1.0f,
                        0.0f
                    );
            }
        }

        // Kode untuk input y dan input x dipisah supaya perhitungannya independen satu dengan yang lain
        // (Kondisial satu ga memengaruhi kedua-dua perhitungan)
        if (moveDirection.y != 0.0f){ 
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
                        (-1.0f*GetFriction().y)*Time.fixedDeltaTime,
                        0.0f,
                        1.0f
                    );
            } else {
                moveVelocity.y =
                    Mathf.Clamp
                    (
                        (GetFriction().y)*Time.fixedDeltaTime,
                        -1.0f,
                        0.0f
                    );
            }
        }

        // 3. Perhitungan tambahan supaya gerakan menjadi lebih baik
        /* Karena dalam perhitungan moveVel belum cover saat kasus
            rb.vel = 0, maka pesawat akan terus memberikan force
            walaupun komponennya 0.
            Misalnya saat rb.vel.x = 0, maka akan tetap diberikan
            Gaya friction, dimana rb.vel jadi berubah (Seharusnya 0)
        */
        if(rb.velocity.x == 0.0f && moveDirection.x == 0.0f) {
            moveVelocity.x = 0;
        }
        if(rb.velocity.y == 0.0f && moveDirection.y == 0.0f) {
            moveVelocity.y = 0;
        }


        // 4. Clamping velocity saat Player sudah cukup lambat
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

        // 5. Set nilai Player yang baru
        rb.velocity = new Vector2(
            // Set velocity x baru
            Mathf.Clamp
            (
                // V0 + at
                // Dikali -1 karena kebalik (W jadi kebawah etc.)
                (rb.velocity.x-moveVelocity.x),
                maxSpeed.x*-1.0f,
                maxSpeed.x
            ),
            
            // Set velocity y baru
            Mathf.Clamp
            (
                (rb.velocity.y-moveVelocity.y),
                maxSpeed.y*-1.0f,
                maxSpeed.y
            )
        );

        // 6. Clamp posisi player supaya tetap didalam kamera
        MoveBound();

        // Extra Debugging
        // Debug.Log("rb: "+rb.velocity.ToString());
        // Debug.Log("md: "+moveDirection.ToString());
        // Debug.Log("mv: "+moveVelocity.ToString());
        // Debug.Log("fx: "+ (GetFriction().x)*Time.fixedDeltaTime);
        // Debug.Log("fy: "+ (GetFriction().y)*Time.fixedDeltaTime);
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
        // Bound position to map boundary
        rb.position = new Vector2(
            Mathf.Clamp
            (
                rb.position.x,
                borderClampHor.x,
                borderClampHor.y
            ),
            Mathf.Clamp
            (
                rb.position.y,
                borderClampVer.x,
                borderClampVer.y
            )
        );
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
