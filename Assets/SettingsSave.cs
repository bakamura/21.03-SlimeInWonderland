using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSave : MonoBehaviour {

    public static float volume;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

}
