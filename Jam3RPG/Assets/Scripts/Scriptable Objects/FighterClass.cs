using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// UnitBaseClass Variables:
//     bool isEnemy;
//     int healthMax;
//     int healthCurrent; 
//     int armorMax;
//     int armorCurrent;
//     int moveRange;
//     ??? position;
//
// AttackingClass Variables:
//     int attackDamage;
//     int attackRange;
//
// UnitBaseClass Functions:
//     ChangeHealth();
//     ChangeArmor();
//     MoveToSpace();
//     ActionSacrifice();
//     Death();
//
// AttackingClass Functions:
//     Attack();
//     CounterAttack();


//[CreateAssetMenu(fileName = "Fighter", menuName = "Jam3RPG/New Fighter")]
public class FighterClass : AttackingClass
{
    public void ActionSacrifice() { // Only usable if team == playerTeam and class != Cult Leader
        if(!isEnemy && unitClass != "Cult Leader"){
            wasSacrificed = true;
        }
    }

    // ChipArmor
    //
    // Called to deal damage to enemy armor. 
    // Calls the enemy’s ChangeArmor() function.
    //
    // Possible Parameters: 
    //    Type Enemy1: Passes target enemy to damage. Reduces their armor.
    //    Int armorDam: damage applied to armor
    //
    // Possible Returns:
    //    Int Total: A string with the new armor total of the damaged enemy.
    //
    private void ChipArmor(UnitBaseClass enemy, int armorDam){  

    }


    // DestroyArmor
    //
    // Called to completly shred the armor of an enemy. 
    // Calls the enemy’s ChangeArmor() function.
    // Only available after being sacrified.
    //
    // Possible Parameters: 
    //    Type? Enemy: Passes target enemy to damage. Sets their armor to 0.
    //
    // Possible Returns:
    //    None?
    //
    private void DestroyArmor(UnitBaseClass enemy){  
        ActionSacrifice();
        if(wasSacrificed){
            ChipArmor(enemy, -1*enemy.armorCurrent);
        }
        Death(wasSacrificed);
    }

}
