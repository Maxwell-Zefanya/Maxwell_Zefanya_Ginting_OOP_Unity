using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set; }
    public static LevelManager levelManager {get; private set; }

    // Update is called once per frame
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        //
        Instance = this;
        levelManager = GetComponentInChildren<LevelManager>();
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameObject.FindWithTag("MainCamera"));
        Debug.Log(GameObject.FindWithTag("MainCamera").ToString());
    }
}
