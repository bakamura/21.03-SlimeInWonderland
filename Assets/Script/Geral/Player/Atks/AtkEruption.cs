using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkEruption : MonoBehaviour {

    [HideInInspector] public float damage;
    public float explosionRange;

    private void Start() {
        StartCoroutine(Explode());
    }

    private IEnumerator Explode() {
        yield return new WaitForSeconds(0.2f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRange);
        foreach (Collider2D hit in hits) {
            if (hit.tag == "Enemy") hit.GetComponent<EnemyBase>().TakeDamage(damage);
            else if (hit.tag == "Litable") hit.GetComponent<Burnable>().lit = true;
        }

        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
    }
}
