using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBoss : MonoBehaviour {

    public float shotForce;
    public float damageShot;

    private void Start() {
        GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.down * shotForce);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") collision.GetComponent<PlayerData>().TakeDamage(damageShot);
    }





}
