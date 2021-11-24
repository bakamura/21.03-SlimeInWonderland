using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    [HideInInspector] public static PlayerHUD instance;


    public Image healthBar;

    public Image[] atkIcon;
    public Image[] atkImageCDown;

    private void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }
    private void FixedUpdate() {
        AtkUI();
    }

    public void DataUI() {
        healthBar.fillAmount = PlayerData.instance.currentHealth / PlayerData.instance.maxHealth;
    }

    private void AtkUI() {
        if(PlayerAttack.instance.atkCDown[0] < PlayerAttack.instance.atkTotalCDown[0]) atkImageCDown[0].fillAmount = PlayerAttack.instance.atkCDown[0] / PlayerAttack.instance.atkTotalCDown[0];
        if (PlayerAttack.instance.atkCDown[1] < PlayerAttack.instance.atkTotalCDown[1]) atkImageCDown[1].fillAmount = PlayerAttack.instance.atkCDown[1] / PlayerAttack.instance.atkTotalCDown[1];
        if (PlayerAttack.instance.atkCDown[2] < PlayerAttack.instance.atkTotalCDown[2]) atkImageCDown[2].fillAmount = PlayerAttack.instance.atkCDown[2] / PlayerAttack.instance.atkTotalCDown[2];
    }

}
