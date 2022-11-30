using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    public void ShowOtherUnit(UnitBaseClass unit = null)
    {
        if (unit == null)
        {
            otherUnitPanel.SetActive(false);
            return;
        }
        otherUnitPanel.SetActive(true);
        otherUnitName.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = unit.name;
        otherUnitHealth.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = unit.healthCurrent.ToString();
        otherUnitArmor.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = unit.armorCurrent.ToString();
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

        Debug.Log(actionString);
    }


    public void ShowTargetMessage(bool show = false){
        if(show == false){
            targetPanel.SetActive(false);
            return;
        }

        targetPanel.SetActive(true);
    }
}
