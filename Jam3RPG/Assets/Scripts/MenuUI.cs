using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KeyActionPair = System.Collections.Generic.KeyValuePair<string, (UnitBaseClass.UnitAction action, bool needsTarget)>;


public class MenuUI : MonoBehaviour {
    [Header("Player")]
    [SerializeField] private GameObject playerPanel;
    [SerializeField] private GameObject playerName;
    [SerializeField] private GameObject playerHealth;
    [SerializeField] private GameObject playerArmor;
    [SerializeField] private GameObject playerDamage;
    [SerializeField] private GameObject playerMoveRange;
    
    [Header("Player Actions")]
    [SerializeField] private GameObject actionPanel;
    [SerializeField] private GameObject actionButton1;
    [SerializeField] private GameObject actionButton2;
    [SerializeField] private GameObject actionButton3;
    [SerializeField] private GameObject actionButton4;

    [Header("Other Unit")]
    [SerializeField] private GameObject otherUnitPanel;
    [SerializeField] private GameObject otherUnitName;
    [SerializeField] private GameObject otherUnitHealth;
    [SerializeField] private GameObject otherUnitArmor;
    [SerializeField] private GameObject otherUnitDamage;
    [SerializeField] private GameObject otherUnitMoveRange;

    [Header("Combat Forecast")]
    [SerializeField] private GameObject combatPanel;
    [SerializeField] private GameObject combatUnitName;
    [SerializeField] private GameObject combatAction;

    [Header("Misc")]
    [SerializeField] private GameObject targetPanel;



    void Awake() {
        playerPanel.SetActive(false);
        actionPanel.SetActive(false);
        actionButton1.SetActive(false);
        actionButton2.SetActive(false);
        actionButton3.SetActive(false);
        actionButton4.SetActive(false);
        targetPanel.SetActive(false);
        otherUnitPanel.SetActive(false);
        combatPanel.SetActive(false);
    }


    public void ShowSelectedPlayer(UnitBaseClass unit = null){
        if(unit == null){
            playerPanel.SetActive(false);
            return;
        }
        playerPanel.SetActive(true);
        playerName.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = unit.name;
        playerHealth.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = unit.healthCurrent.ToString();
        playerArmor.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = unit.armorCurrent.ToString();
    }


    public void ShowOtherUnit(UnitBaseClass unit = null){
        if(unit == null){
            otherUnitPanel.SetActive(false);
            return;
        }
        otherUnitPanel.SetActive(true);
        otherUnitName.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = unit.name;
        otherUnitHealth.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = unit.healthCurrent.ToString();
        otherUnitArmor.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = unit.armorCurrent.ToString();
    }

    public void ShowCombatForecast(AttackingClass prime = null, UnitBaseClass unit = null, string action = null){
        if(unit == null){
            combatPanel.SetActive(false);
            return;
        } else if (prime.isEnemy != unit.isEnemy){

        
            combatPanel.SetActive(true);
            //show new stats - attack dmg, and specific
            combatUnitName.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Engaging Enemy " + unit.name;
            
            if(action == "Attack"){
                int atkDmg = prime.attackDamage - unit.armorCurrent;
                atkDmg = Mathf.Max(atkDmg, 0);
                int finalHP = Mathf.Max(unit.healthCurrent - atkDmg, 0);
                combatAction.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = (prime.attackDamage.ToString() + " DMG  - " + unit.armorCurrent.ToString() + " AMR = " + atkDmg.ToString() + " Total DMG\n Enemy HP Change: " + unit.healthCurrent.ToString() + " -> " + finalHP.ToString());
            }
            else if(action == "ChipArmor"){
                int chipStr = prime.GetComponent<FighterClass>().chipDmg;
                int finalArmor = Mathf.Max(unit.armorCurrent - chipStr, 0);
                combatAction.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ("Enemy Armor Change: " + unit.armorCurrent.ToString() + " -> " + finalArmor.ToString());
            }
            else if(action == "DestroyArmor"){
                combatAction.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Armor: 0 \n" + prime.name + " Health: 0";
            }
            else if(action == "Hypnotize"){
                combatAction.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Convert Rate: " + prime.GetComponent<CultLeaderClass>().hp + "%";
            }
            //combatAction.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Health after Attack: " + (prime.attackDamage - unit.healthCurrent).ToString();
           /* foreach (KeyActionPair actionPair in prime.actions)
            {
                if(actionPair.Key == ("ChipArmor")){
                    combatAction2.SetActive(true);
                    combatAction2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Armor after Chip: " + (prime.GetComponent<FighterClass>().chipDmg - unit.armorCurrent).ToString();
                }
                else if(actionPair.Key == ("DestroyArmor")){
                    combatAction3.SetActive(true);
                    combatAction3.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Armor: 0 | " + prime.name + " Health: 0";
    
                }
                else if(actionPair.Key == ("Hypnotize")){
                    combatAction2.SetActive(true);
                    combatAction2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Convert Rate: " + prime.GetComponent<CultLeaderClass>().hp + "%";
                }
            }
            */
        }
    }


    public void ShowActions(string actionString = null){
        if(actionString == null){
            actionPanel.SetActive(false);
            actionButton1.SetActive(false);
            actionButton2.SetActive(false);
            actionButton3.SetActive(false);
            actionButton4.SetActive(false);
            return;
        }

        actionPanel.SetActive(true);
        switch(true){
            case true when actionButton1.activeSelf == false:
                actionButton1.SetActive(true);
                actionButton1.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = actionString;
                break;
            case true when actionButton2.activeSelf == false:
                actionButton2.SetActive(true);
                actionButton2.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = actionString;
                break;
            case true when actionButton3.activeSelf == false:
                actionButton3.SetActive(true);
                actionButton3.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = actionString;
                break;
            case true when actionButton4.activeSelf == false:
                actionButton4.SetActive(true);
                actionButton4.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = actionString;
                break;
            default:
                break;            
        }
    }


    public void ShowTargetMessage(bool show = false){
        if(show == false){
            targetPanel.SetActive(false);
            return;
        }

        targetPanel.SetActive(true);
    }
}
