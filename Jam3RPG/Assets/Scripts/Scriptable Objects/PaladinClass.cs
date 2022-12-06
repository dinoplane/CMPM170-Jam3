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


//[CreateAssetMenu(fileName = "Paladin", menuName = "Jam3RPG/New Paladin")]
public class PaladinClass : AttackingClass
{
    override public void ExtraAwake()
    {
        base.ExtraAwake();
        unitClass = "Paladin";
        //actions.Add(new KeyValuePair<string, bool>("", true));
    }

}
