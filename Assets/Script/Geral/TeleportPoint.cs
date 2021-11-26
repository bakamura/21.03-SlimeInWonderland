using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportPoint : MonoBehaviour {

    private bool isTeleporting = false;
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
            if(needInput) PlayerData.animPlayer.SetBool("OnWater", true);

            if ((!needInput || Input.GetKey(KeyCode.Q)) && !isTeleporting && !PlayerMovement.instance.moveLock) StartCoroutine(TeleportTransition());
        }

        //Invoke("teleportTransition", delayStart);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player" && needInput) PlayerData.animPlayer.SetBool("OnWater", false);
    }

    IEnumerator TeleportTransition() {
        isTeleporting = true;
        PlayerMovement.instance.moveLock = true;

        //yield return new WaitForSeconds(delayStart);

        animatorWipeTransition.SetInteger("Direction", transitionDirection);
        animatorWipeTransition.SetTrigger("WipeIn");

        yield return new WaitForSeconds(transitionDuration / 2);

        PlayerData.instance.transform.position = teleportPosition;
        PlayerData.rbPlayer.velocity = Vector2.zero;


        yield return new WaitForSeconds(transitionDuration / 2);

        animatorWipeTransition.SetTrigger("WipeOut");
        PlayerMovement.instance.moveLock = false;
        isTeleporting = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(teleportPosition, 0.25f);
    }

}
