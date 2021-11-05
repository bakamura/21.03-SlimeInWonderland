using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PlayerAtkList : MonoBehaviour {

    [Header("Components")]
    private Rigidbody2D rbPlayer;
    private Animator animPlayer;
    private PlayerAttack atkScript;
    private PlayerMovement movementScript;
    public Collider2D triggerCol;

    private Vector2 mousePos;
    private Vector2 currentAtkDirection;

    [Header("AttackList")]
    private int triggerState = 0;
    public delegate void AtkMethod();
    public AtkMethod atk0;
    public AtkMethod atk1;
    public AtkMethod atk2;

    private GameObject[] hitEnemies = new GameObject[64];
    private int currentListPos = 0;

    public float[] CDowns; // Implement cooldowns

    [Header("Basic Atk")]
    public float damageBasicAtk;
    public float strenghBasicAtkDash;

    [Header("FireBreath")]
    public float damageFireAtk1;
    public GameObject prefabFireAtk1;

    [Header("Meteor I")]
    public float damageFireAtk5;
    public float rangeFireAtk5;
    public GameObject prefabFireAtk5;

    private void Start() {
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        atkScript = GetComponent<PlayerAttack>();
        movementScript = GetComponent<PlayerMovement>();

        atk0 = BasicAtk;
        atk1 = AssociateSkill(0, 0);
        atk2 = AssociateSkill(4, 0);
    }

    public void SkillUpdate(int skill0, int tree0, int skill1, int tree1, int skill2, int tree2) {
        Debug.Log(skill0 + "-" + tree0 + " / " + skill1 + "-" + tree1 + " / " + skill2 + "-" + tree2);
        atk0 = null;
        atk1 = null;
        atk2 = null;

        atk0 = AssociateSkill(skill0, tree0);
        atk1 = AssociateSkill(skill1, tree1);
        atk2 = AssociateSkill(skill2, tree2);

        Debug.Log(atk0.Method + " / " + atk1.Method + " / " + atk2.Method);
    }

    public AtkMethod AssociateSkill(int skill, int tree) {
        if (skill < 0) return BasicAtk;
        switch (tree) {
            case 0:
                switch (skill) {
                    case 0: return FireAtk1;
                    case 1: return FireAtk1;
                    case 2: return FireAtk1;
                    case 3: return FireAtk1;
                    case 4: return FireAtk5;
                    case 5: return FireAtk1;
                    case 6: return FireAtk1;
                    case 7: return FireAtk1;
                    case 8: return FireAtk1;
                }
                break;
            case 1:
                switch (skill) {
                    case 0: return FireAtk1;
                    case 1: return FireAtk1;
                    case 2: return FireAtk1;
                    case 3: return FireAtk1;
                    case 4: return FireAtk5;
                    case 5: return FireAtk1;
                    case 6: return FireAtk1;
                    case 7: return FireAtk1;
                    case 8: return FireAtk1;
                }
                break;
            case 2:
                switch (skill) {
                    case 0: return FireAtk1;
                    case 1: return FireAtk1;
                    case 2: return FireAtk1;
                    case 3: return FireAtk1;
                    case 4: return FireAtk5;
                    case 5: return FireAtk1;
                    case 6: return FireAtk1;
                    case 7: return FireAtk1;
                    case 8: return FireAtk1;
                }
                break;
            case 3:
                switch (skill) {
                    case 0: return FireAtk1;
                    case 1: return FireAtk1;
                    case 2: return FireAtk1;
                    case 3: return FireAtk1;
                    case 4: return FireAtk5;
                    case 5: return FireAtk1;
                    case 6: return FireAtk1;
                    case 7: return FireAtk1;
                    case 8: return FireAtk1;
                }
                break;
            case 4:
                switch (skill) {
                    case 0: return FireAtk1;
                    case 1: return FireAtk1;
                    case 2: return FireAtk1;
                    case 3: return FireAtk1;
                    case 4: return FireAtk5;
                    case 5: return FireAtk1;
                    case 6: return FireAtk1;
                    case 7: return FireAtk1;
                    case 8: return FireAtk1;
                }
                break;
            case 5:
                switch (skill) {
                    case 0: return FireAtk1;
                    case 1: return FireAtk1;
                    case 2: return FireAtk1;
                    case 3: return FireAtk1;
                    case 4: return FireAtk5;
                    case 5: return FireAtk1;
                    case 6: return FireAtk1;
                    case 7: return FireAtk1;
                    case 8: return FireAtk1;
                }
                break;

        }

        return FireAtk1;
    }

    private void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    public void BasicAtk() {
        StartCoroutine(BasicAtkNum());
    }

    IEnumerator BasicAtkNum() {
        currentAtkDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        movementScript.moveLock = true;
        rbPlayer.velocity = Vector2.zero;
        animPlayer.SetTrigger("Atk0");
        atkScript.currentAtk = 0;
        triggerCol.enabled = true;
        triggerState = 1;

        movementScript.lastDirection = currentAtkDirection;

        yield return new WaitForSeconds(0.1f);

        Debug.Log("Movimento começado");
        rbPlayer.velocity = currentAtkDirection * strenghBasicAtkDash;

        yield return new WaitForSeconds(0.4f);

        Debug.Log("Movimento interrompido");
        rbPlayer.velocity = Vector2.zero;
        triggerCol.enabled = false;
        for(int i = 0; i < hitEnemies.Length; i++) hitEnemies[i] = null;
        currentListPos = 0;
        triggerState = 0;

        yield return new WaitForSeconds(0.3f);

        Debug.Log("Reganhou controle ");
        atkScript.isAtking = false;
        movementScript.moveLock = false;
        animPlayer.SetBool("Moving", false);
    }

    public void FireAtk1() {
        StartCoroutine(FireAtk1Instantiate());
    }

    IEnumerator FireAtk1Instantiate() {
        movementScript.moveLock = true;
        rbPlayer.velocity = Vector2.zero;
        animPlayer.SetTrigger("AtkF1");
        atkScript.currentAtk = 0;

        movementScript.lastDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        GameObject breathInstance = Instantiate(prefabFireAtk1, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(movementScript.lastDirection.y, movementScript.lastDirection.x) * Mathf.Rad2Deg) + 90));
        breathInstance.GetComponent<AtkFireBreath>().direction = new Vector3(movementScript.lastDirection.x, movementScript.lastDirection.y, 0);

        yield return new WaitForSeconds(0.4f);

        breathInstance.GetComponent<AtkFireBreath>().Play();

        yield return new WaitForSeconds(0.4f);

        atkScript.isAtking = false;
        movementScript.moveLock = false;
        animPlayer.SetBool("Moving", false);
    }

    public void FireAtk5() {
        StartCoroutine(FireAtk5Instatiate());
    }

    IEnumerator FireAtk5Instatiate() {
        movementScript.moveLock = true;
        rbPlayer.velocity = Vector2.zero;
        animPlayer.SetTrigger("AtkF5");
        atkScript.currentAtk = 0;

        currentAtkDirection = (mousePos - new Vector2(transform.position.x, transform.position.y));
        movementScript.lastDirection = currentAtkDirection.normalized;

        yield return new WaitForSeconds(0.2f);

        atkScript.isAtking = false;
        movementScript.moveLock = false;
        animPlayer.SetBool("Moving", false);
        GameObject meteorInstance = Instantiate(prefabFireAtk5, transform.position + new Vector3(0, 0.75f, 0), Quaternion.Euler(0, 0, 180));
        meteorInstance.GetComponent<AtkMeteor>().damage = damageFireAtk5;
        if (Vector2.Distance(transform.position, new Vector2(transform.position.x, transform.position.y) + currentAtkDirection) >= rangeFireAtk5) meteorInstance.GetComponent<AtkMeteor>().finalPos = new Vector3(transform.position.x + currentAtkDirection.normalized.x * rangeFireAtk5, transform.position.y + currentAtkDirection.normalized.y * rangeFireAtk5, 0);
        else meteorInstance.GetComponent<AtkMeteor>().finalPos = new Vector3(currentAtkDirection.x + transform.position.x, currentAtkDirection.y + transform.position.y, 0);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        switch (triggerState) {
            case 0: break;
            case 1:
                bool bol = false;
                foreach (GameObject go in hitEnemies) if(go == collision.gameObject.gameObject) bol = true;

                if (collision.GetComponent<EnemyBase>() != null && !bol) {
                    collision.GetComponent<EnemyBase>().TakeDamage(damageBasicAtk);
                    hitEnemies[currentListPos] = collision.gameObject;
                    currentListPos++;
                }
                else if (collision.GetComponent<MovableBoulder>() != null) {
                    int i = (int) (Mathf.Atan2(movementScript.lastDirection.y, movementScript.lastDirection.x) * Mathf.Rad2Deg);
                    if (i <= 45 && i > -45) i = 0;
                    else if (i <= -45 && i >= -135) i = 1;
                    else if (i < -135 || i >= 135) i = 2;
                    else if (i < 135 && i > 45) i = 3;
                    collision.GetComponent<MovableBoulder>().SetTarget(i);
                }
                break;
            case 2:

                break;
        }
    }
}