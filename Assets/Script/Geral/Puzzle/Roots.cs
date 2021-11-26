using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roots : MonoBehaviour {

    private Burnable burnableScript;
    private Animator animRoot;
    private bool burned = false;

    private void Start() {
        burnableScript = GetComponent<Burnable>();
        animRoot = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        if (!burned && burnableScript.lit) StartCoroutine(GetBurnt());
    }

    private IEnumerator GetBurnt() {
        animRoot.SetTrigger("Burn");
        burned = true;

        yield return new WaitForSeconds(1);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

}
