using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerData : MonoBehaviour {

    [Header("Components")]
    private Animator animPlayer;

    [Header("Stats")]
    public float maxHealth;
    //[System.NonSerialized]
    public float currentHealth;
    [System.NonSerialized] public bool blockState;

    [Header("Progression")]
    public float fireXP;
    public float waterXP;
    public float plantXP;
    public float electricXP;
    public float earthXP;
    public float poisonXP;

    public Material[] colorMaterial;
    private int currentColor = 0;

    private void Start() {
        animPlayer = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update() {
        ChangeColor();
    }

    public void TakeDamage(float damage) {
        if (blockState) currentHealth = currentHealth - 0;
        else currentHealth -= damage;
        animPlayer.SetBool("Consuming", false);

        if (currentHealth <= 0) Application.LoadLevel(Application.loadedLevel);
    }

    public void ChangeColor() {
        if (Input.GetKeyDown(KeyCode.C)) {
            if (currentColor < colorMaterial.Length - 1) currentColor++;
            else currentColor = 0;

            GetComponent<SpriteRenderer>().material = colorMaterial[currentColor];
        }
    }


}
