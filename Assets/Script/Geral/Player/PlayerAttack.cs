using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private Rigidbody2D rbPlayer;
    private Animator animPlayer;
    private PlayerMovement movementScript;
    private PlayerData dataScript;

    private Vector2 mousePos;
    private bool isAtking = false;

    [Header("Atk 0")]
    public float atk0TotalCDown;
    [System.NonSerialized] public float atk0CDown;
    public float damageAtk0;
    public float strenghAtk0Dash;
    private Vector2 currentDashDirection;

    [Header("Atk 1")]
    public float atk1TotalCDown;
    [System.NonSerialized] public float atk1CDown;
    public ParticleSystem atk1Particles;

    [Header("Atk 2")]
    public float atk2TotalCDown;
    [System.NonSerialized] public float atk2CDown;

    private int currentAtk = 0;
    private float atkRemember;
    public float totalAtkRemember;

    private void Start() {
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        movementScript = GetComponent<PlayerMovement>();
        dataScript = GetComponent<PlayerData>();
    }

    private void Update() {
        Inputs();
    }

    private void Inputs() {
        if (Input.GetButtonDown("Fire1")) {
            currentAtk = 1;
            atkRemember = totalAtkRemember;
        }
        if (Input.GetButtonDown("Fire2")) {
            currentAtk = 2;
            atkRemember = totalAtkRemember;
        }
        if (Input.GetButtonDown("Fire3")) {
            currentAtk = 3;
            atkRemember = totalAtkRemember;
        }

    }

    private void FixedUpdate() {
        Attack();
        CoolDown();
    }

    private void Attack() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (atkRemember > 0) {
            atkRemember -= Time.fixedDeltaTime;
            Debug.Log(currentAtk);
            switch (currentAtk) {
                case 0:
                    atkRemember = 0;
                    return;
                case 1:
                    if (atk0CDown <= 0) BasicAtk1();
                    return;
                case 2:
                    if (atk1CDown <= 0) FireAtk1();
                    return;
                case 3:

                    return;
            }
        }

    }

    private void BasicAtk1() {
        isAtking = true;
        currentDashDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        movementScript.moveLock = true;
        rbPlayer.velocity = Vector2.zero;
        animPlayer.SetTrigger("Atk0");
        atk0CDown = atk0TotalCDown;
        currentAtk = 0;

        movementScript.lastDirection = currentDashDirection;
        StartCoroutine(BasicAtk1DashNum());
    }

    IEnumerator BasicAtk1DashNum() {
        yield return new WaitForSeconds(0.1f);

        dataScript.blockState = true;
        rbPlayer.velocity = currentDashDirection * strenghAtk0Dash;

        yield return new WaitForSeconds(0.4f);

        dataScript.blockState = false;
        rbPlayer.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.3f);

        isAtking = false;
        movementScript.moveLock = false;
    }

    private void FireAtk1() {
        isAtking = true;
        movementScript.moveLock = true;
        rbPlayer.velocity = Vector2.zero;
        animPlayer.SetTrigger("AtkF1");
        atk1CDown = atk1TotalCDown;
        currentAtk = 0;

        movementScript.lastDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        atk1Particles.transform.rotation = Quaternion.Euler(movementScript.lastDirection.y * -90, movementScript.lastDirection.x * 90, 0);
        StartCoroutine(FireAtk1Instantiate());
    }

    IEnumerator FireAtk1Instantiate() {
        yield return new WaitForSeconds(0.4f);

        atk1Particles.Play();

        yield return new WaitForSeconds(0.4f);

        isAtking = false;
        movementScript.moveLock = false;
    }

    private void CoolDown() {
        if (atk0CDown > 0) atk0CDown -= Time.fixedDeltaTime;
        if (atk0CDown < 0) atk0CDown = 0;
        if (atk1CDown > 0) atk1CDown -= Time.fixedDeltaTime;
        if (atk1CDown < 0) atk0CDown = 0;
        if (atk2CDown > 0) atk2CDown -= Time.fixedDeltaTime;
        if (atk2CDown < 0) atk0CDown = 0;
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Enemy" && isAtking) collision.transform.GetComponent<EnemyBase>().TakeDamage(damageAtk0);
    }
}
