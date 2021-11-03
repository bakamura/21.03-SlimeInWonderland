using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBoss : MonoBehaviour {

    public float shotForce;
    public float damageShot;
    private float span = 10;

    private void Start() {
        GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * shotForce);
    }

    private void FixedUpdate() {
        span -= Time.fixedDeltaTime;
        if (span <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") collision.GetComponent<PlayerData>().TakeDamage(damageShot);
        if (collision.tag == "Wall") Destroy(gameObject);
    }





}
