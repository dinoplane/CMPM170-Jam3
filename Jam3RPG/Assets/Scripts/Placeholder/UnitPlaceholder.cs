using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPlaceholder : MonoBehaviour
{
    public Button buttonComponent;
    public Image imageComponent;
    PhaseManager phaseManager;

    // Start is called before the first frame update
    void Start()
    {
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
        buttonComponent.interactable = true;
    }

    public void MakeInactive()
    {
        buttonComponent.interactable = false;
    }
}
