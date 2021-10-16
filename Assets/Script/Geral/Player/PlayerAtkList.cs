using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtkList : MonoBehaviour {

    [Header("Components")]
    private Rigidbody2D rbPlayer;
    private Animator animPlayer;
    private PlayerAttack atkScript;
    private PlayerMovement movementScript;

    private Vector2 mousePos;
    private Vector2 currentAtkDirection;

    [Header("Basic Atk")]
    public float damageBasicAtk;
    public float strenghBasicAtkDash;

    [Header("FireBreath")]
    public float damageFireAtk1;
    public ParticleSystem particlesFireAtk1;

    [Header("Meteor I")]
    public float damageFireAtk32;
    public float rangeFireAtk32;
    public GameObject prefabFireAtk32;

    private void Start() {
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        atkScript = GetComponent<PlayerAttack>();
        movementScript = GetComponent<PlayerMovement>();
    }

    private void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    public void BasicAtk() {
        currentAtkDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        movementScript.moveLock = true;
        rbPlayer.velocity = Vector2.zero;
        animPlayer.SetTrigger("Atk0");
        atkScript.currentAtk = 0;

        movementScript.lastDirection = currentAtkDirection;
        StartCoroutine(BasicAtkNum());
    }

    IEnumerator BasicAtkNum() {
        yield return new WaitForSeconds(0.1f);

        rbPlayer.velocity = currentAtkDirection * strenghBasicAtkDash;

        yield return new WaitForSeconds(0.4f);

        rbPlayer.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.3f);

        atkScript.isAtking = false;
        movementScript.moveLock = false;
        animPlayer.SetBool("Moving", false);
    }

    public void FireAtk1() {
        movementScript.moveLock = true;
        rbPlayer.velocity = Vector2.zero;
        animPlayer.SetTrigger("AtkF1");
        atkScript.currentAtk = 0;

        movementScript.lastDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        particlesFireAtk1.transform.rotation = Quaternion.Euler(movementScript.lastDirection.y * -90, movementScript.lastDirection.x * 90, 0);
        StartCoroutine(FireAtk1Instantiate());
    }

    IEnumerator FireAtk1Instantiate() {
        yield return new WaitForSeconds(0.4f);

        particlesFireAtk1.Play();

        yield return new WaitForSeconds(0.4f);

        atkScript.isAtking = false;
        movementScript.moveLock = false;
        animPlayer.SetBool("Moving", false);
    }

    public void FireAtk32() {
        movementScript.moveLock = true;
        rbPlayer.velocity = Vector2.zero;
        animPlayer.SetTrigger("AtkF32");
        atkScript.currentAtk = 0;

        currentAtkDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        movementScript.lastDirection = currentAtkDirection;
        StartCoroutine(FireAtk32Instatiate());
    }

    IEnumerator FireAtk32Instatiate() {
        yield return new WaitForSeconds(0.2f);

        atkScript.isAtking = false;
        movementScript.moveLock = false;
        animPlayer.SetBool("Moving", false);
        GameObject meteorInstance = Instantiate(prefabFireAtk32, transform.position + new Vector3(0, 0.75f, 0), Quaternion.Euler(0, 0, 180));
        if (Vector2.Distance(transform.position, mousePos) >= rangeFireAtk32) meteorInstance.GetComponent<AtkMeteor>().finalPos = new Vector3(transform.position.x + currentAtkDirection.x * rangeFireAtk32, transform.position.y + currentAtkDirection.y * rangeFireAtk32, 0);
        else meteorInstance.GetComponent<AtkMeteor>().finalPos = new Vector3(mousePos.x, mousePos.y, 0);
    }


}
