using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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


//[CreateAssetMenu(fileName = "CultLeader", menuName = "Jam3RPG/New Cult Leader")]
public class CultLeaderClass : AttackingClass
{
    //get component of phase manager as ref 
    //write function in phase manager
    public GameObject phaseManagerRef;
    public int garunteedConvertThresh = 2;
    bool isExist;
    private int x;
    [HideInInspector] public int hp;
    Random rand = new Random();

    override public void ExtraAwake()
    {
        base.ExtraAwake();
        unitClass = "Cult Leader";
        actions.Add(new KeyActionPair("Hypnotize", (Hypnotize, true)));
    }
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
    private void Hypnotize(UnitBaseClass enemy){
        //100% convert rate
        if(enemy.healthCurrent <= garunteedConvertThresh){
            ChangeEnemyToAlly(enemy);
            //send success msg
            Debug.Log("Convert Successful");
            Debug.Log(string.Format("Enemy Health: {0} Rolled #: {1} Success threshold: {2}", enemy.healthCurrent, x, hp));
        }
        //dependent on enemy hp
        else{
            //gets random percentage
            x = (int)(rand.NextDouble() * 100);
            //conversion chance based on health
            hp = (10 - (enemy.healthCurrent - garunteedConvertThresh)) * 10;
            if(hp < 10){
                hp = 10;
            }

            if(x <= hp){
                ChangeEnemyToAlly(enemy);
                //send success msg
                Debug.Log("Convert Successful");
                Debug.Log(string.Format("Enemy Health: {0} Rolled #: {1} Success threshold: {2}", enemy.healthCurrent, x, hp));
            }
            else{
                Debug.Log("Conversion Failed");
                Debug.Log(string.Format("Enemy Health: {0} Rolled #: {1} Success threshold: {2}", enemy.healthCurrent, x, hp));
            }
        }
    }

    /*
     * For hypnotize, can make the sprite play on the enemy, then do a coroutine wait for the the anim to finish before waiting for the anim to succeed
     * Alternatively...can make this anim be a sprite on another object, who's position is changed to be the position of the enemy. And the end of that animation
     * calls the real hypnotize function.
     * 
     * Cult leader cannot end turn before the hypnotize finishes because otherwise that would cause a 
    IEnumerator HypnotizeWaitForAnim()
    {

    }*/

    private void ChangeEnemyToAlly(UnitBaseClass enemy)
    {
        enemy.isEnemy = false;
        enemy.turnOver = false;
        phaseManagerRef.GetComponent<PhaseManager>().playerUnits.Add(enemy);
        phaseManagerRef.GetComponent<PhaseManager>().aiUnits.Remove(enemy);

        phaseManagerRef.GetComponent<PhaseManager>().playerUnitsThatCanAct++;
        phaseManagerRef.GetComponent<PhaseManager>().aiUnitsThatCanAct--;

        /*Changes enemy's animator controller to the cultist version*/
        enemy.ChangeAnimator();
    }

}
