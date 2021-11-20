using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour {

    public Animator animHolder;
    private float counter = 0;
    public float circleRange;

    private void Start() {
        StartCoroutine(ShowWASD());
    }

    private void Update() {
        if (animHolder.GetInteger("State") <= 1 && (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)) {
            counter += Time.deltaTime;
            if (counter >= 0.75f) animHolder.SetInteger("State", 2);
        }
        else if (animHolder.GetInteger("State") == 2) {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, circleRange, Vector2.zero);
            foreach (RaycastHit2D hit in hits) if (hit.collider.tag == "Enemy") {
                    animHolder.SetInteger("State", 3);
                    Time.timeScale = 0.001f;
                    break;
                }
        }
        else if (animHolder.GetInteger("State") == 3) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                Time.timeScale = 1;
                animHolder.SetInteger("State", 4);
            }
        }

        else if (animHolder.GetInteger("State") == 10) Destroy(gameObject);
    }

    IEnumerator ShowWASD() {
        yield return new WaitForSeconds(0.75f);

        if (counter < 0.5f) {
            counter = 0;
            animHolder.SetInteger("State", 1);
        }
    }


}
