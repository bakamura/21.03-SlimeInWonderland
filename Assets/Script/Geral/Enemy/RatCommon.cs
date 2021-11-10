using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatCommon : MonoBehaviour {

    [Header("Components")]
    private Rigidbody2D rbRat;
    private Animator animRat;
    private EnemyBase baseScript;
    private RandomPatrol patrolScript;
    private Transform playerTransform;

    [Header("Stats")]
    public float speed;
    public float followGap;

    public float atkRange;
    public float ratDamage;

    private bool resting = false;
    public float restingTime;

    private void Start() {
        rbRat = GetComponent<Rigidbody2D>();
        animRat = GetComponent<Animator>();
        baseScript = GetComponent<EnemyBase>();
        patrolScript = GetComponent<RandomPatrol>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void FixedUpdate() {
        if (patrolScript.aggroSpan > 0 && baseScript.currentHealth > 0) AtkPatern();
        if (baseScript.currentHealth <= 0) StartCoroutine(StopEverything());
    }

    private void AtkPatern() {
        if (!resting && !baseScript.beingKb) {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rbRat.velocity = direction * speed;
            patrolScript.facing = direction;

            animRat.SetBool("Moving", true);
            animRat.SetFloat("Horizontal", direction.x);
            animRat.SetFloat("Vertical", direction.y);

            if (Vector2.Distance(transform.position, playerTransform.position) < followGap) {
                resting = true;
                animRat.SetTrigger("Atk");
                StartCoroutine(AtkInstance());
            }
        }
        else {
            if (!baseScript.beingKb) rbRat.velocity = Vector2.zero;
            animRat.SetBool("Moving", false);
        }

        if (baseScript.beingKb) {
            StopAllCoroutines();
            resting = false;
            if (animRat.GetCurrentAnimatorStateInfo(0).IsName("Attacking")) animRat.SetTrigger("StopAtk");
        }
    }

    IEnumerator AtkInstance() {
        yield return new WaitForSeconds(0.333f);

        Vector3 atkPos = transform.position + (new Vector3(patrolScript.facing.x, patrolScript.facing.y, 0)) / 2;
        Collider2D[] isInRange = Physics2D.OverlapCircleAll(atkPos, atkRange);
        foreach (Collider2D col in isInRange) if (col.tag == "Player") playerTransform.GetComponent<PlayerData>().TakeDamage(ratDamage);

        yield return new WaitForSeconds(restingTime);

        resting = false;
    }

    IEnumerator StopEverything() {
        rbRat.velocity = Vector2.zero; //
        StopAllCoroutines(); //

        yield return new WaitForEndOfFrame();

        rbRat.velocity = Vector2.zero; //
        StopAllCoroutines(); //

        yield return new WaitForEndOfFrame();

        rbRat.velocity = Vector2.zero; //
        StopAllCoroutines(); //

        yield return new WaitForEndOfFrame();

        rbRat.velocity = Vector2.zero; //
        StopAllCoroutines(); //
        GetComponent<RatCommon>().enabled = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (Application.isPlaying) Gizmos.DrawWireSphere(transform.position + (new Vector3(patrolScript.facing.x, patrolScript.facing.y, 0)) / 2, atkRange);
    }

}
