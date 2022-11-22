using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Written by Santiago
- Can later automate the lists by looking for all unit objects in the scene with tags 'playerUnit'
  or 'aiUnit', instead of having to add them all to the lists manually.
 
 */

public class PhaseManager : MonoBehaviour
{
    public List<UnitPlaceholder> playerUnits;
    int playerUnitsThatCanAct = 0;
    public List<UnitPlaceholder> aiUnits;
    int aiUnitsThatCanAct = 0;
    bool playerPhase;

    // Start is called before the first frame update
    void Start()
    {
        MakeUnitsInactive(aiUnits);
        StartPlayerPhase();
    }

    void MakeUnitsInactive(List<UnitPlaceholder> unitList)
    {
        foreach( UnitPlaceholder unit in unitList)
        {
            unit.MakeInactive();
        }
    }

    void StartPlayerPhase()
    {
        foreach (UnitPlaceholder unit in playerUnits)
        {
            unit.MakeActive();
        }
        playerUnitsThatCanAct = playerUnits.Count;
        playerPhase = true;
        Debug.Log("Started Player Phase");
    }

    void StartAiPhase()
    {
        foreach( UnitPlaceholder unit in aiUnits)
        {
            unit.MakeActive();
        }
        aiUnitsThatCanAct = aiUnits.Count;
        playerPhase = false;
        Debug.Log("Started AI Phase");
    }

    //Function called whenever a unit finishes a turn
    public void UnitFinishedTurn()
    {
        if (playerPhase)
        {
            playerUnitsThatCanAct -= 1;
            if(playerUnitsThatCanAct <= 0)
            {
                StartAiPhase();
            }
        }
        else
        {
            aiUnitsThatCanAct -= 1;
            if(aiUnitsThatCanAct <= 0)
            {
                StartPlayerPhase();
            }
        }
    }
}
