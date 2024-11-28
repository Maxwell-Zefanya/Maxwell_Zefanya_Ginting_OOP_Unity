using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUIManager : MonoBehaviour
{
    CombatManager combatManager;
    Health player;
    Label health;
    Label point;
    Label wave;
    Label enemies;
    public int points = 0;
    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = gameObject.GetComponent<UIDocument>().rootVisualElement;

        combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        player = GameObject.Find("Player").GetComponent<Health>();

        health = root.Q<Label>("Health");
        point = root.Q<Label>("Point");
        wave = root.Q<Label>("Wave");
        enemies = root.Q<Label>("Enemy");
    }

    void FixedUpdate() {
        health.text = player.health.ToString();
        wave.text = combatManager.waveNumber.ToString();
        point.text = points.ToString();
        enemies.text = combatManager.totalEnemies.ToString();
    }
}
