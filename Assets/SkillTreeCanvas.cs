using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillTreeCanvas : MonoBehaviour {

    private PlayerAttack atkScript;
    private PlayerAtkList atkListScript;

    public CanvasGroup hudCanvas;
    private CanvasGroup treeCanvas;
    public ScrollRect scrollRect;
    public Image[] skillImage;
    private int currentButtonSelected = 0;

    public Image[] skillSlotImage; //multiply for each skilltree
    private int[] currentSkill = { 0, 0, 0 };

    private void Start() {
        treeCanvas = GetComponent<CanvasGroup>();
        AlternateCanvas(treeCanvas, false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (treeCanvas.interactable) {
                AlternateCanvas(treeCanvas, false);
                AlternateCanvas(hudCanvas, true);
            }
            else {
                AlternateCanvas(treeCanvas, true);
                AlternateCanvas(hudCanvas, false);
            }
        }
    }

    private void AlternateCanvas(CanvasGroup canvas, bool on) {
        canvas.alpha = on? 1 : 0;
        canvas.interactable = on;
        canvas.blocksRaycasts = on;

    }

    public void SkillButton(int skillN) {

        currentButtonSelected = currentButtonSelected != skillN ? skillN : 0;
        FocusOnClick();


        if (currentButtonSelected != 0) Debug.Log("Botao pressioando " + currentButtonSelected);
        else Debug.Log("Botao desselecionado");
    }

    public void SKillSlotButton(int slotN) {
        if (currentButtonSelected == 0) Debug.Log("Sem botão pressionado, nada acontece");
        else if (currentButtonSelected == currentSkill[slotN]) Debug.Log("Esta ja era a habilidade no slot");
        else Debug.Log("Nova Skill = " + currentButtonSelected + ", substituindo " + currentSkill[slotN]);

        if (currentButtonSelected != 0 && currentSkill[slotN] != currentButtonSelected) {
            currentSkill[slotN] = currentButtonSelected - 1;
            skillSlotImage[slotN].sprite = skillImage[currentButtonSelected - 1].sprite;
            currentButtonSelected = 0;
        }
        FocusOnClick();
    }

    private void FocusOnClick() {
        //Darkens everything but the selected skill
        if (currentButtonSelected == 0) {
            scrollRect.enabled = true;
        }
        else {
            scrollRect.enabled = false;
        }
    }
}
