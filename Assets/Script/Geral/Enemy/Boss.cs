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
    [SerializeField] private bool seePlayer = false;
    private int state = 0;

    public GameObject shotGObject;
    [Range(0, 10)]
    public float areaRange;
    public float areaDamage;
    public float restTime;
    public float shootingDelay;
    public float areaAtkDelay;
    public float shotingForce;
    public float shootingTimer;
    public float areaTimer;

    [Header("Components")]
    private EnemyBase statsScript;
    private Vector2 initialPos;
    private Vector2 targetPos;
    public float relativeDistance;
    public float nearRelativeDistance;
    private float currentPos;
    public float jumpDuration;





    private void Start() {
        rbBoss = GetComponent<Rigidbody2D>();
        collBoss = GetComponent<Collider2D>();
        animBoss = GetComponent<Animator>();
        statsScript = GetComponent<EnemyBase>();
        playerGObject = GameObject.FindGameObjectWithTag("Player");
        seePlayer = true;//
    }

    private void FixedUpdate() {
        AtkBoss();
    }

    private void AtkBoss() {
        switch (state) {
            case 0:
                if (seePlayer) StartCoroutine(RestTime());
                break;//detecta o player
            case 1: break;//pausa
            case 2:
                currentPos += Time.fixedDeltaTime / jumpDuration;
                if (currentPos >= 1) {
                    currentPos = 1;
                    state = 3;
                    collBoss.enabled = true;
                    StartCoroutine(InstantiateShots());
                }
                transform.position = Vector2.Lerp(initialPos, targetPos, currentPos);
                break;//calcula e vai a uma pos ao player
            case 3: break;//instancia 3 bolas de fogo
            case 4:
                currentPos += Time.fixedDeltaTime / jumpDuration;
                if (currentPos >= 1) {
                    currentPos = 1;
                    state = 5;
                    collBoss.enabled = true;
                    StartCoroutine(AreaAtk());
                }
                transform.position = Vector2.Lerp(initialPos, targetPos, currentPos);
                break;//se <50, chega perto do player caso seja preciso
            case 5: break;//se <50, segundo ataque
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

        //iniciar animacao

        yield return new WaitForSeconds(0.1f);//calcular tempo quando as anim estiverem prontas

        float a = Mathf.Atan2(playerGObject.transform.position.y - transform.position.y, playerGObject.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 180;
        GameObject go = Instantiate(shotGObject, transform.position, Quaternion.Euler(0, 0, a));
        go.transform.localScale = new Vector3(4, 4, 1);

        yield return new WaitForSeconds(shootingDelay);

        //iniciar animacao

        yield return new WaitForSeconds(0.1f);//calcular tempo quando as anim estiverem prontas

        a = Mathf.Atan2(playerGObject.transform.position.y - transform.position.y, playerGObject.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 180;
        go = Instantiate(shotGObject, transform.position, Quaternion.Euler(0, 0, a));
        go.transform.localScale = new Vector3(4, 4, 1);


        yield return new WaitForSeconds(shootingDelay);

        //iniciar animacao

        yield return new WaitForSeconds(0.1f);//calcular tempo quando as anim estiverem prontas

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
        //iniciar animacao

        yield return new WaitForSeconds(areaAtkDelay);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, areaRange, Vector2.zero);
        foreach (RaycastHit2D hit in hits) if (hit.collider.GetComponent<PlayerData>() != null) hit.collider.GetComponent<PlayerData>().TakeDamage(areaDamage);

        yield return new WaitForSeconds(areaAtkDelay);

        state = 2;
        JumpPos(relativeDistance);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaRange);
    }
}
