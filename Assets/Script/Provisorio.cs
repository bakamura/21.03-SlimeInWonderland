using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Provisorio : MonoBehaviour {

    public Torch[] torches;
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
        if (torches[0].lit && torches[1].lit) state = 1;
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
