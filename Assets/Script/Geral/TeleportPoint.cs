using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportPoint : MonoBehaviour {

    public bool needInput = false;
    public Vector3 teleportPosition;
    [Range(0, 3)] public int transitionDirection;
    public float delayStart;
    public float transitionDuration;
    private Animator animatorWipeTransition;

    private void Start() {
        animatorWipeTransition = GameObject.FindGameObjectWithTag("WipeTransition").GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if(needInput) collision.GetComponent<PlayerMovement>().OnWater(true);

            if (!needInput || Input.GetKey(KeyCode.Q)) {
                if(needInput) collision.GetComponent<PlayerMovement>().SetDive(true); //
                StartCoroutine(TeleportTransition(collision.transform));
            }

            if (!needInput) StartCoroutine(TeleportTransition(collision.transform));
            else if (Input.GetKey(KeyCode.Q)) {
                collision.GetComponent<PlayerMovement>().SetDive(true); //
                StartCoroutine(TeleportTransition(collision.transform));
            }
        }

        //Invoke("teleportTransition", delayStart);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player" && needInput) collision.GetComponent<PlayerMovement>().OnWater(false);
    }

    IEnumerator TeleportTransition(Transform playerTransform) { //DEBUFGAR.LOG
        playerTransform.GetComponent<PlayerMovement>().moveLock = true;

        yield return new WaitForSeconds(delayStart);

        animatorWipeTransition.SetInteger("Direction", transitionDirection);
        animatorWipeTransition.SetTrigger("WipeIn");

        yield return new WaitForSeconds(transitionDuration / 2);

        playerTransform.position = teleportPosition;
        playerTransform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;


        yield return new WaitForSeconds(transitionDuration / 2);

        animatorWipeTransition.SetTrigger("WipeOut");
        playerTransform.GetComponent<PlayerMovement>().moveLock = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(teleportPosition, 0.25f);
    }

}
