using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //

[System.Serializable]
public class Leveling {
    public string name;
    public int lv;
    public float xp;
    public bool[] unlockedSkill;
}

public class PlayerData : MonoBehaviour {

    [HideInInspector] public static PlayerData instance;

    public CanvasGroup deathCanvas;

    [Header("Components")]
    public static Rigidbody2D rbPlayer;
    public static Collider2D colPlayer; //
    public static Animator animPlayer;
    public static SpriteRenderer srPlayer;

    [Header("Stats")]
    public float maxHealth;
    //[System.NonSerialized]
    public float currentHealth;
    [System.NonSerialized] public bool blockState;

    [Header("Progression")]
    public Leveling[] leveling;
    public float normalXP;
    public int normalLv;
    public bool[] normalSkills = new bool[9];
    public float fireXP;
    public int fireLv;
    public bool[] fireSkills = new bool[9];
    public float waterXP;
    public int waterLv;
    public bool[] waterSkills = new bool[9];
    public float plantXP;
    public int plantLv;
    public bool[] plantSkills = new bool[9];
    public float electricXP;
    public int electricLv;
    public bool[] electricSkills = new bool[9];
    public float earthXP;
    public int earthLv;
    public bool[] earthSkills = new bool[9];
    public float poisonXP;
    public int poisonLv;
    public bool[] poisonSkills = new bool[9];

    public Material[] colorMaterial;

    private void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start() {
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        srPlayer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage) {
        if (blockState) currentHealth = currentHealth - 0;
        else currentHealth -= damage;
        if (animPlayer.GetBool("Consuming")) {
            GetComponent<SpriteRendererUpdater>().enabled = true;
            PlayerMovement.instance.moveLock = false;
        }
        animPlayer.SetBool("Consuming", false);
        PlayerAttack.instance.StopAllCoroutines();
        PlayerHUD.instance.DataUI();

        if (currentHealth <= 0) {
            StartCoroutine(Death());
        }
    }

    public void takeHealing(float heal) {
        currentHealth += heal;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        PlayerHUD.instance.DataUI();
    }

    IEnumerator Death() {
        animPlayer.SetTrigger("Death");
        deathCanvas.blocksRaycasts = true;
        deathCanvas.interactable = true;
        tag = "Untagged";
        foreach (Collider2D col in GetComponents<Collider2D>()) col.enabled = false;
        PlayerAttack.instance.StopAllCoroutines();
        GetComponent<PlayerAtkList>().StopAllCoroutines();
        GetComponent<PlayerMovement>().moveLock = false;

        yield return new WaitForSeconds(0.9f);

        while (deathCanvas.alpha < 1) { //
            deathCanvas.alpha += 0.01f;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //
    }

    public void ChangeColor(int color) {
        GetComponent<SpriteRenderer>().material = colorMaterial[color];
    }
}
