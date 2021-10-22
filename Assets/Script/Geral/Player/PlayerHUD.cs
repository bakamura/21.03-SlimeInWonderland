using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    private PlayerData dataScript;
    private PlayerAttack atkScript;

    public Image healthBar;

    public Image[] atkIcon;
    public Image[] atkImageCDown;

    private void Start() {
        dataScript = GetComponent<PlayerData>();
        atkScript = GetComponent<PlayerAttack>();
    }

    private void FixedUpdate() {
        DataUI();
        AtkUI();
    }

    private void DataUI() {
        healthBar.fillAmount = dataScript.currentHealth/dataScript.maxHealth;   
    }

    private void AtkUI() {
        atkImageCDown[0].fillAmount = atkScript.atk0CDown / atkScript.atk0TotalCDown;
        atkImageCDown[1].fillAmount = atkScript.atk1CDown / atkScript.atk1TotalCDown;
        atkImageCDown[2].fillAmount = atkScript.atk2CDown / atkScript.atk2TotalCDown;
    }

}
