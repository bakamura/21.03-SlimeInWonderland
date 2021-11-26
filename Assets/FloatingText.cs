using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour {

    public int type;
    //[HideInInspector]
    public string text;

    private RectTransform rectTransform;
    [HideInInspector] public TextMeshProUGUI textComponent;
    private Vector3 baseScale;
    private float yFinalHeight;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        textComponent = GetComponent<TextMeshProUGUI>();

        StartCoroutine(Floating());
    }

    private IEnumerator Floating() {
        yield return new WaitForEndOfFrame();

        baseScale = rectTransform.localScale;
        yFinalHeight = rectTransform.position.y + 1;
        rectTransform.localScale = Vector3.zero;
        textComponent.text = text;

        switch (type) {
            case 0: //Xp
                rectTransform.localScale = baseScale;

                yield return new WaitForSeconds(1);

                break;
            case 1: //Cure
                textComponent.color = Color.green;
                while (rectTransform.localScale.x < baseScale.x) {
                    rectTransform.localScale += baseScale / 10;

                    yield return new WaitForSeconds(0.01f);
                }
                while (rectTransform.position.y < yFinalHeight) {
                    rectTransform.position += new Vector3(0, 0.01f, 0);
                    textComponent.color += new Color(0, 0, 0, -0.01f);

                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 2: //Damage
                while (rectTransform.localScale.x < baseScale.x * 1.5f) {
                    rectTransform.localScale += baseScale * 0.075f;

                    yield return new WaitForSeconds(0.01f);
                }
                while (rectTransform.localScale.x > baseScale.x) {
                    rectTransform.localScale += baseScale * -0.025f;

                    yield return new WaitForSeconds(0.01f);
                }
                yield return new WaitForSeconds(0.4f);
                while (textComponent.alpha > 0) {
                    textComponent.color = new Color(0, 0, 0, -0.05f);

                    yield return new WaitForSeconds(0.01f);
                }
                break;
        }

        Destroy(gameObject);
    }
}
