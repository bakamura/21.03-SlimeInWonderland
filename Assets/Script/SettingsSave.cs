using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSave : MonoBehaviour {

    public static GameObject instance = null;
    public static float volumeMaster = 1f, volumeMusic = 0.5f, volumeSfx = 0.5f;

    private void Awake() {
        if (instance == null) instance = this.gameObject;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

}
