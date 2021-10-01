using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
    [Header("Components")]
    private Animator animatorEnemy;

    [Header("Stats")]
    public float maxHealth;
    private float currentHealth;

    [System.NonSerialized] public bool isSpecialCase = false;
    [System.NonSerialized] public bool specialCaseTrigger = false;

    private void Start() {
        animatorEnemy = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage) {
        if (!isSpecialCase) currentHealth -= damage;
        else specialCaseTrigger = true;
        if (currentHealth <= 0) animatorEnemy.SetBool("Die", true);
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void DestroyObject() {
        Destroy(gameObject);
    }
}
