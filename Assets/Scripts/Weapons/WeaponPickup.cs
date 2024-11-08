using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon weaponHolder;
    Weapon weapon;

    SpriteRenderer weapon_sr;       // Menyimpan SpriteRenderer weapon yang ada
    Weapon playerWep;                     // Menyimpan weapon yang ada pada Player saat ini
    public bool isPickedUp = false;        // Menyimpan status apakah player sudah mengamil weapon yang sama

    void Awake()
    {
        // Buat clone dari prefab weaponHolder kedalam game
        weapon = Instantiate(weaponHolder, transform.position, transform.rotation);
        weapon_sr = weapon.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (weapon != null) {
            TurnVisual(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ambil senjata yang ada pada Player saat ini (ada atau tidak ada)
        playerWep = other.GetComponentInChildren<Weapon>();
        // Mengecek bila collider dari "other" di parameter memiliki tag player
        if (other.gameObject.CompareTag("Player")){
            // Mengecek bila player sudah punya weapon
            if(playerWep != null) {
                // Mengecek bila Player mengambil weapon yang berbeda
                if(playerWep != weapon) {
                    /* Bila weapon berbeda, maka detach weapon player yang ada,
                        lalu buat weapon player yang di-detach jadi invisible,
                        lalu buat transform weapon yang baru menjadi player transform
                    */
                    TurnVisual(false, playerWep);
                    playerWep.transform.parent = null;
                    Debug.Log("Item changed");
                    // Respawn objek weapon yang ada
                    weapon = Instantiate(weaponHolder, transform.position, transform.rotation);
                } else {
                    Debug.Log("Item already taken");
                }
            }
            // Bila player belum (atau sudah) punya weapon, maka ubah
            // transform weapon (clone) kedalam transform Player
            weapon.transform.SetParent(other.transform);
            weapon.transform.position = new Vector2
                (
                    other.transform.position.x,
                    other.transform.position.y+0.1f
                );
            TurnVisual(true, weapon);
            Debug.Log("Item acquired");
        }
    }

    void TurnVisual(bool on) {
        // Mengatur apakah weapon sprite akan di-render berdasarkan input parameter;
        // "on" adalah parameter
        weapon_sr.enabled = on;
    }

    void TurnVisual(bool on, Weapon weapon) {
        weapon_sr = weapon.GetComponent<SpriteRenderer>();
        weapon_sr.enabled = on;
        // Bila ingin membuat object jadi invisible
        if(!on) {
            Destroy(weapon.gameObject);
        }
    }
}
