using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPatrol : MonoBehaviour {

    [Header("Components")]
    private Rigidbody2D rbRat;
    private Collider2D colliderRat;
    private Animator animRat;

    [Header("Stats")]
    public float speed;
    private bool wasPatroling;
    public bool patroling = true;
    public float aggroDuration;
    public float aggroSpan;
    public Vector2 squareRange;
    private Vector2[] squarePoints = new Vector2[2];
    public int maxDist;
    public float maxRest;
    [SerializeField] private Vector2 currentTarget;
    private bool isGenerating;
    public Vector2 facing;
    private int preventCrash = 0;
    private Vector3 startPos;

    [Header("FOV")]
    public float radius;
    [Range(0, 360)] public float angle;
    private Transform playerPos;


    private void Start() {
        rbRat = GetComponent<Rigidbody2D>();
        colliderRat = GetComponent<Collider2D>();
        animRat = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        startPos = transform.position;

        squarePoints[0] = new Vector2(transform.position.x - squareRange.x / 2, transform.position.y - squareRange.y / 2);
        squarePoints[1] = new Vector2(transform.position.x + squareRange.x / 2, transform.position.y + squareRange.y / 2);

        GenerateTarget();
    }

    private void FixedUpdate() {
        FOVCheck();
        if (wasPatroling != patroling && patroling) GenerateTarget();
        wasPatroling = patroling;
        if (aggroSpan <= 0) Patrol();
    }

    private void Patrol() {
        if (Mathf.Abs(currentTarget.x - transform.position.x) < 0.1f && Mathf.Abs(currentTarget.y - transform.position.y) < 0.1f) {
            if (!isGenerating) StartCoroutine(RestTime());
            transform.position = currentTarget;
            animRat.SetBool("Moving", false);
        }
        else {
            if (currentTarget.x < squarePoints[0].x || currentTarget.x > squarePoints[1].x || currentTarget.y < squarePoints[0].y || currentTarget.y > squarePoints[1].y) currentTarget = startPos;
            Vector2 direction = (currentTarget - new Vector2(transform.position.x, transform.position.y)).normalized;
            rbRat.velocity = direction * speed;
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
                facing = new Vector2(0, 1);
                currentTarget = new Vector2(transform.position.x, transform.position.y + dist);
                return;
            case 1:
                facing = new Vector2(-1, 0);
                currentTarget = new Vector2(transform.position.x - dist, transform.position.y);
                return;
            case 2:
                facing = new Vector2(1, 0);
                currentTarget = new Vector2(transform.position.x + dist, transform.position.y);
                return;
            case 3:
                facing = new Vector2(0, -1);
                currentTarget = new Vector2(transform.position.x, transform.position.y - dist);
                return;
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
            bool isInRadius = false;
            for (int i = 0; i < rangeChecks.Length; i++) if (rangeChecks[i].tag == "Player") isInRadius = true;
            if (isInRadius) {
                Vector2 directionToTarget = (playerPos.position - transform.position).normalized;


                if (Vector2.Angle(facing, directionToTarget) < angle / 2) {
                    float distanceToTarget = Vector2.Distance(transform.position, playerPos.position);
                    RaycastHit2D[] obstruction = Physics2D.RaycastAll(transform.position, directionToTarget, distanceToTarget);
                    bool hitWall = false;
                    for (int i = 0; i < obstruction.Length; i++) if (obstruction[0].transform.tag == "Wall") hitWall = true;
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
        Gizmos.DrawWireCube(startPos, squareRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(facing.x, facing.y));
    }

}
