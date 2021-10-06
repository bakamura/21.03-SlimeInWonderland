using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    private PlayerData dataScript;
    public Image healthBar;

    private PlayerAttack atkScript;
    public Image atk0ImageCDown;
    public Image atk1ImageCDown;
    public Image atk2ImageCDown;

    private void Start() {
        dataScript = GetComponent<PlayerData>();
        atkScript = GetComponent<PlayerAttack>();
    }

    private void FixedUpdate() {
        DataUI();
        AtkUI();
    }

    private void DataUI() {
        
    }

    private void AtkUI() {
        atk0ImageCDown.fillAmount = (atkScript.atk0TotalCDown - atkScript.atk0CDown) / atkScript.atk0TotalCDown;
        atk1ImageCDown.fillAmount = (atkScript.atk1TotalCDown - atkScript.atk1CDown) / atkScript.atk0TotalCDown;
        atk2ImageCDown.fillAmount = (atkScript.atk2TotalCDown - atkScript.atk2CDown) / atkScript.atk0TotalCDown;
    }

}
