using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTitle : MonoBehaviour {

    public float parallaxSpeed;
    public Transform[] parallaxImages;
    public float[] parallaxMultiplier;

    private void Update() {
        for (int i = 0; i < parallaxImages.Length; i++) {
            parallaxImages[i].position += new Vector3(parallaxSpeed * parallaxMultiplier[i] * Time.deltaTime, 0, 0);
            if (parallaxImages[i].position.x > 8) parallaxImages[i].position += new Vector3(-10, 0, 0);
        }
    }
}
