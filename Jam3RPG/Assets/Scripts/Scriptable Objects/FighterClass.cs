using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KeyActionPair = System.Collections.Generic.KeyValuePair<string, (UnitBaseClass.UnitAction action, bool needsTarget)>;



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
    public int chipDmg;

    override public void ExtraAwake()
    {
        base.ExtraAwake();
        actions.Add(new KeyActionPair("ChipArmor", (ChipArmor, true)));
        actions.Add(new KeyActionPair("DestroyArmor", (DestroyArmor, true)));
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
    public void ChipArmor(UnitBaseClass enemy){  
        enemy.ChangeArmor(chipDmg);
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
    public void DestroyArmor(UnitBaseClass enemy){  
        ActionSacrifice();
        StartCoroutine(DestroyArmorAction(enemy));
    }

    IEnumerator DestroyArmorAction(UnitBaseClass enemy)
    {
        yield return new WaitForSeconds(0.5f);
        enemy.ChangeArmor(-enemy.armorCurrent);
        enemy.ChangeHealth(-1000);
        yield return new WaitForSeconds(0.5f);
        Death(wasSacrificed);
    }
}
