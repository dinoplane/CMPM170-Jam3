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
    public int chipDmg;

    override public void ExtraAwake()
    {
        base.ExtraAwake();
        actions.Add(new KeyValuePair<string, bool>("ChipArmor", true));
    }

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
    private void ChipArmor(UnitBaseClass enemy){  
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
    private void DestroyArmor(UnitBaseClass enemy){  
        enemy.ChangeArmor(enemy.armorCurrent*(-1));
    }

    public override void ActionSacrifice(UnitBaseClass enemy){
        Debug.Log("destroy");
        DestroyArmor(enemy);
        ChangeHealth((-1)*(healthCurrent+armorCurrent));
        Debug.Log(string.Format("This unit:   Armor: {0} Health: {1}", armorCurrent, healthCurrent));
        //Debug.Log(string.Format("This unit:   Armor: {0} Health: {1}", armorCurrent, healthCurrent));
    }
}
