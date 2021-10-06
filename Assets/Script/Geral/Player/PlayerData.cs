using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    [Header("Stats")]
    public float maxHealth;
    [System.NonSerialized] public float currentHealth;
    [System.NonSerialized] public bool blockState;

    [Header("Progression")]
    public float fireXP;
    public float waterXP;
    public float plantXP;
    public float electricXP;
    public float lightXP;
    public float darkXP;

    public Material[] colorMaterial;
    private int currentColor = 0;


    private void Update() {
        ChangeColor();
    }

    private void TakeDamage(float damage) {
        if (blockState) ;
        else currentHealth -= damage;
    }

    private void ChangeColor() {
        if (Input.GetKeyDown(KeyCode.C)) {
            if (currentColor < colorMaterial.Length - 1) currentColor++;
            else currentColor = 0;

            GetComponent<SpriteRenderer>().material = colorMaterial[currentColor];
        }
    }


}
