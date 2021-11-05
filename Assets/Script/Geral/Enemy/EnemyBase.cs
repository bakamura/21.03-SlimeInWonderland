using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

    [Header("Components")]
    private Rigidbody2D rbEnemy;
    private Animator animatorEnemy;
    private SpriteRenderer srEnemy;
    private Collider2D colliderEnemy;
    private Transform playerTransform;

    [Header("Stats")]
    public float maxHealth;
    public float currentHealth;
    public bool knockBackable;
    public float knockBackStrength;
    [System.NonSerialized] public bool beingKb = false;

    public int xpType;
    public float xpAmount;

    [System.NonSerialized] public bool isSpecialCase = false;
    [System.NonSerialized] public bool specialCaseTrigger = false;

    private void Start() {
        rbEnemy = GetComponent<Rigidbody2D>();
        animatorEnemy = GetComponent<Animator>();
        srEnemy = GetComponent<SpriteRenderer>();
        colliderEnemy = GetComponent<Collider2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage) {
        if (currentHealth > 0) {
            if (!isSpecialCase) {
                currentHealth -= damage;
                srEnemy.color = Color.red;
                beingKb = true;
                StartCoroutine(stopKB());
            }
            else specialCaseTrigger = true;
            if (currentHealth <= 0) {
                animatorEnemy.SetBool("Die", true);
                colliderEnemy.isTrigger = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<SpriteRenderer>().sortingOrder = 4;
            }
            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }
    }

    IEnumerator stopKB() {
        yield return new WaitForFixedUpdate();

        rbEnemy.velocity = (transform.position - playerTransform.position).normalized * knockBackStrength;

        yield return new WaitForSeconds(0.2f);

        rbEnemy.velocity = Vector2.zero;
        srEnemy.color = Color.white;
        beingKb = false;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        //
    }
}
