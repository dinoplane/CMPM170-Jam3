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


//[CreateAssetMenu(fileName = "Archer", menuName = "Jam3RPG/New Archer")]
public class ArcherClass : UnitBaseClass
{

    // Volley
    //
    // Called to deal damage to either several enemies or several times on a single enemy. 
    // Calls the enemy’s ChangeHealth() function which reduces the damage by their armor before actually changing the enemy’s health.
    // Only available after being sacrified.
    //
    // Possible Parameters: 
    //    Type? Enemy1: Passes target enemy to damage. Checks their armor and calls their ChangeHealth() function.
    //    Type? Enemy2: Passes target enemy to damage. Checks their armor and calls their ChangeHealth() function.
    //    Type? Enemy3: Passes target enemy to damage. Checks their armor and calls their ChangeHealth() function.
    //
    // Possible Returns:
    //    String Totals: A string with the new health totals of the damaged enemies.
    //    Type? Container: Some sort of container that contains positive integers representing the new health of the enemies.
    //
    private void Volley(){  

    }

}
