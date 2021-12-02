using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //

[System.Serializable]
public class Leveling {
    public string name;
    public int lv;
    public float xp;
    public bool[] unlockedSkill = new bool[9];
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
    [HideInInspector] public Transform textParent;
    public GameObject floatingText;

    [Header("Progression")]
    public Leveling[] leveling;

    [HideInInspector] public int currentMaterial = 0;
    public Material[] colorMaterial;
    public Material damageMaterial;

    private void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start() {
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        srPlayer = GetComponent<SpriteRenderer>();
        textParent = GameObject.FindGameObjectWithTag("TextCanvas").GetComponent<Transform>();

        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage) {
        if (blockState) currentHealth = currentHealth - 0;
        else {
            currentHealth -= damage;
            StartCoroutine(DamageFlash());

            if (animPlayer.GetBool("Consuming")) {
                GetComponent<SpriteRendererUpdater>().enabled = true;
                PlayerMovement.instance.moveLock = false;
            }
            animPlayer.SetBool("Consuming", false);
            PlayerAttack.instance.StopAllCoroutines();
            PlayerHUD.instance.DataUI();
        }
        if (currentHealth <= 0 && !animPlayer.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
            StartCoroutine(Death());
        }
    }

    public void takeHealing(float heal) {
        currentHealth += heal;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        PlayerHUD.instance.DataUI();
        FloatingText go = Instantiate(floatingText, transform.position + new Vector3(Random.Range(-0.45f, 0.45f), -0.25f, 0), Quaternion.identity).GetComponent<FloatingText>();
        go.transform.SetParent(textParent);
        go.type = 1;
        go.text = "+" + heal.ToString("F0");
    }

    private IEnumerator DamageFlash() {
        srPlayer.material = damageMaterial;

        yield return new WaitForSeconds(0.2f);

        srPlayer.material = colorMaterial[currentMaterial];
    }

    private IEnumerator Death() {
        animPlayer.SetTrigger("Death");
        deathCanvas.blocksRaycasts = true;
        deathCanvas.interactable = true;
        tag = "Untagged";
        foreach (Collider2D col in GetComponents<Collider2D>()) col.enabled = false;
        PlayerAttack.instance.StopAllCoroutines();
        PlayerAtkList.instance.StopAllCoroutines();
        PlayerMovement.instance.moveLock = true;
        PlayerAttack.instance.canInput = false;

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
