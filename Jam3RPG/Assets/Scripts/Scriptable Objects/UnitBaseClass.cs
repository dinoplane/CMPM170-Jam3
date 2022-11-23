using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitBase", menuName = "Jam3RPG/New Unit Base Class")]
public class UnitBaseClass : MonoBehaviour {
    
    [Header("Faction")]
    public bool isEnemy = false;

    [Header("Basic Stats")]
    public int healthMax;
    private int healthCurrent; 
    public int armorMax;
    private int armorCurrent;
    public int moveRange;
    // public ??? position; Unit should know where it is on the grid
    // Could probably add sprites and sfx here too


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
    private void ChangeHealth(){  

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
    private void ChangeArmor(){

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
    private void MoveToSpace(){

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
        
    }
    
}

