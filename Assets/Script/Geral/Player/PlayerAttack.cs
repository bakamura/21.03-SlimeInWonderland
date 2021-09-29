using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private float atk0CDown;
    public float atk0TotalCDown;
    private float atk1CDown;
    public float atk1TotalCDown;
    private float atk2CDown;
    public float atk2TotalCDown;
    private float atkRemember;
    public float totalAtkRemember;

    private void Update() {
        Inputs();
    }

    private void Inputs() {
        if (Input.GetButtonDown("Fire1")) atkRemember = totalAtkRemember;

    }

    private void FixedUpdate() {
        
        CoolDown();
    }

    private void Attack() {

    }

    private void CoolDown() {
        if (atk0CDown > 0) atk0CDown -= Time.fixedDeltaTime;
        if (atk1CDown > 0) atk1CDown -= Time.fixedDeltaTime;
        if (atk2CDown > 0) atk2CDown -= Time.fixedDeltaTime;
    }
}
