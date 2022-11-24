using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "UnitBase", menuName = "Jam3RPG/New Unit Base Class")]
public class UnitBaseClass : MonoBehaviour {
    
    [Header("Faction")]
    public bool isEnemy = false;

    [Header("Basic Stats")]
    public int healthMax;
    [HideInInspector] public int healthCurrent;
    public int armorMax;
    [HideInInspector] public int armorCurrent;
    public int moveRange;
    // public ??? position; Unit should know where it is on the grid
    // Could probably add sprites and sfx here too

    [Header("Attack Stats")]
    public int attackDamage;
    public int attackRange;

    [Header("Other")]
    public bool turnOver = false;
    [HideInInspector] public bool wasSacrificed = false;
    
    public SpriteRenderer sprite;

    private PhaseManager phaseManager;

    // Set health and armor to their max value
    private void Awake() {
        healthCurrent = healthMax;
        armorCurrent = armorMax;
    }

    public void Start()
    {
        phaseManager = FindObjectOfType<PhaseManager>();
    }

    public void MakeActive()
    {
        turnOver = false;
    }

    public void MakeInactive()
    {
        turnOver = true;
    }

    public void FinishTurn()
    {
        if (phaseManager != null)
        {
            MakeInactive();
            sprite.color = Color.grey;
            phaseManager.UnitFinishedTurn();
        }
    }

    // ChangeHealth
    //
    // Called to change the health value of a unit
    //
    // Possible Parameters: 
    //    Int Amount: Positive or Negative integer that represents amount of healing or damage done to unit
    //
    // Possible Returns:
    //    Int Total: Positive integer that represents new health total
    //
    public void ChangeHealth(int amount){  
        int change;
        // Check if incoming amount is damage or healing. 
        if(amount < 0){ // -ints are damage, +ints are healing
            change = amount  + armorCurrent; // Reduce damage by current armor. Weird bacuse of negatives
        }
        else{
            change = amount; // Healing
        }

        healthCurrent += change; // Change health to show damage or healing
        if (healthCurrent > healthMax) {
            healthCurrent = healthMax;
        }
        else if(healthCurrent <= 0){
            healthCurrent = 0;
            Death(wasSacrificed);
        }

        Debug.Log(string.Format("Damage: {0} Armor: {1} Change: {2} Health: {3}", amount, armorCurrent, change, healthCurrent));
    }


    // ChangeArmor
    //
    // Called to change the armor value of a unit
    //
    // Possible Parameters: 
    //    Int Amount: Positive integer that represents amount of damage done to unit's armor
    //
    // Possible Returns:
    //    Int Total: Positive integer that represents new armor total
    //
    public void ChangeArmor(int amount){
        armorCurrent -= amount;
        if(armorCurrent < 0){
            armorCurrent = 0;
        }
    }


    // MoveToSpace
    //
    // Called to change the unit's position on the grid
    //
    // Possible Parameters: 
    //    Int/Float/Vector2? newPosition: Where to move the unit
    //
    // Possible Returns:
    //    None? 
    //
    public void MoveToSpace(){

    }


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
    public void Attack(UnitBaseClass enemy){  
        enemy.ChangeHealth(-attackDamage);
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
    public void CounterAttack(UnitBaseClass enemy){  
        // if(In range of attacking enemy){
        //     Attack(enemy);
        // }
    }


    // ActionSacrifice
    //
    // Called to sacrifice a unit's soul. Powered up by some factor. Ends on either unit's turn end or start of player turn. Then dies.
    //
    // Possible Parameters: 
    //    None?
    //
    // Possible Returns:
    //    None?
    //
    public void ActionSacrifice() { // Only usable if team == playerTeam and class != Cult Leader
        
    }


    // Death
    //
    // Called to kill a unit and remove them from the grid.
    //
    // Possible Parameters: 
    //    Bool wasSacrificed: If true, play more fun SFX and death animation where unit is disintegrating. 
    //                        If false, default death animation and SFX.
    //
    // Possible Returns:
    //    None?
    //
    public void Death(bool wasSacrificed) { 
        if(wasSacrificed){
            //Do cool sacrifice death
        }
        else{
            //Do regular death
        }
    }
    
}

