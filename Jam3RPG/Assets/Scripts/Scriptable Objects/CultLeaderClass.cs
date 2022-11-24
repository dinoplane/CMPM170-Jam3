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


//[CreateAssetMenu(fileName = "CultLeader", menuName = "Jam3RPG/New Cult Leader")]
public class CultLeaderClass : AttackingClass
{

    // Convert
    //
    // Called to a convert enemy unit to join the player's army.
    // Percentage based? Health based?
    //
    // Possible Parameters: 
    //    Type? Enemy: Passes target enemy to convert.
    //
    // Possible Returns:
    //    Bool Success: Returns a failure or a success 
    //    *Hillbilly Workaround* Type? Unit: Perhaps "kills" enemy unit and spawns a new identical unit in its place thats on the players control.
    //
    private void Convert(){  

    }

}
