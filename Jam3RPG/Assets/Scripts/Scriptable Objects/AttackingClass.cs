using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// UnitBaseClass Variables:
//     public bool isEnemy;
//     public int healthMax;
//     private int healthCurrent; 
//     public int armorMax;
//     private int armorCurrent;
//     public int moveRange;
//     public ??? position;
//
// UnitBaseClass Functions:
//     ChangeHealth();
//     ChangeArmor();
//     MoveToSpace();
//     ActionSacrifice();
//     Death();



[CreateAssetMenu(fileName = "Attacking", menuName = "Jam3RPG/New Attacking Class")]
public class AttackingClass : UnitBaseClass
{


    [Header("Attack Stats")]
    public int attackDamage;
    public int attackRange;


    // Attack
    //
    // Called to deal damage to enemy unit. 
    // Calls the enemy’s ChangeHealth() function which reduces the damage by their armor before actually changing the enemy’s health.
    //
    // Possible Parameters: 
    //    Type? Enemy: Passes target enemy to damage. Checks their armor and calls their ChangeHealth() function.
    //
    // Possible Returns:
    //    Int Total: Positive integer that represents new health total of enemy
    //
    private void Attack(){  

    }


    // CounterAttack
    //
    // Called to deal damage to opposing attacking unit if unit in range. 
    // Checks to see if unit is within range of attacker. Fail if not, otherwise attack back.
    // Calls the attacking unit's ChangeHealth() function which reduces the damage by their armor before actually changing their health.
    //
    // Possible Parameters: 
    //    Type? Enemy: Passes target enemy to damage. Checks if in range, then calls Attack().
    //
    // Possible Returns:
    //    Int Total: Positive integer that represents new health total of enemy
    //
    private void CounterAttack(){  

    }
}
