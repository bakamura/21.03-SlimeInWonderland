using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    IEnumerator Open() {
        GetComponent<Animator>().SetTrigger("Open");
        state = 2;

        yield return new WaitForSeconds(0.5f);

        deactivateCol.enabled = false;
    }

}
