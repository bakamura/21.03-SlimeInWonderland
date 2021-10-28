using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkFireBreath : MonoBehaviour {

    public float damage;
    public ParticleSystem particles;
    [System.NonSerialized] public Vector3 direction;
    private Collider2D col;
    private RaycastHit2D[] hits;

    private void Start() {
        col = GetComponent<Collider2D>();
    }

    public void Play() {
        particles.Play();
        StartCoroutine(CastCol());
    }

    IEnumerator CastCol() {
        yield return new WaitForSeconds(0.2f);

        
        hits = Physics2D.BoxCastAll(transform.position + direction, new Vector2(2.5f, 2.25f), transform.rotation.z + 180, Vector2.zero);

        if (hits != null) foreach (RaycastHit2D hit in hits) {
                if (hit.collider.GetComponent<EnemyBase>() != null) hit.collider.GetComponent<EnemyBase>().TakeDamage(damage);
                else if (hit.collider.GetComponent<Torch>() != null) hit.collider.GetComponent<Torch>().ChangeState(true);
            }

        yield return new WaitForSeconds(1.8f);

        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(direction, 0.1f);
    }
}
