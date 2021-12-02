using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour {

    private EnemyBase baseScript;
    private DoNothingPatrol patrolScript;
    private Rigidbody2D rbJelly;
    private Animator animJelly;
    private bool wasAtking = false, hasMoved = false;

    public float movementDistance, atkDamage, atkRange;

    private void Start() {
        baseScript = GetComponent<EnemyBase>();
        patrolScript = GetComponent<DoNothingPatrol>();
        rbJelly = GetComponent<Rigidbody2D>();
        animJelly = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        if (baseScript.currentHealth > 0) {
            if (patrolScript.aggroSpan > 0) {
                if (!wasAtking) {
                    wasAtking = true;
                    StartCoroutine(Approach());
                }
            }
            else if (wasAtking) {
                wasAtking = false;
                StopAllCoroutines();
            }
        }
        else StopAllCoroutines();
        if (!baseScript.beingKb) rbJelly.velocity = Vector2.zero;
    }

    private IEnumerator Approach() {
        yield return new WaitForSeconds(0.75f);

        Vector3 v3 = (PlayerData.instance.transform.position - transform.position).normalized * movementDistance;
        for (int i = 0; i < 15; i++) {
            transform.position += v3 / 15;

            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(0.125f);

        if (hasMoved && Vector2.Distance(PlayerData.instance.transform.position, transform.position) < atkRange) StartCoroutine(AtkInstantiate());
        else {
            hasMoved = !hasMoved;
            StartCoroutine(Approach());
        }
    }

    private IEnumerator AtkInstantiate() {
        animJelly.SetTrigger("Atk");

        yield return new WaitForSeconds(1.3f);

        AudioManager.instance.Play("JellySpark");
        bool bol = false;
        for (int i = 0; i < 6; i++) {
            if (!bol && Vector2.Distance(PlayerData.instance.transform.position, transform.position) < atkRange) {
                PlayerData.instance.TakeDamage(atkDamage);
                bol = true;
            }

            yield return new WaitForSeconds(0.1f);
        }

        hasMoved = true;
        StartCoroutine(Approach());
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }
}
