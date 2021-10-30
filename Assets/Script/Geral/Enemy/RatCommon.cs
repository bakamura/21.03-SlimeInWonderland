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
            float angle2Player = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle2Player <= 45 && angle2Player > -45) patrolScript.facing = new Vector2(1, 0);
            else if (angle2Player > 45 && angle2Player < 135) patrolScript.facing = new Vector2(0, 1);
            else if (angle2Player >= 135 || angle2Player < -135) patrolScript.facing = new Vector2(-1, 0);
            else if (angle2Player >= -135 && angle2Player <= -45) patrolScript.facing = new Vector2(0, -1);

            if (Vector2.Distance(transform.position, playerGObject.transform.position) < followGap) {
                resting = true;
                animRat.SetTrigger("Atk");
                StartCoroutine(AtkInstance());
            }
        }
        else rbRat.velocity = Vector2.zero;
    }

    IEnumerator AtkInstance() {
        yield return new WaitForSeconds(0.333f);

        Vector3 atkPos = transform.position + (new Vector3(patrolScript.facing.x, patrolScript.facing.y, 0))/2;
        Collider2D[] isInRange = Physics2D.OverlapCircleAll(atkPos, atkRange);
        for (int i = 0; i < isInRange.Length; i++) if (isInRange[i].tag == "Player") playerGObject.GetComponent<PlayerData>().TakeDamage(ratDamage);

        yield return new WaitForSeconds(restingTime);

        resting = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if(Application.isPlaying) Gizmos.DrawWireSphere(transform.position + (new Vector3(patrolScript.facing.x, patrolScript.facing.y, 0)) / 2, atkRange); 
    }

}
