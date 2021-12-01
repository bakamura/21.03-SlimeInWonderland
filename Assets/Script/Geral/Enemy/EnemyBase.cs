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

    public GameObject consumeKeyShow;
    private SpriteRenderer keyShowInstance;

    private void Start() {
        rbEnemy = GetComponent<Rigidbody2D>();
        animatorEnemy = GetComponent<Animator>();
        srEnemy = GetComponent<SpriteRenderer>();
        colliderEnemy = GetComponent<Collider2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //currentHealth = maxHealth;
        if (currentHealth == 0) {
            animatorEnemy.SetBool("Die", true);
            colliderEnemy.isTrigger = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<SpriteRenderer>().sortingOrder = 4;
        }

        keyShowInstance = Instantiate(consumeKeyShow, transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity).GetComponent<SpriteRenderer>();
        keyShowInstance.transform.parent = transform;
    }

    private void Update() {
        Collider2D[] isInRange = Physics2D.OverlapCircleAll(transform.position, 4);
        bool bol = false;
        foreach (Collider2D col in isInRange) if (col.tag == "Player") bol = true;
        if (bol && currentHealth <= 0) {
            keyShowInstance.transform.position = transform.position + new Vector3(0, 0.75f, 0);
            keyShowInstance.color = new Color(1, 1, 1, 1);
        }
        else keyShowInstance.color = new Color(1, 1, 1, 0);
    }

    public void TakeDamage(float damage) {
        if (currentHealth > 0) {
            if (!isSpecialCase) {
                currentHealth -= damage;
                srEnemy.color = Color.red;
                beingKb = true;
                StartCoroutine(stopKB());

                FloatingText go = Instantiate(PlayerData.instance.floatingText, transform.position + new Vector3(Random.Range(-0.45f, 0.45f), Random.Range(-0.35f, 0.35f), 0), Quaternion.identity).GetComponent<FloatingText>();
                go.transform.SetParent(PlayerData.instance.textParent);
                go.color = 1;
                go.type = 2;
                go.transform.localScale *= 1.1f;
                go.text = damage.ToString("F0");
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

}
