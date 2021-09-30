using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
    [Header("Components")]
    private Rigidbody2D rbEnemy;
    private Animator animatorEnemy;


    [Header("Moviment")]
    public float speed;
    public int facing = 1;
    public float restTime;
    private bool walking = true;
    public float distance;
    private Vector3 startPos;
    public bool patrol;

    public bool checkPlayer;
    public bool circleCast;
    public float detectRange;
    public LayerMask layerPlayer;
    public float aggroSpan;
    private float aggroTimer;

    [Header("Stats")]
    public float health;

    private void Start() {
        startPos = transform.position;
        rbEnemy = GetComponent<Rigidbody2D>();
        animatorEnemy = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        DetectPlayer();
        if (health <= 0) {
            animatorEnemy.SetBool("Die", true);
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void Moviment() {

        if (patrol) {
            if (walking) {
                rbEnemy.velocity = new Vector2(facing * speed, rbEnemy.velocity.y);
                animatorEnemy.SetBool("Run", true);
            }
            else animatorEnemy.SetBool("Run", false);
            if (startPos.x - transform.position.x >= distance && walking && facing == -1) {
                walking = false;
                StartCoroutine(Rest());
            }
            if (startPos.x <= transform.position.x && walking && facing == 1) {
                walking = false;
                StartCoroutine(Rest());
            }
            transform.rotation = Quaternion.Euler(0, 90 - (facing * 90), 0);
        }
    }

    private IEnumerator Rest() {
        yield return new WaitForSeconds(restTime);

        facing = -facing;
        walking = true;
    }

    private void DetectPlayer() {
        if (!circleCast) checkPlayer = Physics2D.Raycast(transform.position, new Vector2(facing, 0), detectRange, layerPlayer);
        else checkPlayer = Physics2D.CircleCast(transform.position, detectRange, new Vector2(facing, 0), 0, layerPlayer);
        if (checkPlayer) aggroTimer = aggroSpan;
        if (aggroTimer > 0) {
            aggroTimer -= Time.deltaTime;
            patrol = false;
        }
        else patrol = true;
    }

    public void Die() {
        Destroy(gameObject);
    }
}
