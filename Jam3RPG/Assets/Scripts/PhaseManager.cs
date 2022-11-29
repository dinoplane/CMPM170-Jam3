using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Written by Santiago
- Can later automate the lists by looking for all unit objects in the scene with tags 'playerUnit'
  or 'aiUnit', instead of having to add them all to the lists manually.
- Once we actually have a unit class, replace the UnitBaseClass class used here with that new class
 */

public class PhaseManager : MonoBehaviour
{
    public List<UnitBaseClass> playerUnits;
    [HideInInspector] public int playerUnitsThatCanAct = 0;
    public List<UnitBaseClass> aiUnits;
    [HideInInspector] public int aiUnitsThatCanAct = 0;
    bool playerPhase;
    int currentCycle = 0; /*A cycle conists of both a player phase and an enemy phase. 
                           * New cycle begins at start of each player phase. Will become important later*/

    public CanvasUI canvasUI;

    [SerializeField]
    private GameObject gridManager;

    public BaselineAI enemyAI;

    // Start is called before the first frame update
    void Start()
    {

        UnitBaseClass[] units = FindObjectsOfType<UnitBaseClass>();
        foreach(UnitBaseClass unit in units)
        {
            if (unit.isEnemy)
            {
                aiUnits.Add(unit);
            }
            else
            {
                playerUnits.Add(unit);
            }
        }

        Debug.Log("Player units " + playerUnits.Count);
        Debug.Log("Enemy units " + aiUnits.Count);

        enemyAI = gridManager.GetComponent<BaselineAI>();

        MakeUnitsInactive(aiUnits);
        StartPlayerPhase();
        // MakeUnitsInactive(playerUnits);
        // StartAiPhase();
    }

    
    //Makes list of units unable to be given commands
    void MakeUnitsInactive(List<UnitBaseClass> unitList)
    {
        foreach( UnitBaseClass unit in unitList)
        {
            unit.MakeInactive();
        }
    }

    void StartPlayerPhase()
    {
        //Make all player uints able to be given commands
        foreach (UnitBaseClass unit in playerUnits)
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
        foreach ( UnitBaseClass unit in aiUnits)
        {
            unit.MakeActive();
        }
        aiUnitsThatCanAct = aiUnits.Count;
        playerPhase = false;
        Debug.Log("Started AI Phase");

        if (canvasUI != null)
            canvasUI.StartAiPhase();
        enemyAI.EnemyTurnStart();
    }

    //Called whenever a unit finishes a turn
    public void UnitFinishedTurn()
    {
        if (playerPhase)
        {
            playerUnitsThatCanAct -= 1;
            if(playerUnitsThatCanAct <= 0) //All player units have finished turn
            {
                foreach(UnitBaseClass unit in playerUnits)
                {
                    unit.sprite.color = Color.white; //Un-greys-out the units
                }
                StartAiPhase();
            }
        }
        else
        {
            aiUnitsThatCanAct -= 1;
            if(aiUnitsThatCanAct <= 0)
            {
                foreach (UnitBaseClass unit in aiUnits) //All AI units have finished turn
                {
                    unit.sprite.color = Color.white; //Un-greys-out the units
                }
                StartPlayerPhase();
            }
        }
    }

    public void UnitDied(UnitBaseClass unit)
    {
        if (unit.isEnemy)
        {
            aiUnits.Remove(unit);
            if(aiUnits.Count <= 0)
            {
                Debug.Log("All enemies defeated. Victory!");
            }
        }
        else
        {
            playerUnits.Remove(unit);
            //Checks if Unit has the Cult Leader script as a component
            if (unit.GetComponent<CultLeaderClass>()) 
            {
                Debug.Log("Cult leader defeated. Game Over");
            }
        }
    }
}
