using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rotateSpeed;
    Vector2 newPosition;

    Vector2 changeVelocity;
    Rigidbody2D rb;
    SpriteRenderer sp;
    CircleCollider2D col;
    int i = 0;
    Weapon weapon;          // Untuk mengecek apakah player memiliki weapon
    // Start is called before the first frame update
    void Start()
    {
        // Render asteroid a
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        sp = GetComponent<SpriteRenderer>();

        // Player tidak mungkin punya weapon di awal scene load (seharusnya)
        col.enabled = false;
        sp.enabled = false;

        ChangePosition();
        rb.position = newPosition;
        rb.velocity = new Vector2(
            Random.Range(-1.0f, 1.0f)*speed,
            Random.Range(-1.0f, 1.0f)*speed
        );
    }

    // Update is called once per frame
    void Update()
    {
        weapon = GameObject.Find("/Player").GetComponentInChildren<Weapon>();
        if(weapon != null) {
            sp.enabled = true;
            col.enabled = true;
            ChangePosition();
            changeVelocity = newPosition;
        } else {
            sp.enabled = false;
            col.enabled = false;
        }
        if(i>1000 && Vector2.Distance(newPosition, changeVelocity) < 0.5f) {
            rb.velocity = newPosition*speed;
            i = 0;
        }
        i++;
        // Bound position to map boundary
        rb.position = new Vector2(
            Mathf.Clamp
            (
                rb.position.x,
                -7.8f,
                7.8f
            ),
            Mathf.Clamp
            (
                rb.position.y,
                -4.3f,
                4.3f
            )
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) {
            GameManager.levelManager.LoadScene("Main");
        }
    }

    void ChangePosition() {
        newPosition = new Vector2
        (
            Random.Range(-1.0f, 1.0f),
            Random.Range(-1.0f, 1.0f)
        );
    }
}
