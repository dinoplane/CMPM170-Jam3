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



//[CreateAssetMenu(fileName = "Cleric", menuName = "Jam3RPG/New Cleric")]
public class ClericClass : UnitBaseClass
{


    [Header("Support Stats")]
    public int healAmount;
    public int shieldAmount; 
    public int healRange;


    // HealAlly
    //
    // Called to heal friendly unit. Perhaps either by a flat number or percentage of max HP.
    // Calls the unit's ChangeHealth() function.
    //
    // Possible Parameters: 
    //    Type? Unit: Passes in unit to heal. Calls their ChangeHealth() function.
    //
    // Possible Returns:
    //    Int Total: Positive integer that represents new health total of ally.
    //
    private void HealAlly(){  

    }


    // ShieldAlly
    //
    // Called to shield an ally. 
    // Gives armor? Gives temporary armor? 
    //
    // Possible Parameters: 
    //    Type? Unit: Passes in unit to shield.
    //
    // Possible Returns:
    //    None?
    //
    private void ShieldAlly(){  

    }
}
