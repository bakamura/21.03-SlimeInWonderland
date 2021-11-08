using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    [Header("Components")]
    private Rigidbody2D rbBoss;
    private Collider2D collBoss;
    private Animator animBoss;
    private EnemyBase statsScript;
    private GameObject playerGObject;

    [Header("Stats")]
    private int state = 0;
    public float speed;
    private Vector3 jumpDirection; //
    private Vector3 targetPos;

    private Vector3 startPos;
    public Vector2 arenaRange;

    public GameObject shotGObject;
    public float shotDamage;
    public float shotDelay;
    public float shotForce;
    public float relativeDistance;

    public float areaRange;
    public float areaAtkDelay;
    public float areaDamage;
    public float nearRelativeDistance;

    public float restTime;

    private float lastHorizontal;
    private float lastVertical;

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
            AnimLook();
        }
        else {
            state = 0;
            StopAllCoroutines();
        }
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
                rbBoss.velocity = jumpDirection * speed;
                if (Mathf.Abs(transform.position.x - targetPos.x) < 0.05f || Mathf.Abs(transform.position.y - targetPos.y) < 0.05f) {
                    rbBoss.velocity = Vector2.zero;
                    transform.position = targetPos;
                    state = 3;
                    Physics2D.IgnoreCollision(playerGObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
                    StartCoroutine(InstantiateShots());
                }
                break;
            case 3:
                rbBoss.velocity = Vector2.zero;
                break;
            case 4:
                rbBoss.velocity = jumpDirection * speed;
                if (Mathf.Abs(transform.position.x - targetPos.x) < 0.05f || Mathf.Abs(transform.position.y - targetPos.y) < 0.05f) {
                    rbBoss.velocity = Vector2.zero;
                    transform.position = targetPos;
                    state = 5;
                    Physics2D.IgnoreCollision(playerGObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
                    StartCoroutine(AreaAtk());
                }
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
        jumpDirection = (targetPos - transform.position).normalized;
        Physics2D.IgnoreCollision(playerGObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
    }

    IEnumerator InstantiateShots() {
        yield return new WaitForSeconds(shotDelay);

        animBoss.SetTrigger("Shot");

        yield return new WaitForSeconds(0.6875f);

        float a = Mathf.Atan2(playerGObject.transform.position.y - transform.position.y, playerGObject.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 180;
        GameObject go = Instantiate(shotGObject, transform.position, Quaternion.Euler(0, 0, a));
        go.transform.localScale = new Vector3(4, 4, 1);
        go.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * shotForce);
        go.GetComponent<FireBallBoss>().damageShot = shotDamage;

        yield return new WaitForSeconds(shotDelay);

        animBoss.SetTrigger("Shot");

        yield return new WaitForSeconds(0.6875f);

        a = Mathf.Atan2(playerGObject.transform.position.y - transform.position.y, playerGObject.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 180;
        go = Instantiate(shotGObject, transform.position, Quaternion.Euler(0, 0, a));
        go.transform.localScale = new Vector3(4, 4, 1);
        go.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * shotForce);
        go.GetComponent<FireBallBoss>().damageShot = shotDamage;

        yield return new WaitForSeconds(shotDelay);

        animBoss.SetTrigger("Shot");

        yield return new WaitForSeconds(0.6875f);

        a = Mathf.Atan2(playerGObject.transform.position.y - transform.position.y, playerGObject.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 180;
        go = Instantiate(shotGObject, transform.position, Quaternion.Euler(0, 0, a));
        go.transform.localScale = new Vector3(4, 4, 1);
        go.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * shotForce);
        go.GetComponent<FireBallBoss>().damageShot = shotDamage;

        yield return new WaitForSeconds(restTime);

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

        RaycastHit2D[] hits = Physics2D.CapsuleCastAll(transform.position + new Vector3(0, -0.5f, 0), new Vector2(areaRange, areaRange / 2), 0, 0, Vector2.zero); //precisa debugar
        foreach (RaycastHit2D hit in hits) if (hit.collider.GetComponent<PlayerData>() != null) hit.collider.GetComponent<PlayerData>().TakeDamage(areaDamage);

        yield return new WaitForSeconds(restTime);

        state = 2;
        JumpPos(relativeDistance);
    }

    private void AnimLook() {
        if (transform.position != targetPos) {
            lastHorizontal = targetPos.x - transform.position.x;
            lastVertical = targetPos.y - transform.position.y;
        }
        else {
            lastHorizontal = playerGObject.transform.position.x - transform.position.x;
            lastVertical = playerGObject.transform.position.y - transform.position.y;
        }
        animBoss.SetFloat("Horizontal", lastHorizontal);
        animBoss.SetFloat("Vertical", lastVertical);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Wall") targetPos = transform.position;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        if (Application.isPlaying) Gizmos.DrawWireCube(startPos, arenaRange);
    }
}
