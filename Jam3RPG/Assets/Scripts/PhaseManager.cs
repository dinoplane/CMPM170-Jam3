using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Written by Santiago
- Can later automate the lists by looking for all unit objects in the scene with tags 'playerUnit'
  or 'aiUnit', instead of having to add them all to the lists manually.
- Once we actually have a unit class, replace the UnitPlaceholder class used here with that new class
 */

public class PhaseManager : MonoBehaviour
{
    public List<UnitPlaceholder> playerUnits;
    int playerUnitsThatCanAct = 0;
    public List<UnitPlaceholder> aiUnits;
    int aiUnitsThatCanAct = 0;
    bool playerPhase;
    int currentCycle = 0; /*A cycle conists of both a player phase and an enemy phase. 
                           * New cycle begins at start of each player phase. Will become important later*/

    public CanvasUI canvasUI;

    // Start is called before the first frame update
    void Start()
    {
        MakeUnitsInactive(aiUnits);
        StartPlayerPhase();
    }

    //Makes list of units unable to be given commands
    void MakeUnitsInactive(List<UnitPlaceholder> unitList)
    {
        foreach( UnitPlaceholder unit in unitList)
        {
            unit.MakeInactive();
            Debug.Log("Made unit inactive");
        }
    }

    void StartPlayerPhase()
    {
        //Make all player uints able to be given commands
        foreach (UnitPlaceholder unit in playerUnits)
        {
            unit.MakeActive();
        }
        playerUnitsThatCanAct = playerUnits.Count;
        playerPhase = true;
        Debug.Log("Started Player Phase");
        currentCycle++;

        if(canvasUI != null)
            canvasUI.StartPlayerPhase();
    }

    void StartAiPhase()
    {
        //Make all AI uints able to be given commands
        foreach ( UnitPlaceholder unit in aiUnits)
        {
            unit.MakeActive();
        }
        aiUnitsThatCanAct = aiUnits.Count;
        playerPhase = false;
        Debug.Log("Started AI Phase");

        if (canvasUI != null)
            canvasUI.StartAiPhase();
    }

    //Called whenever a unit finishes a turn
    public void UnitFinishedTurn()
    {
        if (playerPhase)
        {
            playerUnitsThatCanAct -= 1;
            if(playerUnitsThatCanAct <= 0) //All player units have finished turn
            {
                foreach(UnitPlaceholder unit in playerUnits)
                {
                    unit.imageComponent.color = Color.white; //Un-greys-out the units
                }
                StartAiPhase();
            }
        }
        else
        {
            aiUnitsThatCanAct -= 1;
            if(aiUnitsThatCanAct <= 0)
            {
                foreach (UnitPlaceholder unit in aiUnits) //All AI units have finished turn
                {
                    unit.imageComponent.color = Color.white; //Un-greys-out the units
                }
                StartPlayerPhase();
            }
        }
    }
}
