using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour {

    public CanvasGroup mainCanvas, savesCanvas, configsCanvas, confirmQuitCanvas;
    public AudioMixer masterMixer;
    public TMP_Dropdown resSelector;
    public bool fullscreen;

    private void Start() {
        DeactivateAllCanvas();
        ActivateCanvas(mainCanvas, true);
    }

    private void DeactivateAllCanvas() {
        ActivateCanvas(mainCanvas, false);
        ActivateCanvas(savesCanvas, false);
        ActivateCanvas(configsCanvas, false);
        ActivateCanvas(confirmQuitCanvas, false);
    }

    private void ActivateCanvas(CanvasGroup canvas,bool bol) {
        canvas.alpha = bol ? 1 : 0;
        canvas.interactable = bol;
        canvas.blocksRaycasts = bol;
    }

    //Buttons

    public void Return2Main() {
        DeactivateAllCanvas();
        ActivateCanvas(mainCanvas, true);
    }

    public void PlayButton() {
        DeactivateAllCanvas();
        ActivateCanvas(savesCanvas, true);
    }

    public void SaveButton(int i) {
        switch (i) {
            case 0:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 1:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 2:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
    }

    public void ConfigsButton() {
        DeactivateAllCanvas();
        ActivateCanvas(configsCanvas, true);
    }

    public void ChangeVolume(float f) {
        SettingsSave.volume = Mathf.Log10(f) * 20;
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(f) * 20);
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

    public void QuitButton() {
        mainCanvas.interactable = false;
        mainCanvas.blocksRaycasts = false;
        ActivateCanvas(confirmQuitCanvas, true);
    }

    public void ConfirmQuitButton() {
        Application.Quit();
    }

}
