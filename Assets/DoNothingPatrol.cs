using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothingPatrol : MonoBehaviour {

    public float aggroRadius, aggroDuration;
    [System.NonSerialized] public float aggroSpan;

    private void FixedUpdate() {
        if (Vector2.Distance(PlayerData.instance.transform.position, transform.position) < aggroRadius) aggroSpan = aggroDuration;
        else aggroSpan -= Time.fixedDeltaTime;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }
}
