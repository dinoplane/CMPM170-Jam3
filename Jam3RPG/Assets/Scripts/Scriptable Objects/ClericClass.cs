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
// UnitBaseClass Functions:
//     ChangeHealth();
//     ChangeArmor();
//     MoveToSpace();
//     ActionSacrifice();
//     Death();



//[CreateAssetMenu(fileName = "Cleric", menuName = "Jam3RPG/New Cleric")]
public class ClericClass : UnitBaseClass
{


    [Header("Support Stats")]
    public int healAmount;
    public int shieldAmount; 
    public int healRange;

    override public void ExtraAwake()
    {
        base.ExtraAwake();
        unitClass = "Cleric";
        actions.Add(new KeyActionPair("Heal", (HealAlly, true)));
        //actions.Add(new KeyActionPair("Shield", (ShieldAlly, true)));
    }


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
    private void HealAlly(UnitBaseClass target){  
        if(!target.isEnemy){
            target.ChangeHealth(healAmount);
        }else{
            Debug.Log("Heal failed");
        }
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
    private void ShieldAlly(UnitBaseClass target){  
        ActionSacrifice();
        if(wasSacrificed){
            target.healthCurrent = target.healthMax;
            target.armorCurrent = target.armorMax;
            target.invincible = true;
        }
        Death(wasSacrificed);
    }
}
