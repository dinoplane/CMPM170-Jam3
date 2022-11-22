using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPlaceholder : MonoBehaviour
{
    bool turnOver = false;
    Button buttonComponent;
    PhaseManager phaseManager;

    // Start is called before the first frame update
    void Start()
    {
        buttonComponent = this.GetComponent<Button>();
        phaseManager = FindObjectOfType<PhaseManager>();
    }

    public void TakeTurn()
    {
        if(phaseManager != null)
        {
            MakeInactive();
            phaseManager.UnitFinishedTurn();
        }
    }

    public void MakeActive()
    {
        turnOver = false;
        buttonComponent.interactable = true;
        this.GetComponent<Image>().color = Color.white;
    }

    public void MakeInactive()
    {
        turnOver = true;
        buttonComponent.interactable = false;
        this.GetComponent<Image>().color = Color.blue;
    }
}
