using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalFireBall : MonoBehaviour {

    public float shotForce;
    public float damageShot;
    private float span = 10;

    private void FixedUpdate() {
        span -= Time.fixedDeltaTime;
        if (span <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.GetComponent<PlayerData>().TakeDamage(damageShot);
            StartCoroutine(ExplosionTime());
        }
        else if (collision.tag == "Litable") {
            collision.GetComponent<Burnable>().lit = true;
            StartCoroutine(ExplosionTime());
        }
        else if (collision.tag == "Wall") StartCoroutine(ExplosionTime());
    }

    IEnumerator ExplosionTime() {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().SetTrigger("Explosion");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}
