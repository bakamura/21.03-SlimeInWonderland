using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatCommonScript : MonoBehaviour {

    [Header("Components")]
    private Rigidbody2D rbRat;
    private Collider2D colliderRat;
    private Animator animRat;

    [Header("Stats")]
    public float speed;
    [SerializeField] private bool patroling = true;
    public float aggroTimer;
    private float aggroCoolDown;
    private float isAttacking;
    public Vector2 squareRange;
    private Vector2[] squarePoints = new Vector2[2];
    public int maxDist;
    public float maxRest;
    [SerializeField] private Vector2 currentTarget;
    private bool isGenerating;

    private void Start() {
        rbRat = GetComponent<Rigidbody2D>();
        colliderRat = GetComponent<Collider2D>();
        animRat = GetComponent<Animator>();

        squarePoints[0] = new Vector2(transform.position.x - squareRange.x / 2, transform.position.y - squareRange.y / 2);
        squarePoints[1] = new Vector2(transform.position.x + squareRange.x / 2, transform.position.y + squareRange.y / 2);

        GenerateTarget();
    }

    private void FixedUpdate() {
        if (patroling) Patrol();
        else AtkPattern();
    }

    private void Patrol() {
        if (Mathf.Abs(currentTarget.x - transform.position.x) < 0.01f && Mathf.Abs(currentTarget.y - transform.position.y) < 0.01f && !isGenerating) {
            transform.position = currentTarget;
            StartCoroutine(RestTime());
        }
        else {
            Vector2 direction = (currentTarget - new Vector2(transform.position.x, transform.position.y)).normalized;
            rbRat.velocity = direction * speed;
            animRat.SetFloat("Horizontal", direction.x);
            animRat.SetFloat("Vertical", direction.y);
            animRat.SetBool("Moving", direction != Vector2.zero);
        }
    }

    private void GenerateTarget() {
        int dir = Random.Range(0, 4);
        int dist = Random.Range(1, maxDist);

        switch (dir) {
            case 0:
                currentTarget = new Vector2(transform.position.x, transform.position.y + dist);
                return;
            case 1:
                currentTarget = new Vector2(transform.position.x - dist, transform.position.y);
                return;
            case 2:
                currentTarget = new Vector2(transform.position.x + dist, transform.position.y);
                return;
            case 3:
                currentTarget = new Vector2(transform.position.x, transform.position.y - dist);
                return;
        }
    }

    IEnumerator RestTime() {
        isGenerating = true;

        yield return new WaitForSeconds(Random.Range(0, maxRest));

        GenerateTarget();
        while (currentTarget.x < squarePoints[0].x || currentTarget.x > squarePoints[1].x || currentTarget.y < squarePoints[0].y || currentTarget.y > squarePoints[1].y) GenerateTarget();

        isGenerating = false;
    }

    private void AtkPattern() {

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Wall") currentTarget = transform.position;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(squarePoints[0] + new Vector2(squareRange.x / 2, squareRange.y / 2), squareRange);
    }

}
