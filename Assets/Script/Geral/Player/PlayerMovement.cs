using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;
    [System.NonSerialized] public bool moveLock = false;
    private Vector2 direction;
    private Rigidbody2D rbPlayer;

    private void Start() {
        rbPlayer = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        Inputs();
    }

    private void Inputs() {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Movement() {
        if (!moveLock) rbPlayer.velocity = direction * speed;
    }
}
