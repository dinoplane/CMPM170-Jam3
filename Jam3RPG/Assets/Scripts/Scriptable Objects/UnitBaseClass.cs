using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitBase", menuName = "Jam3RPG/New Unit Base Class")]
public class UnitBaseClass : ScriptableObject {
    
    [Header("Faction")]
    public bool isEnemy = false;

    [Header("Basic Stats")]
    public int healthMax;
    private int healthCurrent; 
    public int armorMax;
    private int armorCurrent;
    public int moveRange;
    public VectorInt2 tileCoords; // Tile coordinates
    // public ??? position; Unit should know where it is on the grid
    // Could probably add sprites and sfx here too
    public bool isSacrificed;

    // ChangeHealth
    //
    // Called to change the health value of a unit
    //
    // Possible Parameters: 
    //    Int Amount: Positive or Negative integer that represents amount of healing or damage done to unit
    //
    // Possible Returns:
    //    Int Total: Positive integer that represents new health total
    //
    private void ChangeHealth(int healthDamage){  
        healthCurrent += healthDamage;
    }


    // ChangeArmor
    //
    // Called to change the armor value of a unit
    //
    // Possible Parameters: 
    //    Int Amount: Positive integer that represents amount of damage done to unit's armor
    //
    // Possible Returns:
    //    Int Total: Positive integer that represents new armor total
    //
    private void ChangeArmor(int chipDamage){
        armorCurrent -= chipDamage;
    }


    // MoveToSpace
    //
    // Called to change the unit's position on the grid
    //
    // Possible Parameters: 
    //    Int/Float/Vector2? newPosition: Where to move the unit
    //
    // Possible Returns:
    //    None? 
    //
    private void MoveToSpace(Vector2Int coords){
        // we may need tile coords...
        tileCoords = coords;
    }


    // ActionSacrifice
    //
    // Called to sacrifice a unit's soul. Powered up by some factor. Ends on either unit's turn end or start of player turn. Then dies.
    //
    // Possible Parameters: 
    //    None?
    //
    // Possible Returns:
    //    None?
    //
    private void ActionSacrifice() { // Only usable if team == playerTeam and class != Cult Leader
        Debug.Log("Define sacrifice in calsses!");
        isSacrificed = true; // the phase switcher could check if any units are sacrificed and kill them. 
    }


    // Death
    //
    // Called to kill a unit and remove them from the grid.
    //
    // Possible Parameters: 
    //    Bool wasSacrificed: If true, play more fun SFX and death animation where unit is disintegrating. 
    //                        If false, default death animation and SFX.
    //
    // Possible Returns:
    //    None?
    //
    private void Death() { 
        // Not sure how this will work... but ok...
        if (isSacrificed){
            // Do special Animation
        }

        else if (healthCurrent <= 0){ // Got killed...
            // Do something 
            // play animation
        }
        // delete self...
        Object.Destroy(this.gameObject);
    }
    
}

