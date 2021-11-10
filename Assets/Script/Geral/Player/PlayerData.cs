using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //

public class PlayerData : MonoBehaviour {

    [Header("Components")]
    private Animator animPlayer;
    private PlayerAttack atkScript;

    [Header("Stats")]
    public float maxHealth;
    //[System.NonSerialized]
    public float currentHealth;
    [System.NonSerialized] public bool blockState;

    [Header("Progression")]
    public float normalXP;
    public int normalLv;
    public bool[] normalSkills = new bool[9];
    public float fireXP;
    public int fireLv;
    public bool[] fireSkills = new bool[9];
    public float waterXP;
    public int waterLv;
    public bool[] waterSkills = new bool[9];
    public float plantXP;
    public int plantLv;
    public bool[] plantSkills = new bool[9];
    public float electricXP;
    public int electricLv;
    public bool[] electricSkills = new bool[9];
    public float earthXP;
    public int earthLv;
    public bool[] earthSkills = new bool[9];
    public float poisonXP;
    public int poisonLv;
    public bool[] poisonSkills = new bool[9];

    public Material[] colorMaterial;

    private void Start() {
        animPlayer = GetComponent<Animator>();
        atkScript = GetComponent<PlayerAttack>();
        currentHealth = maxHealth;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) unlockAll();
    }

    //private void FixedUpdate() {
    //    if (currentHealth < maxHealth) { //TO REMOVE
    //        currentHealth += maxHealth * Time.fixedDeltaTime / 60;
    //        if (currentHealth > maxHealth) currentHealth = maxHealth;
    //    }
    //}

    public void TakeDamage(float damage) {
        if (blockState) currentHealth = currentHealth - 0;
        else currentHealth -= damage;
        animPlayer.SetBool("Consuming", false);
        atkScript.StopAllCoroutines();

        if (currentHealth <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void takeHealing(float heal) {
        currentHealth += heal;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    void unlockAll() {
        for (int i = 0; i < 9; i++) normalSkills[i] = true;
        for (int i = 0; i < 9; i++) fireSkills[i] = true;
        for (int i = 0; i < 9; i++) waterSkills[i] = true;
        for (int i = 0; i < 9; i++) plantSkills[i] = true;
        for (int i = 0; i < 9; i++) electricSkills[i] = true;
        for (int i = 0; i < 9; i++) earthSkills[i] = true;
        for (int i = 0; i < 9; i++) poisonSkills[i] = true;
    }

    public void ChangeColor(int color) {
        GetComponent<SpriteRenderer>().material = colorMaterial[color];
    }
}
