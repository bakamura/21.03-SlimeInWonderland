using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatCommon : MonoBehaviour {

    private RandomPatrol patrolScript;
    private Rigidbody2D rbRat;
    private Animator animRat;
    private GameObject playerGObject;

    [Header("Stats")]
    public float speed;
    public float followGap;
    public float atkRange;
    private bool resting;
    public float restingTime;
    public float ratDamage;

    private void Start() {
        patrolScript = GetComponent<RandomPatrol>();
        rbRat = GetComponent<Rigidbody2D>();
        animRat = GetComponent<Animator>();
        playerGObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate() {
        if (patrolScript.aggroSpan > 0) AtkPatern();
    }

    private void AtkPatern() {
        if (!resting) {
            Vector2 direction = (playerGObject.transform.position - transform.position).normalized;
            animRat.SetFloat("Horizontal", direction.x);
            animRat.SetFloat("Vertical", direction.y);
            rbRat.velocity = direction * speed;
            if (Vector2.Distance(transform.position, playerGObject.transform.position) < followGap) {
                resting = true;
                animRat.SetTrigger("Atk");
            }
        }
        else rbRat.velocity = Vector2.zero;
    }

    public void AtkInstance() {
        Collider2D[] isInRange = Physics2D.OverlapCircleAll(transform.position, atkRange);
        for (int i = 0; i < isInRange.Length; i++) if (isInRange[i].tag == "Player") playerGObject.GetComponent<PlayerData>().TakeDamage(ratDamage);
        StartCoroutine(RestTime());
    }

    IEnumerator RestTime() {
        yield return new WaitForSeconds(restingTime);

        resting = false;
    }

}
