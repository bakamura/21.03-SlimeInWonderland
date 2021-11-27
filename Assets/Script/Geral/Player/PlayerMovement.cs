using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [HideInInspector] public static PlayerMovement instance = null;

    [Header("Movement")]
    public float speed;
    [System.NonSerialized] public bool moveLock = false;
    private Vector2 direction;
    [System.NonSerialized] public Vector2 lastDirection;

    private float idleAnimationCooldown = 0;
    public float idleAnimationTotalCooldown;

    private void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Update() {
        Inputs();
    }

    private void Inputs() {
        int x = (int)Input.GetAxisRaw("Horizontal");
        int y = (int)Input.GetAxisRaw("Vertical");

        if ((x == 0 && y == 0 && !PlayerData.animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Atk0") && !moveLock) && (direction.x != 0 || direction.y != 0)) lastDirection = direction;

        direction = new Vector2(x, y).normalized;

        if (!PlayerData.animPlayer.GetBool("Moving") || direction.magnitude > 0) {
            PlayerData.animPlayer.SetFloat("Horizontal", x);
            PlayerData.animPlayer.SetFloat("Vertical", y);
        }
        if (direction.magnitude > 0 && (PlayerData.animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Idle") || PlayerData.animPlayer.GetCurrentAnimatorStateInfo(0).IsName("IdleAnimation"))) PlayerData.animPlayer.SetBool("Moving", true);
        PlayerData.animPlayer.SetFloat("LastHorizontal", lastDirection.x);
        PlayerData.animPlayer.SetFloat("LastVertical", lastDirection.y);
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Movement() {
        if (!moveLock) PlayerData.rbPlayer.velocity = direction * speed;

        if (direction.x < 0.01 && direction.y < 0.01) {
            if (idleAnimationCooldown < idleAnimationTotalCooldown) idleAnimationCooldown += Time.fixedDeltaTime;
            else {
                PlayerData.animPlayer.SetTrigger("IdleAnimation1");
                idleAnimationCooldown = 0;
            }
        }
        if (!PlayerData.animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Idle")) idleAnimationCooldown = 0;
    }

    public void AtMoveEnd() {
        if (direction.magnitude == 0) PlayerData.animPlayer.SetBool("Moving", false);
    }
}
