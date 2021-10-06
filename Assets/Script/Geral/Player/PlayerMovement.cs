using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rbPlayer;
    private Animator animPlayer;

    [Header("Movement")]
    public float speed;
    [System.NonSerialized] public bool moveLock = false;
    private Vector2 direction;
    [System.NonSerialized] public Vector2 lastDirection;

    private float idleAnimationCooldown = 0;
    public float idleAnimationTotalCooldown;

    private void Start() {
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
    }

    private void Update() {
        Inputs();
    }

    private void Inputs() {
        int x = (int) Input.GetAxisRaw("Horizontal");
        int y = (int) Input.GetAxisRaw("Vertical");

        if ((x == 0 && y == 0 && !animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Atk0")) && direction.x != 0 || direction.y != 0) lastDirection = direction;

        direction = new Vector2(x, y).normalized;

        if (!animPlayer.GetBool("Moving") || direction.magnitude > 0) {
            animPlayer.SetFloat("Horizontal", x);
            animPlayer.SetFloat("Vertical", y);
        }
        if (direction.magnitude > 0 && animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Idle")) animPlayer.SetBool("Moving", true);
        animPlayer.SetFloat("LastHorizontal", lastDirection.x);
        animPlayer.SetFloat("LastVertical", lastDirection.y);
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Movement() {
        if (!moveLock) rbPlayer.velocity = direction * speed;

        if (direction.x < 0.01 && direction.y < 0.01) {
            if (idleAnimationCooldown < idleAnimationTotalCooldown) idleAnimationCooldown += Time.fixedDeltaTime;
            else {
                animPlayer.SetTrigger("IdleAnimation1");
                idleAnimationCooldown = 0;
            }
        }
        if(!animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Idle Tree")) idleAnimationCooldown = 0;
    }

    public void AtMoveEnd() {
        if(direction.magnitude == 0) animPlayer.SetBool("Moving", false);
    }
}
