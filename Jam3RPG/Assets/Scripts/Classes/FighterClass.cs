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
//     int attackDamage;
//     int attackRange;
//     bool wasSacrificed;
//
// UnitBaseClass Functions:
//     ChangeHealth(int amount);
//     ChangeArmor(int amount);
//     MoveToSpace();
//     Attack(UnitBaseClass enemy);
//     CounterAttack(UnitBaseClass enemy);
//     ActionSacrifice();
//     Death();


//[CreateAssetMenu(fileName = "Fighter", menuName = "Jam3RPG/New Fighter")]
public class FighterClass : UnitBaseClass
{

    // ChipArmor
    //
    // Called to deal damage to enemy armor. 
    // Calls the enemy’s ChangeArmor() function.
    //
    // Possible Parameters: 
    //    Type Enemy1: Passes target enemy to damage. Reduces their armor.
    //
    // Possible Returns:
    //    Int Total: A string with the new armor total of the damaged enemy.
    //
    private void ChipArmor(){  

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
    private void DestoryArmor(){  

    }

}
