using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportPoint : MonoBehaviour {

    public Vector3 teleportPosition;
    [Range(0, 3)] public int transitionDirection;
    public float transitionDuration;
    private Animator animatorWipeTransition;

    private void Start() {
        animatorWipeTransition = GameObject.FindGameObjectWithTag("WipeTransition").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") StartCoroutine(teleportTransition(collision.transform));
    }

    IEnumerator teleportTransition(Transform playerTransform) {
        playerTransform.GetComponent<PlayerMovement>().moveLock = true;
        animatorWipeTransition.SetInteger("Direction", transitionDirection);
        animatorWipeTransition.SetTrigger("WipeIn");

        yield return new WaitForSeconds(transitionDuration / 2);

        playerTransform.position = teleportPosition;

        yield return new WaitForSeconds(transitionDuration / 2);

        animatorWipeTransition.SetTrigger("WipeOut");
        playerTransform.GetComponent<PlayerMovement>().moveLock = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(teleportPosition, 0.25f);
    }

}
