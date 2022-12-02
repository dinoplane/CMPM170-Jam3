using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KeyActionPair = System.Collections.Generic.KeyValuePair<string, (UnitBaseClass.UnitAction action, bool needsTarget)>;

// UnitBaseClass Variables:
//     public bool isEnemy;
//     public int healthMax;
//     public int healthCurrent; 
//     public int armorMax;
//     public int armorCurrent;
//     public int moveRange;
//     public ??? position;
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


//[CreateAssetMenu(fileName = "Archer", menuName = "Jam3RPG/New Archer")]
public class ArcherClass : AttackingClass
{
    public int PowerShotDamage = 12;

    override public void ExtraAwake()
    {
        base.ExtraAwake();
        unitClass = "Archer";
        actions.Add(new KeyActionPair("PowerShot", (PowerShot, true)));
    }

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

    /* too out of scope for now
    private void Volley(){  

    }*/

    private void PowerShot(UnitBaseClass target){
        ActionSacrifice();
        StartCoroutine(PowerShotAction(target));
    }

    IEnumerator PowerShotAction(UnitBaseClass enemy)
    {
        yield return new WaitForSeconds(0.5f);
        enemy.ChangeHealth((-1) * PowerShotDamage);
        yield return new WaitForSeconds(0.5f);
        Death(wasSacrificed);
        StopAllCoroutines();
    }
}
