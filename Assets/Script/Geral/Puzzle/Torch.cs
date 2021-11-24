using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Torch : MonoBehaviour {

    private Burnable burnableScript;
    private Animator animTorch;
    private Light2D light2D;

    private void Start() {
        burnableScript = GetComponent<Burnable>();
        animTorch = GetComponent<Animator>();
        light2D = GetComponent<Light2D>();
        GetComponent<SpriteRenderer>().sortingOrder = (int) (transform.position.y * -10);
    }

    private void Update() {
        light2D.enabled = burnableScript.lit;
        animTorch.SetBool("Lit", burnableScript.lit);
    }
}
