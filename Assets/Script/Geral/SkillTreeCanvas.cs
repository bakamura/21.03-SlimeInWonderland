using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class TreeClass {
    public Sprite[] skillImage;
}

public class SkillTreeCanvas : MonoBehaviour {

    private PlayerData dataScript;
    private PlayerMovement movementScipt;
    private PlayerAttack atkScript;
    private PlayerAtkList atkListScript;
    private PlayerHUD hudScript;
    [SerializeField] private Image[] imgChild;

    public bool canOpenTab;
    public CanvasGroup hudCanvas;
    private CanvasGroup treeCanvas;
    public ScrollRect scrollRect;
    public TreeClass[] skillImage; //skillN, treeN

    private int lastTree = -1;
    private int[] currentButtonSelected = { -1, -1 };

    public Image[] skillSlotImage;
    public Image currentBtnImg;

    private void Start() {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        dataScript = playerGO.GetComponent<PlayerData>();
        movementScipt = playerGO.GetComponent<PlayerMovement>();
        atkScript = playerGO.GetComponent<PlayerAttack>();
        atkListScript = playerGO.GetComponent<PlayerAtkList>();
        hudScript = playerGO.GetComponent<PlayerHUD>();
        imgChild = GetComponentsInChildren<Image>();

        int[] x = new int[3];
        int y = 0;
        for (int i = 0; i < imgChild.Length; i++) foreach (Image img in skillSlotImage) if (imgChild[i] == img) {
                    x[y] = i;
                    y++;
                    if (y == 3) i = imgChild.Length;
                }

        treeCanvas = GetComponent<CanvasGroup>();
        AlternateCanvas(treeCanvas, false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (treeCanvas.interactable) {
                AlternateCanvas(treeCanvas, false);

                hudScript.atkIcon[0].sprite = skillSlotImage[0].sprite;
                hudScript.atkIcon[1].sprite = skillSlotImage[1].sprite;
                hudScript.atkIcon[2].sprite = skillSlotImage[2].sprite;

                Time.timeScale = 1;
                movementScipt.moveLock = false;
                atkScript.canInput = true;
                AlternateCanvas(hudCanvas, true);

                if (currentButtonSelected[0] != -1) {
                    FocusOnClick(false);
                    currentButtonSelected = new int[] { -1, -1 };
                }
            }
            else {
                AlternateCanvas(treeCanvas, true);
                Time.timeScale = 0.01f;
                movementScipt.moveLock = true;
                atkScript.canInput = false;
                AlternateCanvas(hudCanvas, false);
            }
        }
    }

    private void AlternateCanvas(CanvasGroup canvas, bool on) {
        canvas.alpha = on ? 1 : 0;
        canvas.interactable = on;
        canvas.blocksRaycasts = on;
    }

    public void SkillButton(int treeN) {
        lastTree = currentButtonSelected[1];
        currentButtonSelected[1] = treeN;
    }

    public void SkillButton2(int skillN) {
        string str = "";
        foreach(bool i in GetTree(currentButtonSelected[1])) str += i? "T " : "F ";
        if (GetTree(currentButtonSelected[1])[skillN] == true) {
            if (currentButtonSelected[0] != skillN || currentButtonSelected[1] != lastTree) {
                if (currentButtonSelected[0] != -1) {
                    ChangeColor(currentBtnImg, 0.5f);
                    ChangeColor(currentBtnImg.transform.GetComponentInChildren<Image>(), 0.5f);
                    currentBtnImg = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Image>();
                    ChangeColor(currentBtnImg, 2);
                    ChangeColor(currentBtnImg.transform.GetComponentInChildren<Image>(), 2);
                }
                if (currentButtonSelected[0] == -1) {
                    currentBtnImg = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Image>();
                    FocusOnClick(true);
                }

                currentButtonSelected[0] = skillN;
            }
            else {
                FocusOnClick(false);
                currentButtonSelected[0] = -1;
            }
        }
        else {
            currentButtonSelected[1] = lastTree;
            //Play sound
        }
    }

    private bool[] GetTree(int i) {
        switch (i) {
            case 0: return dataScript.normalSkills;
            case 1: return dataScript.fireSkills;
            case 2: return dataScript.waterSkills;
            case 3: return dataScript.plantSkills;
            case 4: return dataScript.electricSkills;
            case 5: return dataScript.earthSkills;
            case 6: return dataScript.poisonSkills;
            default:
                Debug.Log("treeGet Error");
                return null;
        }
    }

    public void SKillSlotButton(int slotN) {
        //Make equiping skills that are already equiped in another slot, just swap
        if (currentButtonSelected[0] != -1) {
            if (atkListScript.skill[slotN] != currentButtonSelected[0] || atkListScript.tree[slotN] != currentButtonSelected[1]) {
                atkListScript.skill[slotN] = currentButtonSelected[0];
                atkListScript.tree[slotN] = currentButtonSelected[1];
                skillSlotImage[slotN].sprite = skillImage[currentButtonSelected[1]].skillImage[currentButtonSelected[0]];
                atkScript.atkTotalCDown[slotN] = atkListScript.treeCoolDowns[currentButtonSelected[1]].coolDown[currentButtonSelected[0]];
                currentButtonSelected[0] = -1;
                FocusOnClick(false);
            }
            else if (atkListScript.skill[slotN] == currentButtonSelected[0] && atkListScript.tree[slotN] == currentButtonSelected[1]) {
                currentButtonSelected[0] = -1;
                FocusOnClick(false);
            }
        }
    }

    private void FocusOnClick(bool bol) {
        Debug.Log(bol ? "Focused" : "Unfocused");
        scrollRect.enabled = bol;
        if (bol) {
            foreach (Image img in imgChild) ChangeColor(img, 0.5f);

            foreach (Image img in skillSlotImage) {
                ChangeColor(img.transform.parent.GetComponent<Image>(), 2);
                ChangeColor(img, 2);
            }
            ChangeColor(currentBtnImg, 2);
            ChangeColor(currentBtnImg.transform.GetComponentInChildren<Image>(), 2);
        }
        else {
            foreach (Image img in skillSlotImage) {
                ChangeColor(img.transform.parent.GetComponent<Image>(), 2);
                ChangeColor(img, 0.5f);
            }
            ChangeColor(currentBtnImg, 0.5f);
            ChangeColor(currentBtnImg.transform.GetComponentInChildren<Image>(), 0.5f);

            foreach (Image img in imgChild) ChangeColor(img, 2);
        }
    }

    private void ChangeColor(Image img, float mtply) {
        img.color = img.color * mtply;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
    }

}