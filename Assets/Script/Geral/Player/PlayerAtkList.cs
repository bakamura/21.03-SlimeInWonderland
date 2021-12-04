using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoolDown {
    public float[] coolDown;
}

public class PlayerAtkList : MonoBehaviour {

    [HideInInspector] public static PlayerAtkList instance;

    public Collider2D triggerCol;

    private Vector2 mousePos;
    private Vector3 currentAtkDirection;

    private TriggerWater waterManager;

    [Header("AttackList")]
    [HideInInspector] public int[] skill = new int[3], tree = new int[3];
    public CoolDown[] treeCoolDowns;

    [Header("Trigger")]
    private int triggerState = 0, currentListPos = 0;
    private GameObject[] hitEntities = new GameObject[64];


    [Header("Basic Atk")]
    public float damageBasicAtk, strenghBasicAtkDash;

    [Header("FireBreath")]
    public float damageFireAtk1;
    public GameObject prefabFireAtk1;

    [Header("Fireball")]
    public float damageFireAtk2;
    public GameObject prefabFireAtk2;
    public float speedFireAtk2;

    [Header("Eruption")]
    public float damageFireAtk3;
    public GameObject prefabFireAtk3;
    public float rangeFireAtk3;

    [Header("Meteor I")]
    public float damageFireAtk5;
    public GameObject prefabFireAtk5;
    public float rangeFireAtk5;

    [Header("Splash")]
    public float damageWaterAtk1;
    public GameObject prefabWaterAtk1;

    private void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start() {
        waterManager = GameObject.FindGameObjectWithTag("WaterManager").GetComponent<TriggerWater>();
        skill[0] = 0;
        skill[1] = -1;
        skill[2] = -1;
        tree[0] = 0;
        tree[1] = 0; //null
        tree[2] = 0; //null
    }

    private void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void CastSkill(int skillSlot) {
        FindTree(skillSlot);
    }

    private void FindTree(int skillSlot) {
        if (tree[skillSlot] == 2 || !PlayerData.animPlayer.GetBool("OnWater")) {
            PlayerAttack.instance.atkCDown[skillSlot] = PlayerAttack.instance.atkTotalCDown[skillSlot];
            PlayerAttack.instance.isAtking = true;
            if (tree[skillSlot] != 0) {
                waterManager.transformCollider(tree[skillSlot] == 2);
                PlayerData.instance.currentMaterial = tree[skillSlot];
                PlayerData.srPlayer.material = PlayerData.instance.colorMaterial[tree[skillSlot]];
            }
            switch (tree[skillSlot]) {
                case 0:
                    BasicAtk(skill[skillSlot]);
                    break;
                case 1:
                    FireAtks(skill[skillSlot]);
                    break;
                case 2:
                    WaterAtk(skill[skillSlot]);
                    break;
                case 3:
                    PlantAtk(skill[skillSlot]);
                    break;
                case 4:
                    ElectricAtk(skill[skillSlot]);
                    break;
                case 5:
                    EarthAtk(skill[skillSlot]);
                    break;
                case 6:
                    PoisonAtk(skill[skillSlot]);
                    break;
                case -1:
                    StartCoroutine(DoNothingAtk());
                    break;
            }
        }
        else {
            PlayerAttack.instance.atkCDown[skillSlot] = 0.05f;
            PlayerAttack.instance.atkRemember = 0;
            AudioManager.instance.Play("DenyCastingSkill");
            StartCoroutine(DoNothingAtk());
        }
    }

    private void Cast(bool bol) {
        if (bol) {
            PlayerAttack.instance.currentAtk = 0;
            PlayerMovement.instance.moveLock = true;
            PlayerData.rbPlayer.velocity = Vector2.zero;
        }
        else {
            PlayerAttack.instance.isAtking = false;
            PlayerMovement.instance.moveLock = false;
            PlayerData.animPlayer.SetBool("Moving", false);
        }
    }

    private void BasicAtk(int atk) {
        switch (atk) {
            case 0:
                StartCoroutine(BasicAtk1Instantiate());
                break;
            case 1:
                StartCoroutine(DoNothingAtk());
                break;
            case 2:
                StartCoroutine(DoNothingAtk());
                break;
            case 3:
                StartCoroutine(DoNothingAtk());
                break;
            case 4:
                StartCoroutine(DoNothingAtk());
                break;
            default:
                StartCoroutine(DoNothingAtk());
                break;
        }
    }

    private void FireAtks(int atk) {
        switch (atk) {
            case 0:
                StartCoroutine(FireAtk1Instantiate());
                break;
            case 1:
                StartCoroutine(FireAtk2Instantiate());
                break;
            case 2:
                StartCoroutine(FireAtk3Instantiate());
                break;
            case 3:
                StartCoroutine(DoNothingAtk());
                break;
            case 4:
                StartCoroutine(FireAtk5Instatiate());
                break;
            case 5:
                StartCoroutine(DoNothingAtk());
                break;
            case 6:
                StartCoroutine(DoNothingAtk());
                break;
            case 7:
                StartCoroutine(DoNothingAtk());
                break;
            case 8:
                StartCoroutine(DoNothingAtk());
                break;
            default:
                StartCoroutine(DoNothingAtk());
                break;
        }
    }

    private void WaterAtk(int atk) {
        switch (atk) {
            case 0:
                StartCoroutine(WaterAtk1Instantiate()); //
                break;
            case 1:
                StartCoroutine(DoNothingAtk());
                break;
            case 2:
                StartCoroutine(DoNothingAtk());
                break;
            case 3:
                StartCoroutine(DoNothingAtk());
                break;
            case 4:
                StartCoroutine(DoNothingAtk());
                break;
            case 5:
                StartCoroutine(DoNothingAtk());
                break;
            case 6:
                StartCoroutine(DoNothingAtk());
                break;
            case 7:
                StartCoroutine(DoNothingAtk());
                break;
            case 8:
                StartCoroutine(DoNothingAtk());
                break;
            default:
                StartCoroutine(DoNothingAtk());
                break;
        }
    }

    private void PlantAtk(int atk) {
        switch (atk) {
            case 0:
                //StartCoroutine();
                break;
            case 1:
                //StartCoroutine();
                break;
            case 2:
                //StartCoroutine();
                break;
            case 3:
                //StartCoroutine();
                break;
            case 4:
                //StartCoroutine();
                break;
            case 5:
                //StartCoroutine();
                break;
            case 6:
                //StartCoroutine();
                break;
            case 7:
                //StartCoroutine();
                break;
            case 8:
                //StartCoroutine();
                break;
            default:
                StartCoroutine(DoNothingAtk());
                break;
        }
    }

    private void ElectricAtk(int atk) {
        switch (atk) {
            case 0:
                //StartCoroutine();
                break;
            case 1:
                //StartCoroutine();
                break;
            case 2:
                //StartCoroutine();
                break;
            case 3:
                //StartCoroutine();
                break;
            case 4:
                //StartCoroutine();
                break;
            case 5:
                //StartCoroutine();
                break;
            case 6:
                //StartCoroutine();
                break;
            case 7:
                //StartCoroutine();
                break;
            case 8:
                //StartCoroutine();
                break;
            default:
                StartCoroutine(DoNothingAtk());
                break;
        }
    }

    private void EarthAtk(int atk) {
        switch (atk) {
            case 0:
                //StartCoroutine();
                break;
            case 1:
                //StartCoroutine();
                break;
            case 2:
                //StartCoroutine();
                break;
            case 3:
                //StartCoroutine();
                break;
            case 4:
                //StartCoroutine();
                break;
            case 5:
                //StartCoroutine();
                break;
            case 6:
                //StartCoroutine();
                break;
            case 7:
                //StartCoroutine();
                break;
            case 8:
                //StartCoroutine();
                break;
            default:
                StartCoroutine(DoNothingAtk());
                break;
        }
    }

    private void PoisonAtk(int atk) {
        switch (atk) {
            case 0:
                //StartCoroutine();
                break;
            case 1:
                //StartCoroutine();
                break;
            case 2:
                //StartCoroutine();
                break;
            case 3:
                //StartCoroutine();
                break;
            case 4:
                //StartCoroutine();
                break;
            case 5:
                //StartCoroutine();
                break;
            case 6:
                //StartCoroutine();
                break;
            case 7:
                //StartCoroutine();
                break;
            case 8:
                //StartCoroutine();
                break;
            default:
                StartCoroutine(DoNothingAtk());
                break;
        }
    }

    //Basic
    private IEnumerator DoNothingAtk() {
        yield return new WaitForEndOfFrame();
        PlayerAttack.instance.isAtking = false;
        PlayerMovement.instance.moveLock = false;
    }

    private IEnumerator BasicAtk1Instantiate() {
        Cast(true);
        currentAtkDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        PlayerData.animPlayer.SetTrigger("Atk0");
        triggerCol.enabled = true;
        triggerState = 1;

        PlayerMovement.instance.lastDirection = currentAtkDirection;

        yield return new WaitForSeconds(0.1f);

        //Debug.Log("Movimento começado");
        PlayerData.rbPlayer.velocity = currentAtkDirection * strenghBasicAtkDash;

        yield return new WaitForSeconds(0.4f);

        //Debug.Log("Movimento interrompido");
        PlayerData.rbPlayer.velocity = Vector2.zero;
        triggerCol.enabled = false;
        for (int i = 0; i < hitEntities.Length; i++) hitEntities[i] = null;
        currentListPos = 0;
        triggerState = 0;

        yield return new WaitForSeconds(0.3f);

        //Debug.Log("Reganhou controle ");
        Cast(false);
    }

    //Fire
    private IEnumerator FireAtk1Instantiate() {
        Cast(true);
        PlayerMovement.instance.lastDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        PlayerData.animPlayer.SetTrigger("AtkF1");

        AtkFireBreath breathInstance = Instantiate(prefabFireAtk1, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(PlayerMovement.instance.lastDirection.y, PlayerMovement.instance.lastDirection.x) * Mathf.Rad2Deg) + 90)).GetComponent<AtkFireBreath>();
        breathInstance.direction = new Vector3(PlayerMovement.instance.lastDirection.x, PlayerMovement.instance.lastDirection.y, 0);
        breathInstance.damage = damageFireAtk1;

        yield return new WaitForSeconds(0.4f);

        AudioManager.instance.Play("PlayerAtkF1");
        breathInstance.GetComponent<AtkFireBreath>().Play();

        yield return new WaitForSeconds(0.4f);

        Cast(false);
    }

    private IEnumerator FireAtk2Instantiate() {
        Cast(true);
        PlayerMovement.instance.lastDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        PlayerData.animPlayer.SetTrigger("AtkF2");

        yield return new WaitForSeconds(0.2f);

        AtkFireBall go = Instantiate(prefabFireAtk2, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(PlayerMovement.instance.lastDirection.y, PlayerMovement.instance.lastDirection.x) * Mathf.Rad2Deg) + 180)).GetComponent<AtkFireBall>();
        go.damage = damageFireAtk2;
        go.GetComponent<Rigidbody2D>().velocity = PlayerMovement.instance.lastDirection * speedFireAtk2;
        Cast(false);
    }

    private IEnumerator FireAtk3Instantiate() {
        Cast(true);
        PlayerData.animPlayer.SetTrigger("AtkF3"); //
        currentAtkDirection = (mousePos - new Vector2(transform.position.x, transform.position.y));

        yield return new WaitForSeconds(0.3f);

        AtkEruption go = Instantiate(prefabFireAtk3, Vector2.Distance(transform.position, transform.position + currentAtkDirection) >= rangeFireAtk3 ? (transform.position + currentAtkDirection.normalized * rangeFireAtk3) : (transform.position + currentAtkDirection), Quaternion.Euler(0, 0, 90)).GetComponent<AtkEruption>();
        go.damage = damageFireAtk3;

        yield return new WaitForSeconds(0.05f);

        Cast(false);

    }

    private IEnumerator FireAtk5Instatiate() {
        Cast(true);
        PlayerData.animPlayer.SetTrigger("AtkF5");

        currentAtkDirection = (mousePos - new Vector2(transform.position.x, transform.position.y));
        PlayerMovement.instance.lastDirection = currentAtkDirection.normalized;

        yield return new WaitForSeconds(0.2f);

        AtkMeteor meteorInstance = Instantiate(prefabFireAtk5, transform.position + new Vector3(0, 0.75f, 0), Quaternion.Euler(0, 0, 180)).GetComponent<AtkMeteor>();
        meteorInstance.damage = damageFireAtk5;
        if (Vector2.Distance(transform.position, transform.position + currentAtkDirection) >= rangeFireAtk5) meteorInstance.finalPos = new Vector3(transform.position.x + currentAtkDirection.normalized.x * rangeFireAtk5, transform.position.y + currentAtkDirection.normalized.y * rangeFireAtk5, 0);
        else meteorInstance.finalPos = new Vector3(currentAtkDirection.x + transform.position.x, currentAtkDirection.y + transform.position.y, 0);

        Cast(false);
    }

    //Water
    private IEnumerator WaterAtk1Instantiate() {
        Cast(true);
        PlayerData.animPlayer.SetTrigger("AtkW1");
        AudioManager.instance.Play("PlayerAtkW1");

        AtkSplash go = Instantiate(prefabWaterAtk1, transform.position, Quaternion.identity).GetComponent<AtkSplash>();
        go.damage = damageWaterAtk1;
        
        yield return new WaitForSeconds(0.1f);

        Cast(false);

    }

    private void OnTriggerStay2D(Collider2D collision) {
        switch (triggerState) {
            case 0: break;
            case 1:
                bool bol = false;
                foreach (GameObject go in hitEntities) if (go == collision.gameObject.gameObject) bol = true;

                if (collision.GetComponent<EnemyBase>() != null && !bol) {
                    collision.GetComponent<EnemyBase>().TakeDamage(damageBasicAtk);
                    hitEntities[currentListPos] = collision.gameObject;
                    currentListPos++;
                }
                else if (collision.GetComponent<MovableBoulder>() != null && !bol) {
                    int i = (int)(Mathf.Atan2(PlayerMovement.instance.lastDirection.y, PlayerMovement.instance.lastDirection.x) * Mathf.Rad2Deg);
                    if (i <= 45 && i > -45) i = 0;
                    else if (i <= -45 && i >= -135) i = 1;
                    else if (i < -135 || i >= 135) i = 2;
                    else if (i < 135 && i > 45) i = 3;
                    collision.GetComponent<MovableBoulder>().SetTarget(i);
                    hitEntities[currentListPos] = collision.gameObject;
                    currentListPos++;
                }
                break;
            case 2:

                break;
        }
    }

}