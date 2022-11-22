using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPlaceholder : MonoBehaviour
{
    bool turnOver = false;
    Button buttonComponent;
    public Image imageComponent;
    PhaseManager phaseManager;

    // Start is called before the first frame update
    void Start()
    {
        buttonComponent = this.GetComponent<Button>();
        imageComponent = this.GetComponent<Image>();
        phaseManager = FindObjectOfType<PhaseManager>();
    }

    public void TakeTurn()
    {
        if(phaseManager != null)
        {
            MakeInactive();
            imageComponent.color = Color.blue;
            phaseManager.UnitFinishedTurn();
        }
    }

    public void MakeActive()
    {
        turnOver = false;
        buttonComponent.interactable = true;
    }

    public void MakeInactive()
    {
        turnOver = true;
        buttonComponent.interactable = false;
    }
}
