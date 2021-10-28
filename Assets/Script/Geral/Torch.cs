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
        ChangeState(lit);
    }

    public void ChangeState(bool bol) {
        lit = bol;
        light2D.enabled = bol;
        animTorch.SetBool("Lit", bol);
    }
}
