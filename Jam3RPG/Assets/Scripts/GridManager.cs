using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

// Clean up code tasks
// selected could be of type unit base!
// Some how we need to render from up to right


enum SelectMode{
    IdleMode, // Nothing selected
    MoveMode, // Moving selected unit
    PickActionMode, // Picking action from UI
    ChooseTargetMode, // Choosing target for action
    SacrificeMode // Sacrificing??? Not sure if we need.
};

public class GridManager : MonoBehaviour
{

    private GameObject cursor;
    public Sprite valid;
    public Sprite invalid;

    private Grid tmap;

    private Vector2Int cursorTileCoords;// of cursor

    private GameObject selectedUnit = null;
    private List<KeyValuePair<string, bool>> selectedUnitActions = null;

    private Vector2Int pastTile;// of units
    private Vector3 pastPosition;

    // private (i think we should make a grid???)
    private SelectMode gridMode =  SelectMode.IdleMode;

    private string selectedUnitAction = "";

    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.Find("Cursor");
        tmap = GameObject.Find("Grid").GetComponent<Grid>();
        cursorTileCoords = new Vector2Int(0, 0);

        SetCursorPos(cursorTileCoords);

        // Snap all units to grid
        UnitBaseClass[] units = FindObjectsOfType<UnitBaseClass>();
        foreach(UnitBaseClass unit in units)
        {
            SnapUnitToGrid(unit);
        }
    }

    public void OnGridMovement(InputAction.CallbackContext context){ // Called when WASD is pressed... kinda useless
        //Debug.Log($"Movement {context.phase} {context.ReadValue<Vector2>()}");
        // Vector2Int curr_val = Vector2Int.CeilToInt(context.ReadValue<Vector2>());
        
        // switch (context.phase){
        //     case InputActionPhase.Started:
        //         break;
            
        //     case InputActionPhase.Performed:
        //         Vector3Int newval = new Vector3Int();
        //         newval.x = +curr_val.y;
        //         newval.y = -curr_val.x;
        //         //Debug.Log(newval);
        //         UpdateCursorPos(newval);
        //         break;
            
        //     case InputActionPhase.Canceled:

        //         break;
        // }
    }

    public void OnCursorMove(InputAction.CallbackContext context){
        //Debug.Log($"Movement {context.phase} {context.ReadValue<Vector2>()}");
        //Gets the tile that the mouse cursor (not the in-game one) is pointing at
        //Then, sets the in-game cursor to be hovering over that tile
        Vector2Int curr_val = Vector2Int.CeilToInt(context.ReadValue<Vector2>());
        Vector3 dest = Camera.main.ScreenToWorldPoint(new Vector3(curr_val.x, curr_val.y, Camera.main.nearClipPlane));
        Vector3Int tile = tmap.LocalToCell(dest);
        cursorTileCoords.x = tile.x;
        cursorTileCoords.y = tile.y;

        SetCursorPos(cursorTileCoords);

        if (gridMode == SelectMode.MoveMode){
            //Debug.Log(cursorTileCoords);
            UnitBaseClass unit = selectedUnit.GetComponent<UnitBaseClass>();
            UpdateCursorSprite(unit.tilePosition, unit.moveRange);

            // Check if the cursor is on a friendly sprite??? a small collision check

        } else if (gridMode == SelectMode.ChooseTargetMode){
            AttackingClass unit = selectedUnit.GetComponent<AttackingClass>();
            UpdateCursorSprite(unit.tilePosition, unit.attackRange);
        }
    }

    public void OnClickBoard(InputAction.CallbackContext context){ // Called when left mouse button is selected
        //Debug.Log("Click");
        
        if (context.phase == InputActionPhase.Started){
            // Cast a raycast
            RaycastHit hit;
            bool hasSelectedUnit = Physics.Raycast(Camera.main.transform.position, cursor.transform.position - Camera.main.transform.position, out hit);
            UnitBaseClass hitUnit = (!hasSelectedUnit) ? null : hit.collider.gameObject.GetComponent<UnitBaseClass>();
            
            //Debug.Log("Click");
            switch (gridMode){
                case SelectMode.IdleMode: // We don't have anything selected
                {
                    // If we selected a unit 
                    if (hitUnit && !hitUnit.turnOver){ //Selected a unit class? 
                        SetSelUnit(hit.collider.gameObject);
                        gridMode = SelectMode.MoveMode; // Moving units now...
                        Debug.Log("Moving to Move Mode!");
                    }
                    else {
                        Debug.Log("Selected nothing");
                    }
                } break;

                case SelectMode.MoveMode: // We have something selected
                {
                    UnitBaseClass unit = selectedUnit.GetComponent<UnitBaseClass>();
                    if (hitUnit){ // If we have touched another unit
                        if (selectedUnit == hit.collider.gameObject) // And it is self
                            ChangeToPickActionMode(); // Don't move. Just pick action.

                        else if (!hitUnit.turnOver) // And they are friendly
                            SetSelUnit(hit.collider.gameObject);

                    } else { // If empty tile selected that is within range
                        if (checkTileInRange(unit.tilePosition, cursorTileCoords, unit.moveRange)){
                            //Move selected unit and update position
                            unit.MoveToSpace(cursorTileCoords);
                            selectedUnit.transform.position = cursor.transform.position;
                            ChangeToPickActionMode();
                        }
                    }
                } break;
                
                //Case: Pick action mode!
                // 1 -> Action -> ChooseTargetMode -> IdleMode
                // 2 -> Wait -> IdleMode
                // 3 -> Sacrifice

                case SelectMode.ChooseTargetMode: // We are choosing a target
                {
                    // Assume actions only hit enemies...
                    /*We can add a special case later for the healer - Santi
                    Later we might need cases for unit actions that require a target that are also not attacking units*/
                    AttackingClass unit = selectedUnit.GetComponent<AttackingClass>();
                    if (checkTileInRange(unit.tilePosition, cursorTileCoords, unit.attackRange) && 
                            hitUnit && hitUnit.isEnemy != unit.isEnemy){ // check if target is in attack range and if target is on opposite team
                                UnitExecuteAction(hitUnit);
                                EndSelUnitTurn();
                    }
                } break;  
            }
        }
    }

    void ChangeToPickActionMode()
    {
        //Get unit's action list
        selectedUnitActions = selectedUnit.GetComponent<UnitBaseClass>().actions;
        Debug.Log("Picking Action Mode!");
        /*Because this is delared when MoveMode is ended, it doesn't show the action list again
         when canceling the target select.
        The list will not be deleted when going from PickTarget to PickAction, 
        but will when going from PickAction to Move*/
        foreach (KeyValuePair<string, bool> actionPair in selectedUnitActions)
        {
            Debug.Log("Available action: " + actionPair.Key);
        }
        gridMode = SelectMode.PickActionMode;
    }

    public void OnSelectOption(InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Started && gridMode == SelectMode.PickActionMode)

            /*switch(context.control.name){
                case "1": // Action
                    gridMode = SelectMode.ChooseTargetMode;
                    AttackingClass unit = selectedUnit.GetComponent<AttackingClass>();
                    UpdateCursorSprite(unit.tilePosition, unit.attackRange);*/
                    
        {
            string action = "";
            bool requiresTarget = false;
            switch (context.control.name)
            {
                case "1":
                    if(selectedUnitActions.Count > 0)
                    {
                        action = selectedUnitActions[0].Key;
                        requiresTarget = selectedUnitActions[0].Value;
                    }
                    break;
                case "2":
                    if (selectedUnitActions.Count > 1)
                    {
                        action = selectedUnitActions[1].Key;
                        requiresTarget = selectedUnitActions[1].Value;
                    }
                    break;
                case "3":
                    if (selectedUnitActions.Count > 2)
                    {
                        action = selectedUnitActions[2].Key;
                        requiresTarget = selectedUnitActions[2].Value;
                    }
                    break;
                case "4":
                    if (selectedUnitActions.Count > 3)
                    {
                        action = selectedUnitActions[3].Key;
                        requiresTarget = selectedUnitActions[3].Value;
                    }
                    break;
            };

            if(action != "") //Do not end action select if there is no action for the chosen value
            {
                Debug.Log("Doing action " + action);
                selectedUnitAction = action;
                //Note that wait action is always last (for user friendliness)
                if (requiresTarget)
                {
                    Debug.Log("Choose target mode!");
                    gridMode = SelectMode.ChooseTargetMode;
                    AttackingClass unit = selectedUnit.GetComponent<AttackingClass>();
                    UpdateCursorSprite(unit.tilePosition, unit.attackRange);
                }
                else
                {
                    UnitExecuteAction();
                    EndSelUnitTurn();
                }
            }
            //After selecting target, we execute the action
            //Can be done by seeing what the action was, then getting component of the class that action is defined in, and doing it.
        }
    }
    
    //After choosing an action in PickActionMode, executes that action
    private void UnitExecuteAction(UnitBaseClass target = null)
    {
        switch (selectedUnitAction)
        {
            case "Attack":
                //Get the AttackingClass component of selectedUnit (because it must have one in this case)
                target.ChangeHealth(-5); //<- Replace selectedUnit.GetComponent<AttackingClass>().Attack(target)
                break;
            case "Wait":
                break;
            default:
                break;
        }
    }

    public void OnUndoMove(InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Started){
            switch (gridMode){
                case SelectMode.MoveMode:{
                    // Unselect unit
                    selectedUnit.GetComponent<UnitBaseClass>().SpriteUnselect();
                    selectedUnit = null;
                    // Back to IdleMode
                    gridMode = SelectMode.IdleMode;
                    UpdateCursorSprite(cursorTileCoords, 0);
                    Debug.Log("Back to Idle");
                } break;

                case SelectMode.PickActionMode:{
                    // Unit goes back to past position
                    // Set to MoveMode (there is a possibility Idle mode makes more sense but...)
                    UnitBaseClass unit = selectedUnit.GetComponent<UnitBaseClass>();
                    selectedUnit.transform.position = pastPosition;
                    unit.MoveToSpace(pastTile);
                    selectedUnitActions = null;
                    gridMode = SelectMode.MoveMode;

                    UpdateCursorSprite(unit.tilePosition, unit.moveRange);

                    Debug.Log("Back to Move");
                } break;

                case SelectMode.ChooseTargetMode:{
                    // Unit Goes back to picking action
                    gridMode = SelectMode.PickActionMode;
                    UpdateCursorSprite(cursorTileCoords, 0);
                    Debug.Log("Back to PickAction");
                    foreach (KeyValuePair<string, bool> actionPair in selectedUnitActions)
                    {
                        Debug.Log("Available action: " + actionPair.Key);
                    }
                    } break;

            }
        }
    }

    public void SnapUnitToGrid(UnitBaseClass unit){
        // You have unit WORLD coordinates so use WorldToCell
        Vector3Int tile = tmap.LocalToCell(unit.transform.position);
        unit.tilePosition = new Vector2Int(tile.x, tile.y);
        tile.z = 0;

        // Then snap the unit to the tile!
        Vector3 dest = tmap.GetCellCenterLocal(tile);
        //Debug.Log(pos);
        dest.z = 0;
        unit.transform.position = dest;
    }



    private bool checkTileInRange(Vector2Int src, Vector2Int dst, int range){ // checks if given tile coordinate is in range
        // Get the relative tile position from the source
        Vector2Int diff = src - dst; 
        
        // Get abs val
        diff.x = Mathf.Abs(diff.x);
        diff.y = Mathf.Abs(diff.y);

        // get length of path
        int length = diff.x + diff.y;        
        //Debug.Log(length <= range);

        return length <= range;
    }

    private void SetSelUnit(GameObject unit){
        if (selectedUnit){
            selectedUnit.GetComponent<UnitBaseClass>().SpriteUnselect();
        }
        selectedUnit = unit;
        selectedUnit.GetComponent<UnitBaseClass>().SpriteSelect();
        pastTile = selectedUnit.GetComponent<UnitBaseClass>().tilePosition;
        pastPosition = selectedUnit.transform.position;
    }

    private void EndSelUnitTurn(){
        selectedUnit.GetComponent<UnitBaseClass>().FinishTurn(); 
        selectedUnit = null;
        gridMode = SelectMode.IdleMode;
    }

    private void UpdateCursorSprite(Vector2Int tilePosition, int range){ // updates cursor state based on cursor position
        if (gridMode == SelectMode.MoveMode){
            Collider[] hitColliders = new Collider[10];
            int numColliders = Physics.OverlapSphereNonAlloc(cursor.transform.position, 0.1f, hitColliders);
            //Debug.Log(numColliders);
            if (numColliders > 0 && !hitColliders[0].gameObject.GetComponent<UnitBaseClass>().turnOver){
                cursor.GetComponent<SpriteRenderer>().sprite = valid;
                return;
            }
        }
            
        if (!checkTileInRange(tilePosition, cursorTileCoords, range))
            cursor.GetComponent<SpriteRenderer>().sprite = invalid;
        else cursor.GetComponent<SpriteRenderer>().sprite = valid;
    }

    private void UpdateCursorPos(Vector2Int vIn){
        //cursorTileCoords
        cursorTileCoords += vIn;
        SetCursorPos(cursorTileCoords);
    }
    
    private void SetCursorPos(Vector2Int pos){
        Vector3 dest = tmap.GetCellCenterLocal(new Vector3Int(pos.x, pos.y, 0));
        //Debug.Log(pos);
        dest.z = 0;
        cursor.transform.position = dest;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
