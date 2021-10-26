using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Torch : MonoBehaviour {

    private Animator animTorch;
    private Light2D light2D;
    public bool lit;

    private void Start() {
        animTorch = GetComponent<Animator>();
        light2D = GetComponent<Light2D>();
    }

    private void FixedUpdate() {
        CheckState();
    }

    private void CheckState() {
        light2D.enabled = lit;
        animTorch.SetBool("Lit", lit);
    }
}
