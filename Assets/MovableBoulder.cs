using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBoulder : MonoBehaviour {

    public bool onPosition = false;
    public float moveDuration;
    public float correctionMargin = 0.1f;
    public Vector3 CorrectPos;
    private Vector3 targetPos;

    private void FixedUpdate() {
        MoveToPos();
    }

    private void MoveToPos() {
        if (transform.position != targetPos) {
            transform.position += (targetPos - transform.position) * Time.fixedDeltaTime / moveDuration;
            if (Mathf.Abs((targetPos - transform.position).x) < correctionMargin) transform.position = new Vector3(targetPos.x, transform.position.y, 0);
            if (Mathf.Abs((targetPos - transform.position).y) < correctionMargin) transform.position = new Vector3(transform.position.x, targetPos.y, 0);
        }
        onPosition = transform.position == CorrectPos;
    }

    public void SetTarget(int i) {
        switch (i) {
            case 0:
                targetPos = transform.position + new Vector3(1, 0, 0);
                break;
            case 1:
                targetPos = transform.position + new Vector3(0, -1, 0);
                break;
            case 2:
                targetPos = transform.position + new Vector3(-1, 0, 0);
                break;
            case 3:
                targetPos = transform.position + new Vector3(0, 1, 0);
                break;
        }
        Vector3 aimedPos = new Vector3((int)targetPos.x, (int)targetPos.y, 0);
        aimedPos += targetPos.x < 0 ? new Vector3(-0.5f, 0, 0) : new Vector3(0.5f, 0, 0);
        aimedPos += targetPos.y < 0 ? new Vector3(0, -0.5f, 0) : new Vector3(0, 0.5f, 0);
        targetPos = aimedPos;
    }
}
