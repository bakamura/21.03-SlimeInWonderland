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

    [Header("Swim")]
    private bool canDive = false;
    private bool underWater = false;

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

        if ((x == 0 && y == 0 && !PlayerData.instance.animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Atk0") && !moveLock) && (direction.x != 0 || direction.y != 0)) lastDirection = direction;

        direction = new Vector2(x, y).normalized;

        if (!PlayerData.instance.animPlayer.GetBool("Moving") || direction.magnitude > 0) {
            PlayerData.instance.animPlayer.SetFloat("Horizontal", x);
            PlayerData.instance.animPlayer.SetFloat("Vertical", y);
        }
        if (direction.magnitude > 0 && PlayerData.instance.animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Idle")) PlayerData.instance.animPlayer.SetBool("Moving", true);
        PlayerData.instance.animPlayer.SetFloat("LastHorizontal", lastDirection.x);
        PlayerData.instance.animPlayer.SetFloat("LastVertical", lastDirection.y);
        if (!moveLock && canDive && Input.GetKeyDown(KeyCode.Q)) SetDive(underWater);
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Movement() {
        if (!moveLock) PlayerData.instance.rbPlayer.velocity = direction * speed;

        if (direction.x < 0.01 && direction.y < 0.01) {
            if (idleAnimationCooldown < idleAnimationTotalCooldown) idleAnimationCooldown += Time.fixedDeltaTime;
            else {
                PlayerData.instance.animPlayer.SetTrigger("IdleAnimation1");
                idleAnimationCooldown = 0;
            }
        }
        if (!PlayerData.instance.animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Idle")) idleAnimationCooldown = 0;
    }

    public void AtMoveEnd() {
        if (direction.magnitude == 0) PlayerData.instance.animPlayer.SetBool("Moving", false);
    }

    public void OnWater(bool bol) {
        PlayerData.instance.animPlayer.SetBool("WaterSpot", bol);
        canDive = bol;
    }

    public void SetDive(bool bol) {
        underWater = !bol;
        if (!bol) PlayerData.instance.animPlayer.SetTrigger("Dive");
        else PlayerData.instance.animPlayer.SetTrigger("Emerge");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        //Use other voids   
    }
}
