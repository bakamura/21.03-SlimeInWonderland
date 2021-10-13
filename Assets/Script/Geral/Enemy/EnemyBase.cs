using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
    [Header("Components")]
    private Animator animatorEnemy;
    private Collider2D colliderEnemy;
    private PlayerAttack playerAtkScript;

    [Header("Stats")]
    public float maxHealth;
    private float currentHealth;
    public int xpType;
    public float xpAmount;

    [System.NonSerialized] public bool isSpecialCase = false;
    [System.NonSerialized] public bool specialCaseTrigger = false;

    private void Start() {
        animatorEnemy = GetComponent<Animator>();
        colliderEnemy = GetComponent<Collider2D>();
        playerAtkScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage) {
        if (!isSpecialCase) currentHealth -= damage;
        else specialCaseTrigger = true;
        if (currentHealth <= 0) {
            animatorEnemy.SetBool("Die", true);
            colliderEnemy.isTrigger = true;
        }
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void DestroyObject() {
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        playerAtkScript.xpToGet[xpType] += xpAmount;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        playerAtkScript.xpToGet[xpType] -= xpAmount;
    }
}
