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

    [Header("AttackList")]
    private int aa;
    public delegate void AtkList();
    public AtkList[] atkListFire;
    public AtkList[] atkListWater;
    public AtkList[] atkListPlant;
    public AtkList[] atkListElectric;
    public AtkList[] atkListEarth;
    public AtkList[] atkListPoison;

    [Header("Basic Atk")]
    public float damageBasicAtk;
    public float strenghBasicAtkDash;

    [Header("FireBreath")]
    public float damageFireAtk1;
    public ParticleSystem particlesFireAtk1;

    [Header("Meteor I")]
    public float damageFireAtk5;
    public float rangeFireAtk5;
    public GameObject prefabFireAtk5;

    private void Start() {
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        atkScript = GetComponent<PlayerAttack>();
        movementScript = GetComponent<PlayerMovement>();

        AssignAtkList();
    }

    private void AssignAtkList() {
        atkListFire[0] = FireAtk1;
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

    public void FireAtk5() {
        movementScript.moveLock = true;
        rbPlayer.velocity = Vector2.zero;
        animPlayer.SetTrigger("AtkF5");
        atkScript.currentAtk = 0;

        currentAtkDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        movementScript.lastDirection = currentAtkDirection;
        StartCoroutine(FireAtk5Instatiate());
    }

    IEnumerator FireAtk5Instatiate() {
        yield return new WaitForSeconds(0.2f);

        atkScript.isAtking = false;
        movementScript.moveLock = false;
        animPlayer.SetBool("Moving", false);
        GameObject meteorInstance = Instantiate(prefabFireAtk5, transform.position + new Vector3(0, 0.75f, 0), Quaternion.Euler(0, 0, 180));
        if (Vector2.Distance(transform.position, mousePos) >= rangeFireAtk5) meteorInstance.GetComponent<AtkMeteor>().finalPos = new Vector3(transform.position.x + currentAtkDirection.x * rangeFireAtk5, transform.position.y + currentAtkDirection.y * rangeFireAtk5, 0);
        else meteorInstance.GetComponent<AtkMeteor>().finalPos = new Vector3(mousePos.x, mousePos.y, 0);
    }


}
