using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;
    [System.NonSerialized] public bool moveLock = false;
    private Vector2 direction;
    private Rigidbody2D rbPlayer;
    private Animator animPlayer;

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
        direction = new Vector2(x, y).normalized;
        animPlayer.SetFloat("Horizontal", x);
        animPlayer.SetFloat("Vertical", y);
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Movement() {
        if (!moveLock) rbPlayer.velocity = direction * speed;
    }
}
