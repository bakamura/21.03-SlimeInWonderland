using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererUpdater : MonoBehaviour {

    private SpriteRenderer sr;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        sr.sortingOrder = (int) (-10 * transform.position.y);
    }
}
