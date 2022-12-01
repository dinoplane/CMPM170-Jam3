using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using KeyActionPair = System.Collections.Generic.KeyValuePair<string, (UnitBaseClass.UnitAction action, bool needsTarget)>;


//[CreateAssetMenu(fileName = "UnitBase", menuName = "Jam3RPG/New Unit Base Class")]
public class UnitBaseClass : MonoBehaviour {
    
    [Header("Faction")]
    public bool isEnemy = false;
    public bool isAggro = false;
    public int squad = -1;


    MenuUI menuUI;

    [Header("Basic Stats")]
    public int healthMax;
    public int healthCurrent;
    public int armorMax;
    [HideInInspector] public int armorCurrent;
    public int moveRange;
    public string unitClass = "Fighter";
    // public ??? position; Unit should know where it is on the grid
    public Vector2Int tilePosition;
    // Could probably add sprites and sfx here too

    [Header("Other")]
    [HideInInspector] public bool wasSacrificed = false;
    public bool turnOver = false;
    public SpriteRenderer sprite;
    Animator animator;
    Animator vfx;
    public RuntimeAnimatorController altAnimController;
    public bool invincible = false;

    private PhaseManager phaseManager;

    // Declare a delegate type for actions:
    public delegate void UnitAction(UnitBaseClass target);
    
    [HideInInspector] public List<KeyActionPair> actions = new List<KeyActionPair>(); //The bool is whether or not it requires a target.

    
    // Set health and armor to their max value
    private void Awake() {
        healthCurrent = healthMax;
        armorCurrent = armorMax;
        actions.Add(new KeyActionPair("Wait", (Wait, false)));
        ExtraAwake();
        menuUI = FindObjectOfType<MenuUI>();
        animator = GetComponent<Animator>();
        vfx = GetComponentInChildren<VFXonUnit>().vfxAnimator; //A rather roundabout way of getting this...

        if (animator == vfx)
            Debug.LogError("Bruh");

        if (!isEnemy)
        {
            ChangeAnimator();
        }
    }

    //Swaps to the stored Cultist animation controller
    public void ChangeAnimator()
    {
        animator.runtimeAnimatorController = altAnimController;
    }

    //Designed to be overwritten by subclasses, adding on more stuff.
    virtual public void ExtraAwake()
    {

    }

    public void Start()
    {
        phaseManager = FindObjectOfType<PhaseManager>();
    }

    public void MakeActive()
    {
        turnOver = false;
        if(invincible){
            invincible = false;
        }
    }

    public void MakeInactive()
    {
        turnOver = true;
    }

    public void SpriteSelect(){
        animator.SetBool("Selected", true);
    }

    public void SpriteUnselect(){
        animator.SetBool("Selected", false);
    }

    public void Wait(UnitBaseClass unit = null){
    }

    public void FinishTurn()
    {
        if (phaseManager != null)
        {
            MakeInactive();
            SpriteUnselect();
            if (!wasSacrificed)
            {
                sprite.color = Color.grey;
            }
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
        if(amount < 0 && !invincible){ // -ints are damage, +ints are healing
            change = Mathf.Min(amount  + armorCurrent, 0); // Reduce damage by current armor. Returns 0 if armor > damage
            vfx.SetTrigger("Hit");
        }else if(invincible){
            change = 0;
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
        //in the case of DestroyArmor()
        if(amount < 0){
            armorCurrent = 0;
        }
        //ChipArmor();
        else{
            armorCurrent -= amount;
            if(armorCurrent < 0){
                armorCurrent = 0;
            }
        }
        Debug.Log(string.Format("Chip Damage: {0} Armor: {1} Health: {2}", amount, armorCurrent, healthCurrent));
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
    public void MoveToSpace(Vector2Int tile){ // This function doesn't modify the transform
        tilePosition = tile;
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
        if(!isEnemy && unitClass != "Cult Leader"){
            wasSacrificed = true;
            animator.SetBool("Sacrificed", true);
        }
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
            Debug.Log(name.ToString() + " has sacrificed themselves to the Dark God");
        }
        else{
            //Do regular death
            Debug.Log(name.ToString() + " has died.");
        }
        phaseManager.UnitDied(this);
        animator.SetBool("Dead", true);
        Destroy(gameObject, 1.5f); //Will later change to an end of anim call to destroy the object
    }

    public int GetDistanceFromTile(Vector2Int tile){ // computes the distance from the tile to this unit
        Vector2Int result = tilePosition - tile;
        return Mathf.Abs(result.x) + Mathf.Abs(result.y); // cost
    }
    
    // returns the distance if in range, -1 otherwise
    public int CheckTileInRange(Vector2Int tile, int range){ 
        int ret = GetDistanceFromTile(tile);
        return (ret <= range) ? ret : -1;
    }

    // returns the distance if in movement range, -1 otherwise
    public int CheckTileInMoveRange(Vector2Int tile){ 
        return CheckTileInRange(tile, moveRange);
    }

    
    void OnMouseOver(){
        //UnitBaseClass mouseOverUnit = gameObject.GetComponent<UnitBaseClass>();
        menuUI.ShowOtherUnit(this);
        //Debug.Log(unitClass);
        //Debug.Log("HP: " + healthCurrent + "/" + healthMax);
        //Debug.Log("AP: " + armorCurrent);
        AttackingClass tempRef = gameObject.GetComponent<AttackingClass>();
        int dam = 0;
        if(tempRef != null){
            dam = tempRef.attackDamage;
        }
        //Debug.Log("Dmg: " + dam);
    } 
}

