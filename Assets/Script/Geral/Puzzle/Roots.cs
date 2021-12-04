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

        yield return new WaitForSeconds(0.5f);

        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        GetComponent<SpriteRenderer>().enabled = false;
    }

}
