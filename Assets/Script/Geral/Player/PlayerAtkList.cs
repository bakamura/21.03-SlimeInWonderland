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
    private Vector2 currentAtkDirection;

    [Header("AttackList")]
    public int[] skill = new int[3];
    public int[] tree = new int[3];
    public CoolDown[] treeCoolDowns;

    [Header("Trigger")]
    private int triggerState = 0;
    private GameObject[] hitEntities = new GameObject[64];
    private int currentListPos = 0;


    [Header("Basic Atk")]
    public float damageBasicAtk;
    public float strenghBasicAtkDash;

    [Header("FireBreath")]
    public float damageFireAtk1;
    public GameObject prefabFireAtk1;

    [Header("Meteor I")]
    public float damageFireAtk5;
    public float rangeFireAtk5;
    public GameObject prefabFireAtk5;

    private void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start() {
        skill[0] = 0;
        skill[1] = -1;
        skill[2] = -1;
        tree[0] = 0;
        tree[1] = -1; //null
        tree[2] = -1; //null
    }

    private void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void CastSkill(int skillSlot) {
        FindTree(tree[skillSlot], skill[skillSlot]);
    }

    private void FindTree(int tree, int skill) {
        switch (tree) {
            case 0:
                BasicAtk(skill);
                break;
            case 1:
                FireAtks(skill);
                break;
            case 2:
                WaterAtk(skill);
                break;
            case 3:
                PlantAtk(skill);
                break;
            case 4:
                ElectricAtk(skill);
                break;
            case 5:
                EarthAtk(skill);
                break;
            case 6:
                PoisonAtk(skill);
                break;
            case -1:
                StartCoroutine(DoNothingAtk());
                break;
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

    private void FireAtks(int atk) {
        switch (atk) {
            case 0:
                StartCoroutine(FireAtk1Instantiate());
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
    IEnumerator DoNothingAtk() {
        yield return new WaitForEndOfFrame();
        PlayerAttack.instance.isAtking = false;
        PlayerMovement.instance.moveLock = false;
    }

    IEnumerator BasicAtk1Instantiate() {
        currentAtkDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        PlayerMovement.instance.moveLock = true;
        PlayerData.instance.rbPlayer.velocity = Vector2.zero;
        PlayerData.instance.animPlayer.SetTrigger("Atk0");
        PlayerAttack.instance.currentAtk = 0;
        triggerCol.enabled = true;
        triggerState = 1;

        PlayerMovement.instance.lastDirection = currentAtkDirection;

        yield return new WaitForSeconds(0.1f);

        Debug.Log("Movimento começado");
        PlayerData.instance.rbPlayer.velocity = currentAtkDirection * strenghBasicAtkDash;

        yield return new WaitForSeconds(0.4f);

        Debug.Log("Movimento interrompido");
        PlayerData.instance.rbPlayer.velocity = Vector2.zero;
        triggerCol.enabled = false;
        for (int i = 0; i < hitEntities.Length; i++) hitEntities[i] = null;
        currentListPos = 0;
        triggerState = 0;

        yield return new WaitForSeconds(0.3f);

        Debug.Log("Reganhou controle ");
        PlayerAttack.instance.isAtking = false;
        PlayerMovement.instance.moveLock = false;
        PlayerData.instance.animPlayer.SetBool("Moving", false);
    }

    //Fire
    IEnumerator FireAtk1Instantiate() {
        PlayerMovement.instance.moveLock = true;
        PlayerData.instance.rbPlayer.velocity = Vector2.zero;
        PlayerData.instance.animPlayer.SetTrigger("AtkF1");
        PlayerAttack.instance.currentAtk = 0;

        PlayerMovement.instance.lastDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        GameObject breathInstance = Instantiate(prefabFireAtk1, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(PlayerMovement.instance.lastDirection.y, PlayerMovement.instance.lastDirection.x) * Mathf.Rad2Deg) + 90));
        breathInstance.GetComponent<AtkFireBreath>().direction = new Vector3(PlayerMovement.instance.lastDirection.x, PlayerMovement.instance.lastDirection.y, 0);
        breathInstance.GetComponent<AtkFireBreath>().damage = damageFireAtk1;

        yield return new WaitForSeconds(0.4f);

        breathInstance.GetComponent<AtkFireBreath>().Play();

        yield return new WaitForSeconds(0.4f);

        PlayerAttack.instance.isAtking = false;
        PlayerMovement.instance.moveLock = false;
        PlayerData.instance.animPlayer.SetBool("Moving", false);
    }

    IEnumerator FireAtk5Instatiate() {
        PlayerMovement.instance.moveLock = true;
        PlayerData.instance.rbPlayer.velocity = Vector2.zero;
        PlayerData.instance.animPlayer.SetTrigger("AtkF5");
        PlayerAttack.instance.currentAtk = 0;

        currentAtkDirection = (mousePos - new Vector2(transform.position.x, transform.position.y));
        PlayerMovement.instance.lastDirection = currentAtkDirection.normalized;

        yield return new WaitForSeconds(0.2f);

        PlayerAttack.instance.isAtking = false;
        PlayerMovement.instance.moveLock = false;
        PlayerData.instance.animPlayer.SetBool("Moving", false);
        GameObject meteorInstance = Instantiate(prefabFireAtk5, transform.position + new Vector3(0, 0.75f, 0), Quaternion.Euler(0, 0, 180));
        meteorInstance.GetComponent<AtkMeteor>().damage = damageFireAtk5;
        if (Vector2.Distance(transform.position, new Vector2(transform.position.x, transform.position.y) + currentAtkDirection) >= rangeFireAtk5) meteorInstance.GetComponent<AtkMeteor>().finalPos = new Vector3(transform.position.x + currentAtkDirection.normalized.x * rangeFireAtk5, transform.position.y + currentAtkDirection.normalized.y * rangeFireAtk5, 0);
        else meteorInstance.GetComponent<AtkMeteor>().finalPos = new Vector3(currentAtkDirection.x + transform.position.x, currentAtkDirection.y + transform.position.y, 0);
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