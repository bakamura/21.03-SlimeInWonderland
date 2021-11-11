using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class MovableBoulder : MonoBehaviour {

    private Rigidbody2D rbBoulder;
    private Animator animBoulder;
    private Light2D lightBoulder;

    public bool onPosition = false;
    public float moveDuration;
    public float correctionMargin = 0.1f;
    public Vector3 CorrectPos;
    private Vector3 targetPos;
    public Vector3[] AcceptedPos;

    private void Start() {
        rbBoulder = GetComponent<Rigidbody2D>();
        animBoulder = GetComponent<Animator>();
        lightBoulder = GetComponent<Light2D>();
        lightBoulder.enabled = false;

        targetPos = transform.position;
    }

    private void FixedUpdate() {
        if (!onPosition) MoveToPos();
    }

    private void MoveToPos() {
        if (transform.position != targetPos) {
            rbBoulder.velocity = (targetPos - transform.position).normalized / moveDuration;
            if (Mathf.Abs((targetPos - transform.position).x) < correctionMargin) transform.position = new Vector3(targetPos.x, transform.position.y, 0);
            if (Mathf.Abs((targetPos - transform.position).y) < correctionMargin) transform.position = new Vector3(transform.position.x, targetPos.y, 0);
            if (Mathf.Abs((targetPos - transform.position).x) < correctionMargin && Mathf.Abs((targetPos - transform.position).y) < correctionMargin) setRBody(false);
        }
        if (transform.position == CorrectPos) StartCoroutine(Drop());
    }

    IEnumerator Drop() {
        animBoulder.SetTrigger("Positioned");
        setRBody(false);

        yield return new WaitForSeconds(0.25f);

        onPosition = true;
        lightBoulder.enabled = true;
        GetComponent<Collider2D>().enabled = false;
    }

    public void SetTarget(int i) {
        if (!onPosition) {
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
            bool isAccepted = false;
            foreach (Vector3 pos in AcceptedPos) if (aimedPos == pos) isAccepted = true;
            targetPos = isAccepted ? aimedPos : transform.position; //
            setRBody(true);
        }
    }

    private void setRBody(bool bol) { //true = dynamic
        if (bol) {
            rbBoulder.bodyType = RigidbodyType2D.Dynamic;
            rbBoulder.mass = 100;
            rbBoulder.gravityScale = 0;
            rbBoulder.freezeRotation = true;
        }
        else rbBoulder.bodyType = RigidbodyType2D.Static;
    }
}
