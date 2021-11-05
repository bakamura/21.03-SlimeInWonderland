using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPatrol : MonoBehaviour {

    [Header("Components")]
    private Rigidbody2D rbRat;
    private Animator animRat;
    private EnemyBase dataScript;

    [Header("Stats")]
    public float speed;
    public int maxDist;
    public float maxRest;
    public Vector2 squareRange;
    private Vector2[] squarePoints = new Vector2[2];
    private bool isGenerating;
    private Vector3 currentTarget;
    private Vector3 startPos;
    [System.NonSerialized] public Vector2 facing;

    public float aggroDuration;
    [System.NonSerialized] public float aggroSpan;
    private float lastAggroSpan;

    private int preventCrash = 0;

    [Header("FOV")]
    public float radius;
    [Range(0, 360)] public float angle;
    private Transform playerPos;


    private void Start() {
        rbRat = GetComponent<Rigidbody2D>();
        animRat = GetComponent<Animator>();
        dataScript = GetComponent<EnemyBase>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        startPos = transform.position;
        squarePoints[0] = new Vector2(transform.position.x - squareRange.x / 2, transform.position.y - squareRange.y / 2);
        squarePoints[1] = new Vector2(transform.position.x + squareRange.x / 2, transform.position.y + squareRange.y / 2);

        GenerateTarget();
    }

    private void FixedUpdate() {
        if (dataScript.currentHealth > 0) {
            FOVCheck();
            if (lastAggroSpan != aggroSpan) {
                if (aggroSpan > 0) GenerateTarget();
                lastAggroSpan = aggroSpan;
            }
            if (aggroSpan <= 0) Patrol();
        }
    }

    private void Patrol() {
        if (Mathf.Abs(currentTarget.x - transform.position.x) < 0.1f && Mathf.Abs(currentTarget.y - transform.position.y) < 0.1f) {
            if (!isGenerating) StartCoroutine(RestTime());
            transform.position = currentTarget;
            animRat.SetBool("Moving", false);
        }
        else {
            if (currentTarget.x < squarePoints[0].x || currentTarget.x > squarePoints[1].x || currentTarget.y < squarePoints[0].y || currentTarget.y > squarePoints[1].y) currentTarget = startPos;
            Vector3 direction = (currentTarget - transform.position).normalized;
            facing = direction;
            if(!dataScript.beingKb) rbRat.velocity = direction * speed;
            animRat.SetBool("Moving", true);
            animRat.SetFloat("Horizontal", direction.x);
            animRat.SetFloat("Vertical", direction.y);
        }
    }

    private void GenerateTarget() {
        int dir = Random.Range(0, 4);
        int dist = Random.Range(1, maxDist);

        switch (dir) {
            case 0:
                currentTarget = new Vector3(transform.position.x, transform.position.y + dist, 0);
                break;
            case 1:
                currentTarget = new Vector3(transform.position.x - dist, transform.position.y, 0);
                break;
            case 2:
                currentTarget = new Vector3(transform.position.x + dist, transform.position.y, 0);
                break;
            case 3:
                currentTarget = new Vector3(transform.position.x, transform.position.y - dist, 0);
                break;
        }
    }

    IEnumerator RestTime() {
        isGenerating = true;

        yield return new WaitForSeconds(Random.Range(0, maxRest));

        if (aggroSpan <= 0) {
            GenerateTarget();
            while ((currentTarget.x < squarePoints[0].x || currentTarget.x > squarePoints[1].x || currentTarget.y < squarePoints[0].y || currentTarget.y > squarePoints[1].y) && preventCrash < 10) {
                GenerateTarget();
                preventCrash++;
            }
            if (preventCrash == 10) currentTarget = startPos;
            preventCrash = 0;
        }
        isGenerating = false;
    }

    private void FOVCheck() {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius);

        if (rangeChecks.Length != 0) {
            foreach(Collider2D col in rangeChecks) if (col.tag == "Player") {
                Vector2 directionToTarget = (playerPos.position - transform.position).normalized;

                if (Vector2.Angle(facing, directionToTarget) < angle / 2) {
                    RaycastHit2D[] obstruction = Physics2D.RaycastAll(transform.position, directionToTarget, Vector2.Distance(transform.position, playerPos.position));
                    bool hitWall = false; //
                    foreach(RaycastHit2D obstruct in obstruction) if (obstruct.transform.tag == "Wall") hitWall = true;
                    if (!hitWall) aggroSpan = aggroDuration;
                }
            }
        }
        if (aggroSpan > 0) aggroSpan -= Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Wall") currentTarget = transform.position;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        if (Application.isPlaying) Gizmos.DrawWireCube(startPos, squareRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(facing.x, facing.y));
    }

}
