using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Static, ada sejak awal runtime
    public static Player Instance;
    PlayerMovement playerMovement;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        // .Find akan mereturn object yg ada di parameter
        // (pada kasus ini GameObject engineeffect)
        animator = GameObject.Find("/Player/Engine/EngineEffect").GetComponent<Animator>();
    }

    // Dipanggil sekali, yaitu saat game baru startup
    void Awake()
    {
        // Oleh Ray, "Game Design Pattern - Using Singletons in Unity," Medium
        // Membentuk singleton untuk Player
        if (Instance == null){
            Instance = this;
        } else if(Instance != this){
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    // Berfungsi sebagai semacam tickrate untuk kalkulasi apapun
    // Secara default dipanggil 50 kali sekali (setiap 0.02s)
    // Framerate independent
    void FixedUpdate()
    {
        playerMovement.Move();
    }

    // Call rate mengikuti frame
    // Di-call setelah semua update yg lain udh di-call
    void LateUpdate()
    {
        // Panggil method .IsMoving() dari script (class) PlayerMovement
        // Pakai method .SetBool() dari class Animator
        if(playerMovement.IsMoving()){
            animator.SetBool("IsMoving", true);
        } else {
            animator.SetBool("IsMoving", false);
        }
    }
}
