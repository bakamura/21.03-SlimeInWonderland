﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour {

    private EnemyBase baseScript;
    private DoNothingPatrol patrolScript;
    private Rigidbody2D rbJelly;
    private Animator animJelly;
    private bool wasAtking = false;
    private int state = 0;

    public float movementDistance, atkDamage, atkRange;

    private void Start() {
        baseScript = GetComponent<EnemyBase>();
        patrolScript = GetComponent<DoNothingPatrol>();
        rbJelly = GetComponent<Rigidbody2D>();
        animJelly = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        if (baseScript.currentHealth <= 0) {
            if (patrolScript.aggroSpan > 0) if (!wasAtking) StartCoroutine(Approach());
            else if (wasAtking) StopAllCoroutines();
        }
        if (!baseScript.beingKb) rbJelly.velocity = Vector2.zero;
    }

    private IEnumerator Approach() {
        yield return new WaitForSeconds(0.4f);

        Vector3 v3 = (PlayerData.instance.transform.position - transform.position).normalized * movementDistance;
        for (int i = 0; i < 40; i++) {
            transform.position += v3 / 40;

            yield return new WaitForSeconds(0.01f);
        }
        if (state < 2 && Vector2.Distance(PlayerData.instance.transform.position, transform.position) < atkRange) StartCoroutine(AtkInstantiate());
        else {
            state = state < 2 ? ++state : 0;
            StartCoroutine(Approach());
        }
    }

    private IEnumerator AtkInstantiate() {
        animJelly.SetTrigger("Atk");

        yield return new WaitForSeconds(1.3f);

        bool bol = false;
        for (int i = 0; i < 6; i++) {
            if (!bol && Vector2.Distance(PlayerData.instance.transform.position, transform.position) < atkRange) {
                PlayerData.instance.TakeDamage(atkDamage);
                bol = true;
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.3f);

        state = 0;
        StartCoroutine(Approach());
    }

}