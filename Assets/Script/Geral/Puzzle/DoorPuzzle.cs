using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DoorPuzzle : MonoBehaviour {

    public Collider2D deactivateCol;
    public MovableBoulder[] boulders;
    private int state = 0;

    private void FixedUpdate() {
        if (state == 0) CheckState();
        else if (state == 1) StartCoroutine(Open());
    }

    private void CheckState() {
        int i = boulders.Length;
        foreach (MovableBoulder boulder in boulders) if (boulder.onPosition) i -= 1;
        if (i == 0) state = 1;
    }

    private IEnumerator Open() {
        GetComponent<Animator>().SetTrigger("Open");
        AudioManager.instance.Play("OpenDoorPuzzle");
        state = 2;

        yield return new WaitForSeconds(0.5f);

        foreach (Light2D light in GetComponents<Light2D>()) light.enabled = true;
        deactivateCol.enabled = false;
    }

}
