using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    public Material[] colorMaterial;
    private int currentColor = 0;


    private void Update() {
        ChangeColor();
    }

    private void ChangeColor() {
        if (Input.GetKeyDown(KeyCode.C)) {
            if (currentColor < colorMaterial.Length - 1) currentColor++;
            else currentColor = 0;

            GetComponent<SpriteRenderer>().material = colorMaterial[currentColor];
        }
    }


}
