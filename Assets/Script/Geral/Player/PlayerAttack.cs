using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private Animator animPlayer;
    private PlayerData dataScript;
    private PlayerAtkList atkList;

    public float[] xpToGet = { 0, 0, 0, 0, 0, 0 };

    [Header("Atk General")]
    public bool canInput = true;
    [System.NonSerialized] public bool isAtking = false;
    [System.NonSerialized] public int currentAtk = 0;
    private float atkRemember;
    public float totalAtkRemember;
    delegate void AtkMethod();

    public float atk0TotalCDown;
    [System.NonSerialized] public float atk0CDown;
    AtkMethod atk0;
    public float atk1TotalCDown;
    [System.NonSerialized] public float atk1CDown;
    AtkMethod atk1;
    public float atk2TotalCDown;
    [System.NonSerialized] public float atk2CDown;
    AtkMethod atk2;

    private void Start() {
        animPlayer = GetComponent<Animator>();
        dataScript = GetComponent<PlayerData>();

        atkList = GetComponent<PlayerAtkList>();
        atk0 = atkList.BasicAtk;
        atk1 = atkList.FireAtk1;
        atk2 = atkList.FireAtk32;
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
            if (Input.GetKeyDown(KeyCode.F)) {  
                animPlayer.SetBool("Consuming", true);
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
                        return;
                    case 1:
                        if (atk0CDown <= 0) {
                            isAtking = true;
                            atk0();
                            atk0CDown = atk0TotalCDown;
                        }
                        return;
                    case 2:
                        if (atk1CDown <= 0) {
                            isAtking = true;
                            atk1();
                            atk1CDown = atk1TotalCDown;
                        }
                        return;
                    case 3:
                        if (atk2CDown <= 0) {
                            isAtking = true;
                            atk2();
                            atk2CDown = atk2TotalCDown;
                        }
                        return;
                }
            }
        }
    }

    private void CoolDown() {
        if (atk0CDown > 0) atk0CDown -= Time.fixedDeltaTime;
        if (atk0CDown < 0) atk0CDown = 0;
        if (atk1CDown > 0) atk1CDown -= Time.fixedDeltaTime;
        if (atk1CDown < 0) atk1CDown = 0;
        if (atk2CDown > 0) atk2CDown -= Time.fixedDeltaTime;
        if (atk2CDown < 0) atk2CDown = 0;
    }



    //Animator
    public void GetXp() {
        dataScript.fireXP += xpToGet[0];
        dataScript.waterXP += xpToGet[1];
        dataScript.plantXP += xpToGet[2];
        dataScript.electricXP += xpToGet[3];
        dataScript.earthXP += xpToGet[4];
        dataScript.poisonXP += xpToGet[5];

        for (int i = 0; i < xpToGet.Length; i++) xpToGet[i] = 0;
        animPlayer.SetBool("Consuming", false);
    }
}
