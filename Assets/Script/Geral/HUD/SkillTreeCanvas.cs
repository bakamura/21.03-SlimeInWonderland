using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI[] lvText;

    private int lastTree = -1;
    private int[] currentButtonSelected = { -1, -1 };

    public Image[] skillSlotImage;
    private Image currentBtnImg;

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

                lvText[0].text = dataScript.normalLv.ToString();
                lvText[1].text = dataScript.fireLv.ToString();
                lvText[2].text = dataScript.waterLv.ToString();
                lvText[3].text = dataScript.plantLv.ToString();
                //lvText[4].text = dataScript.electricLv.ToString();
                //lvText[5].text = dataScript.earthLv.ToString();
                //lvText[6].text = dataScript.poisonLv.ToString();

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
        foreach (bool i in GetTree(currentButtonSelected[1])) str += i ? "T " : "F ";

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
            int spentPoints = 0;
            foreach (bool bol in GetTree(currentButtonSelected[1])) spentPoints += bol ? 1 : 0;
            switch (currentButtonSelected[1]) {
                case 0:
                    if (spentPoints < dataScript.normalLv) {
                        GetTree(0)[skillN] = true;
                        AudioManager.instance.Play("UnlockSkill");
                        FocusOnClick(false);
                    }
                    else {
                        AudioManager.instance.Play("DenyUnlock");
                        currentButtonSelected[1] = lastTree;
                    }
                    break;
                case 1:
                    if (spentPoints < dataScript.fireLv) {
                        GetTree(1)[skillN] = true;
                        AudioManager.instance.Play("UnlockSkill");
                        FocusOnClick(false);
                    }
                    else {
                        AudioManager.instance.Play("DenyUnlock");
                        currentButtonSelected[1] = lastTree;
                    }
                    break;
                case 2:
                    if (spentPoints < dataScript.waterLv) {
                        GetTree(2)[skillN] = true;
                        AudioManager.instance.Play("UnlockSkill");
                        FocusOnClick(false);
                    }
                    else {
                        AudioManager.instance.Play("DenyUnlock");
                        currentButtonSelected[1] = lastTree;
                    }
                    break;
                case 3:
                    if (spentPoints < dataScript.plantLv) {
                        GetTree(3)[skillN] = true;
                        AudioManager.instance.Play("UnlockSkill");
                        FocusOnClick(false);
                    }
                    else {
                        AudioManager.instance.Play("DenyUnlock");
                        currentButtonSelected[1] = lastTree;
                    }
                    break;
                case 4:
                    if (spentPoints < dataScript.electricLv) {
                        GetTree(4)[skillN] = true;
                        AudioManager.instance.Play("UnlockSkill");
                        FocusOnClick(false);
                    }
                    else {
                        AudioManager.instance.Play("DenyUnlock");
                        currentButtonSelected[1] = lastTree;
                    }
                    break;
                case 5:
                    if (spentPoints < dataScript.earthLv) {
                        GetTree(5)[skillN] = true;
                        AudioManager.instance.Play("UnlockSkill");
                        FocusOnClick(false);
                    }
                    else {
                        AudioManager.instance.Play("DenyUnlock");
                        currentButtonSelected[1] = lastTree;
                    }
                    break;
                case 6:
                    if (spentPoints < dataScript.poisonLv) {
                        GetTree(6)[skillN] = true;
                        AudioManager.instance.Play("UnlockSkill");
                        FocusOnClick(false);
                    }
                    else {
                        AudioManager.instance.Play("DenyUnlock");
                        currentButtonSelected[1] = lastTree;
                    }
                    break;
            }
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
        img.color = Color.white * mtply;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
    }

}