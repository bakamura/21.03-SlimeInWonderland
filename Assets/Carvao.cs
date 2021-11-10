using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carvao : MonoBehaviour {
    [Header("Components")]
    private Rigidbody2D rbCarvao;
    private Animator animCarvao;
    private EnemyBase baseScript;
    private RandomPatrol patrolScript;
    private Transform playerTransform;
    public GameObject shotGObject;

    [Header("Stats")]
    public float speed;
    public float followGap;

    public float carvaoDamage;

    private bool resting = false;
    public float restingTime;

    public float shotDamage;
    public float shotDelay;
    public float shotForce;
    public float atkRange;

    private void Start() {
        rbCarvao = GetComponent<Rigidbody2D>();
        animCarvao = GetComponent<Animator>();
        baseScript = GetComponent<EnemyBase>();
        patrolScript = GetComponent<RandomPatrol>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void FixedUpdate() {
        if (patrolScript.aggroSpan > 0 && baseScript.currentHealth > 0) AtkPatern();
        if (baseScript.currentHealth <= 0) rbCarvao.velocity = Vector2.zero;
    }

    private void AtkPatern() {
        if (!resting && !baseScript.beingKb) {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rbCarvao.velocity = direction * speed;
            patrolScript.facing = direction;

            animCarvao.SetBool("Moving", true);
            animCarvao.SetFloat("Horizontal", direction.x);
            animCarvao.SetFloat("Vertical", direction.y);

            if (Vector2.Distance(transform.position, playerTransform.position) < followGap) {
                resting = true;
                animCarvao.SetTrigger("Shot");
                StartCoroutine(InstantiateShots());
            }
        }
        else {
            if (!baseScript.beingKb) rbCarvao.velocity = Vector2.zero;
            animCarvao.SetBool("Moving", false);
        }
        if (baseScript.beingKb) {
            StopAllCoroutines();
            resting = false;
            if (animCarvao.GetCurrentAnimatorStateInfo(0).IsName("Attacking")) animCarvao.SetTrigger("StopAtk");
        }
    }

    IEnumerator InstantiateShots() {
        yield return new WaitForSeconds(shotDelay);

        animCarvao.SetTrigger("Shot");

        yield return new WaitForSeconds(0.6875f);

        float a = Mathf.Atan2(playerTransform.transform.position.y - transform.position.y, playerTransform.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 180;
        GameObject go = Instantiate(shotGObject, transform.position, Quaternion.Euler(0, 0, a));
        go.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * shotForce);
        go.GetComponent<FireBallBoss>().damageShot = shotDamage;

        yield return new WaitForSeconds(shotDelay);

        animCarvao.SetTrigger("Shot");

        yield return new WaitForSeconds(0.6875f);

        a = Mathf.Atan2(playerTransform.transform.position.y - transform.position.y, playerTransform.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 180;
        go = Instantiate(shotGObject, transform.position, Quaternion.Euler(0, 0, a));
        go.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * shotForce);
        go.GetComponent<FireBallBoss>().damageShot = shotDamage;

        yield return new WaitForSeconds(restingTime);

        resting = false;
    }

        private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (Application.isPlaying) Gizmos.DrawWireSphere(transform.position + (new Vector3(patrolScript.facing.x, patrolScript.facing.y, 0)) / 2, atkRange);
    }

}
