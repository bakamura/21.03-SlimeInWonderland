using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TreeClass {
    public Image[] skillImage;
}

public class SkillTreeCanvas : MonoBehaviour {

    private PlayerMovement movementScipt;
    private PlayerAttack atkScript;
    private PlayerAtkList atkListScript;
    private PlayerHUD hudScript;

    public bool canOpenTab;
    public CanvasGroup hudCanvas;
    private CanvasGroup treeCanvas;
    public ScrollRect scrollRect;
    public TreeClass[] skillImage; //skillN, treeN
    private int[] currentButtonSelected = { 0, 0 };

    public Image[] skillSlotImage; 
    private int[] currentSkill = { 0, 0, 0 };

    private void Start() {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        movementScipt = playerGO.GetComponent<PlayerMovement>();
        atkScript = playerGO.GetComponent<PlayerAttack>();
        hudScript = playerGO.GetComponent<PlayerHUD>();

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
                //Change player Skills
                Time.timeScale = 1;
                movementScipt.moveLock = false;
                atkScript.canInput = true;
                AlternateCanvas(hudCanvas, true);
            }
            else {
                AlternateCanvas(treeCanvas, true);
                Time.timeScale = 0;
                movementScipt.moveLock = true;
                atkScript.canInput = false;
                AlternateCanvas(hudCanvas, false);
            }
        }
    }

    private void AlternateCanvas(CanvasGroup canvas, bool on) {
        canvas.alpha = on? 1 : 0;
        canvas.interactable = on;
        canvas.blocksRaycasts = on;
    }

    public void SkillButton(int skillN  ) {
        currentButtonSelected[0] = (currentButtonSelected[0] != skillN) ? skillN : 0;
        FocusOnClick();


        if (currentButtonSelected[0] != 0) Debug.Log("Botao pressioando " + currentButtonSelected);
        else Debug.Log("Botao desselecionado");
    }

    public void SkillButton2(int treeN) {
        currentButtonSelected[1] = treeN;
    }

    public void SKillSlotButton(int slotN) {
        if (currentButtonSelected[0] == 0) Debug.Log("Sem botão pressionado, nada acontece");
        else if (currentButtonSelected[0] == currentSkill[slotN]) Debug.Log("Esta ja era a habilidade no slot");
        else Debug.Log("Nova Skill = " + currentButtonSelected + ", substituindo " + currentSkill[slotN]);

        if (currentButtonSelected[0] != 0 && currentSkill[slotN] != currentButtonSelected[0]) {
            currentSkill[slotN] = currentButtonSelected[0] - 1;
            skillSlotImage[slotN].sprite = skillImage[currentButtonSelected[1]].skillImage[currentButtonSelected[0] - 1].sprite;
            currentButtonSelected[0] = 0;
        }
        FocusOnClick();
    }

    private void FocusOnClick() {
        //Darkens everything but the selected skill
        if (currentButtonSelected[0] == 0) {
            scrollRect.enabled = true;
            //Use color scheme, not RBG one
        }
        else {
            scrollRect.enabled = false;

        }
    }
}
