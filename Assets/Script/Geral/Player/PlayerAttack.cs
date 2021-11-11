using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private Rigidbody2D rbPlayer;
    private Animator animPlayer;
    private PlayerData dataScript;
    private PlayerMovement movementScript;
    private PlayerAtkList atkList;
    public SkillTreeCanvas treeCanvasScript;

    public float[] xpToGet = { 0, 0, 0, 0, 0, 0 };

    [Header("Atk General")]
    public bool canInput = true;
    [System.NonSerialized] public bool isAtking = false;
    [System.NonSerialized] public int currentAtk = 0;
    private float atkRemember;
    public float totalAtkRemember;

    public float[] atkTotalCDown = new float[3];
    [System.NonSerialized] public float[] atkCDown = new float[3];

    private void Start() {
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        dataScript = GetComponent<PlayerData>();
        movementScript = GetComponent<PlayerMovement>();
        atkList = GetComponent<PlayerAtkList>();
    }

    private void Update() {
        Inputs();
    }

    private void Inputs() {
        if (canInput) {
            if (Input.GetButtonDown("Fire1")) {
                currentAtk = 1;
                atkRemember = totalAtkRemember;
            }
            if (Input.GetButtonDown("Fire2")) {
                currentAtk = 2;
                atkRemember = totalAtkRemember;
            }
            if (Input.GetKeyDown(KeyCode.E)) {
                currentAtk = 3;
                atkRemember = totalAtkRemember;
            }
            if (Input.GetKeyDown(KeyCode.F)) {  //Consume
                currentAtk = 4;
                atkRemember = totalAtkRemember;
            }
        }
    }

    private void FixedUpdate() {
        Attack();
        CoolDown();
    }

    private void Attack() {
        if (atkRemember > 0) {
            atkRemember -= Time.fixedDeltaTime;
            if (!isAtking) {
                switch (currentAtk) {
                    case 0:
                        atkRemember = 0;
                        break;
                    case 1:
                        if (atkCDown[0] <= 0) {
                            isAtking = true;
                            atkList.CastSkill(0);
                            atkCDown[0] = atkTotalCDown[0];
                        }
                        break;
                    case 2:
                        if (atkCDown[1] <= 0) {
                            isAtking = true;
                            atkList.CastSkill(1);
                            atkCDown[1] = atkTotalCDown[1];
                        }
                        break;
                    case 3:
                        if (atkCDown[2] <= 0) {
                            isAtking = true;
                            atkList.CastSkill(2);
                            atkCDown[2] = atkTotalCDown[2];
                        }
                        break;
                    case 4:
                        if (!animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Consume")) {
                            RaycastHit2D[] corpses = Physics2D.CircleCastAll(transform.position, 0.5f, Vector2.zero);
                            GameObject nearest = null;
                            float minDist = 5;
                            foreach (RaycastHit2D corpse in corpses) if (Vector2.Distance(transform.position, corpse.transform.position) < minDist && corpse.collider.tag == "Enemy") {
                                    if (corpse.collider.GetComponent<EnemyBase>().currentHealth < 0) {
                                        nearest = corpse.collider.gameObject;
                                        minDist = Vector2.Distance(transform.position, corpse.transform.position);
                                    }
                            }
                            Debug.Log(nearest.name);
                            if (nearest != null) {
                                animPlayer.SetBool("Consuming", true);
                                StartCoroutine(Consume(nearest.GetComponent<EnemyBase>()));
                            }
                        }
                        break;
                }
            }
        }
    }

    private void CoolDown() {
        if (atkCDown[0] > 0) atkCDown[0] -= Time.fixedDeltaTime;
        if (atkCDown[0] < 0) atkCDown[0] = 0;
        if (atkCDown[1] > 0) atkCDown[1] -= Time.fixedDeltaTime;
        if (atkCDown[1] < 0) atkCDown[1] = 0;
        if (atkCDown[2] > 0) atkCDown[2] -= Time.fixedDeltaTime;
        if (atkCDown[2] < 0) atkCDown[2] = 0;
    }

    IEnumerator Consume(EnemyBase consumed) {
        animPlayer.SetBool("Consuming", true);
        movementScript.moveLock = true;
        rbPlayer.velocity = Vector2.zero;

        yield return new WaitForSeconds(1.25f);

        GetXP(consumed.xpType, consumed.xpAmount);
        dataScript.takeHealing(consumed.maxHealth / 5);
        Destroy(consumed.gameObject);

        animPlayer.SetBool("Consuming", false);
        movementScript.moveLock = false;
    }

    private void GetXP(int type, float amount) {
        switch (type) {
            case 0:
                dataScript.normalXP += amount;
                dataScript.normalLv = CheckLvUp(dataScript.normalXP);
                break;
            case 1:
                dataScript.fireXP += amount;
                dataScript.fireLv = CheckLvUp(dataScript.fireXP);
                break;
            case 2:
                dataScript.waterXP += amount;
                dataScript.waterLv = CheckLvUp(dataScript.waterXP);
                break;
            case 3:
                dataScript.plantXP += amount;
                dataScript.plantLv = CheckLvUp(dataScript.plantXP);
                break;
            case 4:
                dataScript.electricXP += amount;
                dataScript.electricLv = CheckLvUp(dataScript.electricXP);
                break;
            case 5:
                dataScript.earthXP += amount;
                dataScript.earthLv = CheckLvUp(dataScript.earthXP);
                break;
            case 6:
                dataScript.poisonXP += amount;
                dataScript.poisonLv = CheckLvUp(dataScript.poisonXP);
                break;
        }
    }

    private int CheckLvUp(float totalAmount) {
        return (int) Mathf.Pow((totalAmount / 3), 1 / 3);
    }

}
