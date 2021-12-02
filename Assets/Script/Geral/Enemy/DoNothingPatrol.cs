using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothingPatrol : MonoBehaviour {

    private EnemyBase baseScript;
    public float aggroRadius, aggroDuration;
    [System.NonSerialized] public float aggroSpan;

    private void Start() {
        baseScript = GetComponent<EnemyBase>();
    }

    private void FixedUpdate() {

        if (baseScript.currentHealth > 0 && PlayerData.instance.currentHealth > 0) {
            if (Vector2.Distance(PlayerData.instance.transform.position, transform.position) < aggroRadius) aggroSpan = aggroDuration;
            else if (aggroSpan > 0) aggroSpan -= Time.fixedDeltaTime;
        }
        else aggroSpan = 0;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }
}
