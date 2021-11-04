using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    private Rigidbody2D rbBoss;
    private Collider2D collBoss;
    private Animator animBoss;
    private GameObject playerGObject;

    [Header("Stats")]
    public float speed;
    public float followGap;
    public float atkRange;
    public float bossDamage;
    private int state = 0;

    public GameObject shotGObject;
    private Vector3 startPos;
    [Range(0, 10)]
    public float areaRange;
    public float areaDamage;
    public float restTime;
    public float shootingDelay;
    public float areaAtkDelay;
    public float shotingForce;
    public float shootingTimer;
    public float areaTimer;
    private float lastHorizontal;
    private float lastVertical;

    [Header("Components")]
    private EnemyBase statsScript;
    private Vector2 initialPos;
    private Vector3 targetPos;
    public float relativeDistance;
    public float nearRelativeDistance;
    private float currentPos;
    public float jumpDuration;
    public Vector2 arenaRange;

    private void Start() {
        rbBoss = GetComponent<Rigidbody2D>();
        collBoss = GetComponent<Collider2D>();
        animBoss = GetComponent<Animator>();
        statsScript = GetComponent<EnemyBase>();
        playerGObject = GameObject.FindGameObjectWithTag("Player");

        startPos = transform.position;
    }

    private void FixedUpdate() {
        if (statsScript.currentHealth > 0) {
            AtkBoss();
            if (transform.position != targetPos) {
                lastHorizontal = targetPos.x - transform.position.x;
                lastVertical = targetPos.y - transform.position.y;
            }
            else {
                lastHorizontal = playerGObject.transform.position.x - transform.position.x;
                lastVertical = playerGObject.transform.position.y - transform.position.y;
            }
        }
        else {
            state = 0;
            StopAllCoroutines();
        }
        animBoss.SetFloat("Horizontal", lastHorizontal);
        animBoss.SetFloat("Vertical", lastVertical);
    }

    private void AtkBoss() {
        switch (state) {
            case 0:

                RaycastHit2D[] hits = Physics2D.BoxCastAll(startPos, arenaRange, 0, Vector2.zero);
                foreach (RaycastHit2D hit in hits) if (hit.collider.tag == "Player") StartCoroutine(RestTime());
                rbBoss.velocity = Vector2.zero;
                break;
            case 1:
                rbBoss.velocity = Vector2.zero;
                break;
            case 2:
                currentPos += Time.fixedDeltaTime / jumpDuration;
                if (currentPos >= 1) {
                    currentPos = 1;
                    state = 3;
                    collBoss.enabled = true;
                    StartCoroutine(InstantiateShots());
                }
                transform.position = Vector2.Lerp(initialPos, targetPos, currentPos);
                break;
            case 3:
                rbBoss.velocity = Vector2.zero;
                break;
            case 4:
                currentPos += Time.fixedDeltaTime / jumpDuration;
                if (currentPos >= 1) {
                    currentPos = 1;
                    state = 5;
                    collBoss.enabled = true;
                    StartCoroutine(AreaAtk());
                }
                transform.position = Vector2.Lerp(initialPos, targetPos, currentPos);
                break;
            case 5:
                rbBoss.velocity = Vector2.zero;
                break;
        }

    }

    IEnumerator RestTime() {
        state = 1;

        yield return new WaitForSeconds(restTime);

        state = 2;
        JumpPos(relativeDistance);
    }

    private void JumpPos(float distance) {
        int i = Random.Range(0, 360);
        Vector3 relativePos = new Vector3(Mathf.Cos(i * Mathf.PI / 180), Mathf.Sin(i * Mathf.PI / 180), 0).normalized;
        targetPos = playerGObject.transform.position + relativePos * distance;
        currentPos = 0;
        initialPos = transform.position;
        collBoss.enabled = false;
    }

    IEnumerator InstantiateShots() {
        yield return new WaitForSeconds(shootingDelay);

        animBoss.SetTrigger("Shot");

        yield return new WaitForSeconds(0.6875f);

        float a = Mathf.Atan2(playerGObject.transform.position.y - transform.position.y, playerGObject.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 180;
        GameObject go = Instantiate(shotGObject, transform.position, Quaternion.Euler(0, 0, a));
        go.transform.localScale = new Vector3(4, 4, 1);

        yield return new WaitForSeconds(shootingDelay);

        animBoss.SetTrigger("Shot");

        yield return new WaitForSeconds(0.6875f);

        a = Mathf.Atan2(playerGObject.transform.position.y - transform.position.y, playerGObject.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 180;
        go = Instantiate(shotGObject, transform.position, Quaternion.Euler(0, 0, a));
        go.transform.localScale = new Vector3(4, 4, 1);


        yield return new WaitForSeconds(shootingDelay);

        animBoss.SetTrigger("Shot");

        yield return new WaitForSeconds(0.6875f);

        a = Mathf.Atan2(playerGObject.transform.position.y - transform.position.y, playerGObject.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 180;
        go = Instantiate(shotGObject, transform.position, Quaternion.Euler(0, 0, a));
        go.transform.localScale = new Vector3(4, 4, 1);


        yield return new WaitForSeconds(shootingDelay * 3);

        if (statsScript.currentHealth < statsScript.maxHealth / 2) {
            state = 4;
            JumpPos(nearRelativeDistance);
        }
        else {
            state = 2;
            JumpPos(relativeDistance);
        }  
    }

    IEnumerator AreaAtk() {
        animBoss.SetTrigger("Area");

        yield return new WaitForSeconds(0.6f);

        RaycastHit2D[] hits = Physics2D.CapsuleCastAll(transform.position + new Vector3(0, -0.5f, 0), new Vector2 (areaRange, areaRange / 2), 0, 0, Vector2.zero); //precisa debugar
        foreach (RaycastHit2D hit in hits) if (hit.collider.GetComponent<PlayerData>() != null) hit.collider.GetComponent<PlayerData>().TakeDamage(areaDamage);

        yield return new WaitForSeconds(areaAtkDelay);

        state = 2;
        JumpPos(relativeDistance);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(1.5f, -0.5f , 0), areaRange);
        Gizmos.DrawWireSphere(transform.position + new Vector3(-1.5f, -0.5f, 0), areaRange);
        Gizmos.color = Color.green;
        if (Application.isPlaying) Gizmos.DrawWireCube(startPos, arenaRange);
    }
}
