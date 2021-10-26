using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkMeteor : MonoBehaviour {

    public float height;
    public float duration;
    private Vector3 startPos;
    public Vector3 finalPos;
    [Range(0, 1)] private float currentPos = 0;

    [Header("Stats")]
    public float impactRadius;
    public float damage;

    private void Start() {
        startPos = transform.position;
    }

    private void FixedUpdate() {
        UpdatePos();
    }

    private void UpdatePos() {
        if (currentPos < 1) {
            currentPos += Time.fixedDeltaTime / duration;
            Vector2 x = Vector2.Lerp(startPos, finalPos, currentPos);
            Vector3 currentWorldPos = new Vector3(x.x, -4 * height * currentPos * currentPos + 4 * height * currentPos + Mathf.Lerp(startPos.y, finalPos.y, currentPos), 0);

            currentPos += 0.1f;
            x = Vector2.Lerp(startPos, finalPos, currentPos);
            Vector3 nextWorldPos = new Vector3(x.x, -4 * height * currentPos * currentPos + 4 * height * currentPos + Mathf.Lerp(startPos.y, finalPos.y, currentPos), 0) - currentWorldPos;
            currentPos -= 0.1f;
            float currentAngle = Mathf.Atan2(nextWorldPos.y, nextWorldPos.x) * Mathf.Rad2Deg + 90;

            transform.position = currentWorldPos;
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
        else if (currentPos >= .975f && currentPos < 2) {
            transform.position = finalPos;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            DamageInstance();
            currentPos = 2;
        }
    }

    private void DamageInstance() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, impactRadius, Vector2.zero);
        foreach (RaycastHit2D i in hits) {
            Debug.Log("Collided with: " + i.transform.name);
            if (i.transform.tag == "Enemy") i.transform.GetComponent<EnemyBase>().TakeDamage(damage);
            else if (i.transform.tag == "Litable") i.transform.GetComponent<Torch>().lit = true;
        }
        //If II, instantiate xxxxx
    }

    //Animator
    public void DestroySelf() {
        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}
