using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    List<UnitPlaceholder> playerUnits;
    int playerUnitsThatCanAct = 0;
    List<UnitPlaceholder> aiUnits;
    int aiUnitsThatCanACt = 0;
    bool playerPhase;

    // Start is called before the first frame update
    void Start()
    {
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        playerUnitsThatCanAct = playerUnits.Count;
        playerPhase = true;
    }

    void StartAiTurn()
    {
        aiUnitsThatCanACt = aiUnits.Count;
    }

    /*
     When a unit finishes its turn, call this function
        - decrease unitsThatCanAct (for whoever's current phase it is) by 1
        - if this number is now 0, start the opposing player's phase

     */
}
