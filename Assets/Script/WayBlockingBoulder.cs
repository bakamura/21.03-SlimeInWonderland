using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayBlockingBoulder : MonoBehaviour {

    public MovableBoulder[] boulders;
    public Vector3 targetPos;
    private Vector3 startPos;
    private float currentPos = 0;
    private int state = 0;

    private void Start() {
        startPos = transform.position;
    }

    private void FixedUpdate() {
        if (state == 0) CheckState();
        else if (state == 1) MoveAway();
    }

    private void CheckState() {
        int i = boulders.Length;
        foreach (MovableBoulder boulder in boulders) if(boulder.onPosition) i -= 1; 
        if(i == 0) state = 1;
    }

    private void MoveAway() {
        currentPos += Time.fixedDeltaTime;
        if (currentPos >= 1) {
            currentPos = 1;
            state = 2;
        }
        transform.position = Vector3.Lerp(startPos, targetPos, currentPos);

    }

}
