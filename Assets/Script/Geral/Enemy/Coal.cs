using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Coal : MonoBehaviour {
    [Header("Components")]
    private Rigidbody2D rbCarvao;
    private Animator animCarvao;
    private EnemyBase baseScript;
    private RandomPatrol patrolScript;
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

    private bool inWall;

    private void Start() {
        rbCarvao = GetComponent<Rigidbody2D>();
        animCarvao = GetComponent<Animator>();
        baseScript = GetComponent<EnemyBase>();
        patrolScript = GetComponent<RandomPatrol>();
    }

    private void FixedUpdate() {
        if (patrolScript.aggroSpan > 0 && baseScript.currentHealth > 0) AtkPatern();
        if (baseScript.currentHealth <= 0) {
            StopAllCoroutines();
            GetComponent<Light2D>().enabled = false;
            rbCarvao.velocity = Vector2.zero;
        }
    }

    private void AtkPatern() {
        Vector2 direction = (transform.position - PlayerData.instance.transform.position).normalized;
        if (!resting && !baseScript.beingKb) {
            rbCarvao.velocity = direction * speed;
            patrolScript.facing = direction;

            animCarvao.SetBool("Moving", true);
            animCarvao.SetFloat("Horizontal", direction.x);
            animCarvao.SetFloat("Vertical", direction.y);

            if (Vector2.Distance(transform.position, PlayerData.instance.transform.position) > followGap || inWall) {
                resting = true;
                animCarvao.SetTrigger("Shot");
                StartCoroutine(InstantiateShots());
            }
        }
        else {
            animCarvao.SetFloat("Horizontal", -direction.x);
            animCarvao.SetFloat("Vertical", -direction.y);
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

        float a = Mathf.Atan2(PlayerData.instance.transform.transform.position.y - transform.position.y, PlayerData.instance.transform.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 180;
        GameObject go = Instantiate(shotGObject, transform.position, Quaternion.Euler(0, 0, a));
        go.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * shotForce);
        go.GetComponent<FireBall>().damageShot = shotDamage;

        yield return new WaitForSeconds(restingTime);

        resting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Wall") inWall = true;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.tag == "Wall") inWall = false;
    }
}
