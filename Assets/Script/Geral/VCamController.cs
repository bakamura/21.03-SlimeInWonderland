using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCamController : MonoBehaviour {

    [Header("Settings")]
    public Collider2D[] camColliders;
    private int currentVCam = -1;
    private Animator animatorVCam;
    private GameObject playerGO;

    private void Start() {
        animatorVCam = GetComponent<Animator>();
        playerGO = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate() {
        for (int i = 0; i < camColliders.Length; i++) if (camColliders[i].IsTouching(playerGO.GetComponent<Collider2D>())) currentVCam = i;
        if (currentVCam >= 0) {
            animatorVCam.SetInteger("CurrentVCam", currentVCam);
            currentVCam = -1;
        }
    }
}
