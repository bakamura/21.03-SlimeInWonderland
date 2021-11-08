using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private Animator animPlayer;
    private PlayerData dataScript;
    private PlayerAtkList atkList;
    public SkillTreeCanvas treeCanvasScript;

    public float[] xpToGet = { 0, 0, 0, 0, 0, 0 };

    [Header("Atk General")]
    public bool canInput = true;
    [System.NonSerialized] public bool isAtking = false;
    [System.NonSerialized] public int currentAtk = 0;
    private float atkRemember;
    public float totalAtkRemember;

    public float[] atkTotalCDown = new float[3];
    [System.NonSerialized] public float[] atkCDown = new float[3];

    private void Start() {
        animPlayer = GetComponent<Animator>();
        dataScript = GetComponent<PlayerData>();

        atkList = GetComponent<PlayerAtkList>();
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
                        break;
                    case 1:
                        if (atkCDown[0] <= 0) {
                            isAtking = true;
                            atkList.CastSkill(0);
                            atkCDown[0] = atkTotalCDown[0];
                        }
                        break;
                    case 2:
                        if (atkCDown[1] <= 0) {
                            isAtking = true;
                            atkList.CastSkill(1);
                            atkCDown[1] = atkTotalCDown[1];
                        }
                        break;
                    case 3:
                        if (atkCDown[2] <= 0) {
                            isAtking = true;
                            atkList.CastSkill(2);
                            atkCDown[2] = atkTotalCDown[2];
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
