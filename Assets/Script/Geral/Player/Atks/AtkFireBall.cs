using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkFireBall : MonoBehaviour {

    [HideInInspector] public float damage;
    public float explosionRange;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Enemy" || collision.tag == "Litable" || collision.tag == "Wall") StartCoroutine(Explode());
    }

    private IEnumerator Explode() {
        yield return new WaitForSeconds(0.1f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRange);
        foreach (Collider2D hit in hits) {
            if (hit.tag == "Enemy") hit.GetComponent<EnemyBase>().TakeDamage(damage);
            else if (hit.tag == "Litable") hit.GetComponent<Burnable>().lit = true;
        }
        Destroy(gameObject);
    }

}
