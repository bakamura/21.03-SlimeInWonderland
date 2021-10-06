using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {

    private Rigidbody2D rbPlayer;
    private Animator animPlayer;
    private PlayerMovement movementScript;

    private Vector2 mousePos;
    private bool isAtking = false;

    [Header("Atk 0")]
    public float atk0TotalCDown;
    private float atk0CDown;
    public float damageAtk0;
    public float strenghAtk0Dash;
    private Vector2 currentDashDirection;
    public Image atk0ImageCDown;

    [Header("Atk 1")]
    public float atk1TotalCDown;
    private float atk1CDown;

    [Header("Atk 2")]
    public float atk2TotalCDown;
    private float atk2CDown;

    private int currentAtk = 0;
    private float atkRemember;
    public float totalAtkRemember;

    private void Start() {
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        movementScript = GetComponent<PlayerMovement>();
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
        UpdateUI();
    }

    private void Attack() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (atkRemember > 0) {
            atkRemember -= Time.fixedDeltaTime;
            switch (currentAtk) {
                case 0:
                    atkRemember = 0;
                    return;
                case 1:
                    if (atk0CDown <= 0) BasicAtk1();
                    return;
                case 2:

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
    }

    private void CoolDown() {
        if (atk0CDown > 0) atk0CDown -= Time.fixedDeltaTime;
        if (atk0CDown < 0) atk0CDown = 0;
        if (atk1CDown > 0) atk1CDown -= Time.fixedDeltaTime;
        if (atk1CDown < 0) atk0CDown = 0;
        if (atk2CDown > 0) atk2CDown -= Time.fixedDeltaTime;
        if (atk2CDown < 0) atk0CDown = 0;
    }

    private void UpdateUI() {
        atk0ImageCDown.fillAmount = (atk0TotalCDown - atk0CDown) / atk0TotalCDown;
    }

    #region AnimatorFunctions

    private void BasicAtk1Dash() {
        rbPlayer.AddForce(currentDashDirection * strenghAtk0Dash, ForceMode2D.Force);
    }

    private void BreakMovement() {
        rbPlayer.velocity = Vector2.zero;
    }

    private void RegainControl() {
        isAtking = false;
        movementScript.moveLock = false;
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Enemy" && isAtking) collision.transform.GetComponent<EnemyBase>().TakeDamage(damageAtk0);
    }
}
