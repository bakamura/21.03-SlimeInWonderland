using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    [HideInInspector] public static PlayerHUD instance;

    private PlayerData dataScript;
    private PlayerAttack atkScript;

    public Image healthBar;

    public Image[] atkIcon;
    public Image[] atkImageCDown;

    private void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start() {
        dataScript = GetComponent<PlayerData>();
        atkScript = GetComponent<PlayerAttack>();
    }

    private void FixedUpdate() {
        DataUI();
        AtkUI();
    }

    private void DataUI() {
        healthBar.fillAmount = dataScript.currentHealth / dataScript.maxHealth;
    }

    private void AtkUI() {
        if(atkScript.atkCDown[0] < atkScript.atkTotalCDown[0]) atkImageCDown[0].fillAmount = atkScript.atkCDown[0] / atkScript.atkTotalCDown[0];
        if (atkScript.atkCDown[1] < atkScript.atkTotalCDown[1]) atkImageCDown[1].fillAmount = atkScript.atkCDown[1] / atkScript.atkTotalCDown[1];
        if (atkScript.atkCDown[2] < atkScript.atkTotalCDown[2]) atkImageCDown[2].fillAmount = atkScript.atkCDown[2] / atkScript.atkTotalCDown[2];
    }

}
