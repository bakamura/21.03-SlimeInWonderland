using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkSplash : MonoBehaviour {

    public float damage, radius;

    private void Start() {
        StartCoroutine(DoDamage());
    }

    private IEnumerator DoDamage() {
        yield return new WaitForSeconds(0.05f);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
        foreach (RaycastHit2D hit in hits) if (hit.collider.tag == "Enemy") hit.collider.GetComponent<EnemyBase>().TakeDamage(damage);

        yield return new WaitForSeconds(0.45f);

        Destroy(gameObject);
    }

}
