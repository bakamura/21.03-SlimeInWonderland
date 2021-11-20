using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [HideInInspector] public static PlayerAttack instance;

    public SkillTreeCanvas treeCanvasScript;

    [Header("Atk General")]
    public bool canInput = true;
    [System.NonSerialized] public bool isAtking = false;
    [System.NonSerialized] public int currentAtk = 0;
    private float atkRemember;
    public float totalAtkRemember;

    public float[] atkTotalCDown = new float[3];
    [System.NonSerialized] public float[] atkCDown = new float[3];

    private void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Update() {
        Inputs();
    }

    private void Inputs() {
        if (canInput) {
            if (Input.GetButtonDown("Fire1")) {
                currentAtk = 1;
                atkRemember = totalAtkRemember;
                if (atkCDown[0] > totalAtkRemember) AudioManager.instance.Play("SkillCD");
            }
            if (Input.GetButtonDown("Fire2")) {
                currentAtk = 2;
                atkRemember = totalAtkRemember;
                if (atkCDown[0] > totalAtkRemember) AudioManager.instance.Play("SkillCD");
            }
            if (Input.GetKeyDown(KeyCode.E)) {
                currentAtk = 3;
                atkRemember = totalAtkRemember;
                if (atkCDown[0] > totalAtkRemember) AudioManager.instance.Play("SkillCD");
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
                            PlayerAtkList.instance.CastSkill(0);
                            atkCDown[0] = atkTotalCDown[0];
                        }
                        break;
                    case 2:
                        if (atkCDown[1] <= 0) {
                            isAtking = true;
                            PlayerAtkList.instance.CastSkill(1);
                            atkCDown[1] = atkTotalCDown[1];
                        }
                        break;
                    case 3:
                        if (atkCDown[2] <= 0) {
                            isAtking = true;
                            PlayerAtkList.instance.CastSkill(2);
                            atkCDown[2] = atkTotalCDown[2];
                        }
                        break;
                    case 4:
                        if (!PlayerData.instance.animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Consume")) {
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
                                PlayerData.instance.animPlayer.SetBool("Consuming", true);
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
        PlayerData.instance.animPlayer.SetBool("Consuming", true);
        PlayerMovement.instance.moveLock = true;
        PlayerData.instance.rbPlayer.velocity = Vector2.zero;

        yield return new WaitForSeconds(1.25f);

        GetXP(consumed.xpType, consumed.xpAmount);
        PlayerData.instance.takeHealing(consumed.maxHealth / 5);
        Destroy(consumed.gameObject);

        PlayerData.instance.animPlayer.SetBool("Consuming", false);
        PlayerMovement.instance.moveLock = false;
    }

    private void GetXP(int type, float amount) {
        switch (type) {
            case 0:
                PlayerData.instance.normalXP += amount;
                PlayerData.instance.normalLv = CheckLvUp(PlayerData.instance.normalXP);
                break;
            case 1:
                PlayerData.instance.fireXP += amount;
                PlayerData.instance.fireLv = CheckLvUp(PlayerData.instance.fireXP);
                break;
            case 2:
                PlayerData.instance.waterXP += amount;
                PlayerData.instance.waterLv = CheckLvUp(PlayerData.instance.waterXP);
                break;
            case 3:
                PlayerData.instance.plantXP += amount;
                PlayerData.instance.plantLv = CheckLvUp(PlayerData.instance.plantXP);
                break;
            case 4:
                PlayerData.instance.electricXP += amount;
                PlayerData.instance.electricLv = CheckLvUp(PlayerData.instance.electricXP);
                break;
            case 5:
                PlayerData.instance.earthXP += amount;
                PlayerData.instance.earthLv = CheckLvUp(PlayerData.instance.earthXP);
                break;
            case 6:
                PlayerData.instance.poisonXP += amount;
                PlayerData.instance.poisonLv = CheckLvUp(PlayerData.instance.poisonXP);
                break;
        }
    }

    private int CheckLvUp(float totalAmount) {
        return (int) Mathf.Pow((totalAmount / 3), 1 / 3);
    }

}
