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

    private List<Image> imgChild;

    public bool canOpenTab;
    public CanvasGroup hudCanvas, pauseCanvas;
    private CanvasGroup treeCanvas;
    public ScrollRect scrollRect;
    public TreeClass[] skillImage; //skillN, treeN
    public TextMeshProUGUI[] lvText;
    public GameObject hoverBox;
    private int lastTree = -1;
    private int[] currentButtonSelected = { -1, -1 };

    public Image[] skillSlotImage;
    [SerializeField] private Image currentBtnImg;

    private void Start() {
        imgChild = new List<Image>(GetComponentsInChildren<Image>());
        foreach (Image img in skillSlotImage) {
            imgChild.Remove(img);
            imgChild.Remove(img.transform.parent.GetComponent<Image>());
        }

        treeCanvas = GetComponent<CanvasGroup>();
        AlternateCanvas(treeCanvas, false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (treeCanvas.interactable) {
                AlternateCanvas(treeCanvas, false);
                AlternateCanvas(hudCanvas, true);

                Time.timeScale = 1;
                PlayerMovement.instance.moveLock = false;
                PlayerAttack.instance.canInput = true;
                HUDIconUpdate();
            }
            else if (PlayerData.instance.currentHealth > 0) {
                AlternateCanvas(treeCanvas, true);
                AlternateCanvas(pauseCanvas, false);
                AlternateCanvas(hudCanvas, false);

                Time.timeScale = 0;
                PlayerMovement.instance.moveLock = true;
                PlayerAttack.instance.canInput = false;

                for (int i = 1; i < 7; i++) lvText[i].text = PlayerData.instance.leveling[i].lv.ToString();
            }
        }
        if (treeCanvas.interactable) {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, hits);
            bool hitButton = false;
            foreach (RaycastResult hit in hits) if (hit.gameObject.GetComponent<SkillData>() != null) {
                    hoverBox.GetComponentsInChildren<TextMeshProUGUI>()[0].text = hit.gameObject.GetComponent<SkillData>().skillName;
                    hoverBox.GetComponentsInChildren<TextMeshProUGUI>()[1].text = hit.gameObject.GetComponent<SkillData>().description;
                    hitButton = true;
                }

            hoverBox.transform.position = hitButton ? (Input.mousePosition + new Vector3(Input.mousePosition.x > 960 ? -157.5f : 157.5f, 0, 0)) : new Vector3(-500, 500, 0);
        }
    }

    private void AlternateCanvas(CanvasGroup canvas, bool on) {
        canvas.alpha = on ? 1 : 0;
        canvas.interactable = on;
        canvas.blocksRaycasts = on;
    }

    public void HUDIconUpdate() {
        PlayerHUD.instance.atkIcon[0].sprite = skillSlotImage[0].sprite;
        PlayerHUD.instance.atkIcon[1].sprite = skillSlotImage[1].sprite;
        PlayerHUD.instance.atkIcon[2].sprite = skillSlotImage[2].sprite;

        if (currentButtonSelected[0] != -1) {
            FocusOnClick(false);
            currentButtonSelected = new int[] { -1, -1 };
        }
    }

    public void SkillButton(int treeN) {
        lastTree = currentButtonSelected[1];
        currentButtonSelected[1] = treeN;
    }

    public void SkillButton2(int skillN) {
        if (PlayerData.instance.leveling[currentButtonSelected[1]].unlockedSkill[skillN] == true) {
            if (currentButtonSelected[0] != skillN || currentButtonSelected[1] != lastTree) {
                currentBtnImg = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
                FocusOnClick(true);
                currentButtonSelected[0] = skillN;
            }
            else {
                FocusOnClick(false);
                currentButtonSelected[0] = -1;
            }
        }
        else {
            int spentPoints = 0;
            foreach (bool bol in PlayerData.instance.leveling[currentButtonSelected[1]].unlockedSkill) spentPoints += bol ? 1 : 0;

            if (spentPoints < PlayerData.instance.leveling[currentButtonSelected[1]].lv && CanUnlock(skillN)) {
                PlayerData.instance.leveling[currentButtonSelected[1]].unlockedSkill[skillN] = true;
                AudioManager.instance.Play("UnlockSkill");
                FocusOnClick(false);
            }
            else {
                AudioManager.instance.Play("DenyUnlock");
                currentButtonSelected[1] = lastTree;
            }
        }
    }

    private bool CanUnlock(int i) {
        switch (i) {
            case 0: return true;
            case 1:
                if (PlayerData.instance.leveling[currentButtonSelected[1]].unlockedSkill[0]) return true;
                else return false;
            case 2:
                if (PlayerData.instance.leveling[currentButtonSelected[1]].unlockedSkill[0]) return true;
                else return false;
            case 3:
                if (PlayerData.instance.leveling[currentButtonSelected[1]].unlockedSkill[1]) return true;
                else return false;
            case 4:
                if (PlayerData.instance.leveling[currentButtonSelected[1]].unlockedSkill[1]) return true;
                else return false;
            case 5:
                if (PlayerData.instance.leveling[currentButtonSelected[1]].unlockedSkill[2]) return true;
                else return false;
            case 6:
                if (PlayerData.instance.leveling[currentButtonSelected[1]].unlockedSkill[4]) return true;
                else return false;
            case 7:
                if (PlayerData.instance.leveling[currentButtonSelected[1]].unlockedSkill[5]) return true;
                else return false;
            case 8:
                if (PlayerData.instance.leveling[currentButtonSelected[1]].unlockedSkill[7]) return true;
                else return false;
            default:
                Debug.LogWarning("ErrorChecking for unlock skill");
                return false;
        }
    }

    public void SKillSlotButton(int slotN) {
        if (currentButtonSelected[0] != -1) {
            if (PlayerAtkList.instance.skill[slotN] != currentButtonSelected[0] || PlayerAtkList.instance.tree[slotN] != currentButtonSelected[1]) {
                int i = CheckOtherSlots(slotN);
                if (i != -1) {
                    PlayerAtkList.instance.skill[i] = PlayerAtkList.instance.skill[slotN];
                    PlayerAtkList.instance.tree[i] = PlayerAtkList.instance.tree[slotN];
                    skillSlotImage[i].sprite = skillSlotImage[slotN].sprite;
                    PlayerAttack.instance.atkTotalCDown[i] = PlayerAttack.instance.atkTotalCDown[slotN];
                }
                PlayerAtkList.instance.skill[slotN] = currentButtonSelected[0];
                PlayerAtkList.instance.tree[slotN] = currentButtonSelected[1];
                skillSlotImage[slotN].sprite = skillImage[currentButtonSelected[1]].skillImage[currentButtonSelected[0]];
                PlayerAttack.instance.atkTotalCDown[slotN] = PlayerAtkList.instance.treeCoolDowns[currentButtonSelected[1]].coolDown[currentButtonSelected[0]];
                currentButtonSelected[0] = -1;
                FocusOnClick(false);

            }
            else if (PlayerAtkList.instance.skill[slotN] == currentButtonSelected[0] && PlayerAtkList.instance.tree[slotN] == currentButtonSelected[1]) {
                currentButtonSelected[0] = -1;
                FocusOnClick(false);
            }
        }
    }

    private void FocusOnClick(bool bol) {
        scrollRect.enabled = !bol;
        if (bol) {
            foreach (Image img in imgChild) ChangeColor(img, 0.5f);
            ChangeColor(currentBtnImg, 1);
            ChangeColor(currentBtnImg.transform.GetChild(0).GetComponent<Image>(), 1);

        }
        else foreach (Image img in imgChild) ChangeColor(img, 1);
    }

    private void ChangeColor(Image img, float mtply) {
        img.color = Color.white * mtply;
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
    }

    private int CheckOtherSlots(int currentSlot) {
        if (currentSlot != 0 && PlayerAtkList.instance.skill[0] == currentButtonSelected[0] && PlayerAtkList.instance.tree[0] == currentButtonSelected[1]) return 0;
        else if (currentSlot != 1 && PlayerAtkList.instance.skill[1] == currentButtonSelected[0] && PlayerAtkList.instance.tree[1] == currentButtonSelected[1]) return 1;
        else if (currentSlot != 2 && PlayerAtkList.instance.skill[2] == currentButtonSelected[0] && PlayerAtkList.instance.tree[2] == currentButtonSelected[1]) return 2;
        else return -1;
    }

}