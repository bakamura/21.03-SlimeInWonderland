using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;

public class PauseCanvas : MonoBehaviour {

    private CanvasGroup pauseCanvas;
    public CanvasGroup hudCanvas, treeCanvas, mainGroup, settingsGroup;

    public AudioMixer masterMixer;
    public Light2D globalLight;
    public TMP_Dropdown resSelector;
    private bool fullscreen = false;

    private void Start() {
        pauseCanvas = GetComponent<CanvasGroup>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pauseCanvas.interactable) ContinueButton();
            else if (PlayerData.instance.currentHealth > 0) { 
                AlternateCanvas(pauseCanvas, true);
                AlternateCanvas(mainGroup, true);
                AlternateCanvas(settingsGroup, false);
                if (treeCanvas.interactable) {
                    AlternateCanvas(treeCanvas, false);
                    treeCanvas.GetComponent<SkillTreeCanvas>().HUDIconUpdate();
                }
                AlternateCanvas(hudCanvas, false);

                Time.timeScale = 0;
                PlayerMovement.instance.moveLock = true;
                PlayerAttack.instance.canInput = false;

            }
        }
    }

    public void ContinueButton() {
        AlternateCanvas(pauseCanvas, false);
        AlternateCanvas(hudCanvas, true);

        Time.timeScale = 1;
        PlayerMovement.instance.moveLock = false;
        PlayerAttack.instance.canInput = true;
    }

    public void SettingsButton(bool bol) {
        AlternateCanvas(mainGroup, !bol);
        AlternateCanvas(settingsGroup, bol);
    }

    public void QuitButton() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0); //Load Main Menu
    }

    public void ChangeVolume(float f) {
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(f) * 20);
    }

    public void ChangeBrightness(float f) {
        globalLight.intensity = f;
    }

    public void ChangeResolution() {
        switch (resSelector.value) {
            case 0:
                Screen.SetResolution(640, 360, fullscreen);
                break;
            case 1:
                Screen.SetResolution(854, 480, fullscreen);
                break;
            case 2:
                Screen.SetResolution(1280, 720, fullscreen);
                break;
            case 3:
                Screen.SetResolution(1920, 1080, fullscreen);
                break;
        }
    }

    public void ChangeFullScreen() {
        fullscreen = !fullscreen;
        ChangeResolution();
    }

    private void AlternateCanvas(CanvasGroup canvas, bool on) {
        canvas.alpha = on ? 1 : 0;
        canvas.interactable = on;
        canvas.blocksRaycasts = on;
    }
}
